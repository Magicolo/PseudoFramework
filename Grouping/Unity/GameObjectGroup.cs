using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class GameObjectGroup : Group<IGameObjectElement>, IGameObjectGroup
	{
		public MatchType Match
		{
			get { return match; }
		}
		public Type[] Filter
		{
			get { return filter; }
		}

		readonly MatchType match;
		readonly Type[] filter;

		public GameObjectGroup(MatchType match, params Type[] filter)
			: base(new GameObjectTemplate(filter), CreateMatcher(match))
		{
			this.match = match;
			this.filter = filter;
		}

		public bool Contains(GameObject element)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].Element == element)
					return true;
			}

			return false;
		}

		static IMatcher<IGameObjectElement> CreateMatcher(MatchType match)
		{
			switch (match)
			{
				default:
				case MatchType.All:
					return new AllMatcher<IGameObjectElement>();
				case MatchType.Any:
					return new AnyMatcher<IGameObjectElement>();
				case MatchType.None:
					return new NoneMatcher<IGameObjectElement>();
			}
		}
	}
}
