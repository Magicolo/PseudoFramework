using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class Group<TElement> : IGroup<TElement>
	{
		public event Action<TElement> OnAdded = delegate { };
		public event Action<TElement> OnRemoved = delegate { };
		public IMatcher Matcher
		{
			get { return matcher; }
		}
		public int Count
		{
			get { return elements.Count; }
		}

		public TElement this[int index]
		{
			get { return elements[index]; }
		}

		readonly IMatcher matcher;
		readonly IList<int> filter;
		readonly HashSet<TElement> hashedElements = new HashSet<TElement>();
		readonly List<TElement> elements = new List<TElement>();

		public Group(IMatcher matcher, IList<int> filter)
		{
			this.matcher = matcher;
			this.filter = filter;
		}

		public bool Update(IElementInfo<TElement> info)
		{
			if (matcher.Matches(info.Identifiers, filter))
			{
				Add(info.Element);
				return true;
			}
			else
			{
				Remove(info.Element);
				return false;
			}
		}

		public bool Contains(TElement element)
		{
			return hashedElements.Contains(element);
		}

		public int IndexOf(TElement element)
		{
			return elements.IndexOf(element);
		}

		public TElement Find(Predicate<TElement> match)
		{
			return elements.Find(match);
		}

		public int FindIndex(Predicate<TElement> match)
		{
			return elements.FindIndex(match);
		}

		public TElement[] ToArray()
		{
			return elements.ToArray();
		}

		public void CopyTo(TElement[] array, int index = 0)
		{
			elements.CopyTo(array, index);
		}

		public List<TElement>.Enumerator GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		void Add(TElement element)
		{
			if (hashedElements.Add(element))
			{
				elements.Add(element);
				OnAdded(element);
			}
		}

		void Remove(TElement element)
		{
			if (hashedElements.Remove(element))
			{
				elements.Remove(element);
				OnRemoved(element);
			}
		}

		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}
	}
}
