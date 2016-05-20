using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Groupingz.Internal;

namespace Pseudo.Groupingz
{
	public class GroupManager<TElement, TId> : IGroupManager<TElement, TId>
	{
		public IGroupFactory<TElement> GroupFactory
		{
			get { return groupFactory; }
		}
		public IConverter<TId, int> Converter
		{
			get { return converter; }
		}

		readonly IGroupFactory<TElement> groupFactory;
		readonly IConverter<TId, int> converter;
		readonly List<IElementInfo<TElement>> infos = new List<IElementInfo<TElement>>();
		readonly List<IGroup<TElement>> groups = new List<IGroup<TElement>>();
		readonly Dictionary<TElement, IElementInfo<TElement>> elementToInfo = new Dictionary<TElement, IElementInfo<TElement>>();
		readonly Dictionary<Filter, IGroup<TElement>> filterToGroup = new Dictionary<Filter, IGroup<TElement>>();

		public GroupManager(IGroupFactory<TElement> groupFactory = null, IConverter<TId, int> converter = null)
		{
			this.groupFactory = groupFactory ?? new GroupFactory<TElement>();
			this.converter = converter ?? new Converter<TId>();
		}

		IElementInfo<TElement> GetInfo(TElement element)
		{
			IElementInfo<TElement> info;

			if (!elementToInfo.TryGetValue(element, out info))
			{
				info = CreateInfo(element);
				infos.Add(info);
				elementToInfo[element] = info;
			}

			return info;
		}

		IElementInfo<TElement> CreateInfo(TElement element)
		{
			var info = new ElementInfo<TElement>(element);
			Update(info);

			return info;
		}

		IGroup<TElement> GetGroup(Filter filter)
		{
			IGroup<TElement> group;

			if (!filterToGroup.TryGetValue(filter, out group))
			{
				group = CreateGroup(filter);
				groups.Add(group);
				filterToGroup[filter] = group;
			}

			return group;
		}

		IGroup<TElement> CreateGroup(Filter filter)
		{
			var group = groupFactory.Create(filter.Match, filter.Identifiers);

			for (int i = 0; i < infos.Count; i++)
				group.Update(infos[i]);

			return group;
		}

		void Update(IElementInfo<TElement> info)
		{
			for (int i = 0; i < groups.Count; i++)
				groups[i].Update(info);
		}

		// Because of a mysterious bug, these public methods need to be after the private ones or Unity will not compile...
		public IGroup<TElement> GetGroup(MatchType match, params TId[] filters)
		{
			return GetGroup(new Filter(match, filters
				.Select(f => converter.ConvertTo(f))
				.OrderBy(v => v)
				.ToArray()));
		}

		public void Register(TElement element, TId identifier)
		{
			var info = GetInfo(element);
			info.Add(converter.ConvertTo(identifier));

			Update(info);
		}

		public void Register(TElement element, params TId[] identifiers)
		{
			var info = GetInfo(element);

			for (int i = 0; i < identifiers.Length; i++)
				info.Add(converter.ConvertTo(identifiers[i]));

			Update(info);
		}

		public void Unregister(TElement element, TId identifier)
		{
			var info = GetInfo(element);

			if (info.Remove(converter.ConvertTo(identifier)))
				Update(info);
		}

		public void Unregister(TElement element, params TId[] identifiers)
		{
			var info = GetInfo(element);
			bool success = false;

			for (int i = 0; i < identifiers.Length; i++)
				success |= info.Remove(converter.ConvertTo(identifiers[i]));

			if (success)
				Update(info);
		}
	}
}
