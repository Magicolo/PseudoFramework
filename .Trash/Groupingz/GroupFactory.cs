using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class GroupFactory<TElement> : FactoryBase<MatchType, IList<int>, IGroup<TElement>>, IGroupFactory<TElement>
	{
		readonly IMatcher[] matchers =
	   {
			new AllMatcher(),
			new AnyMatcher(),
			new NoneMatcher(),
			new ExactMatcher()
		};

		public override IGroup<TElement> Create(MatchType argument1, IList<int> argument2)
		{
			return new Group<TElement>(matchers[(int)argument1], argument2);
		}
	}
}
