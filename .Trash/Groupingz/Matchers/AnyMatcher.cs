using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class AnyMatcher : IMatcher
	{
		public virtual bool Matches(IList<int> a, IList<int> b)
		{
			if (b.Count == 0)
				return true;
			else if (a.Count == 0)
				return false;
			else if (a.Count == 1 && b.Count == 1)
				return a[0] == b[0];
			else if (a[0] == b[0])
				return true;
			else if (a[0] > b.Last() || a.Last() < b[0])
				return false;

			int lastId = a.Last();
			int lastIndex = 0;
			for (int i = 0; i < b.Count; i++)
			{
				int id2 = b[i];

				if (id2 > lastId)
					return false;

				for (int j = lastIndex; j < a.Count; j++)
				{
					int id1 = a[j];

					if (id1 == id2)
						return true;
					else if (id2 > id1)
						lastIndex = j + 1;
				}
			}

			return false;
		}
	}
}
