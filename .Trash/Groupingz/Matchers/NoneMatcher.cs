using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class NoneMatcher : AnyMatcher
	{
		public override bool Matches(IList<int> a, IList<int> b)
		{
			return !base.Matches(a, b);
		}
	}
}
