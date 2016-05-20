using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class ExactMatcher : IMatcher
	{
		public bool Matches(IList<int> a, IList<int> b)
		{
			if (a.Count != b.Count)
				return false;
			else if (a.Count == 0 && b.Count == 0)
				return true;
			else if (a.Count == 1 && b.Count == 1)
				return a[0] == b[0];

			for (int i = 0; i < b.Count; i++)
			{
				if (a[i] != b[i])
					return false;
			}

			return true;
		}
	}
}
