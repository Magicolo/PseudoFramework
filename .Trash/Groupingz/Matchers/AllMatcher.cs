using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class AllMatcher : IMatcher
	{
		public bool Matches(IList<int> a, IList<int> b)
		{
			if (b.Count == 0)
				return true;
			else if (a.Count < b.Count)
				return false;
			else if (a.Count == 1 && b.Count == 1)
				return a[0] == b[0];
			else if (a[0] > b[b.Count - 1])
				return false;

			int lastId = a[a.Count - 1];

			if (lastId < b[0])
				return false;

			int lastIndex = 0;
			for (int i = 0; i < b.Count; i++)
			{
				int id2 = b[i];
				bool contains = false;

				if (id2 > lastId)
					return false;

				for (int j = lastIndex; j < a.Count; j++)
				{
					int id1 = a[j];

					if (id1 == id2)
					{
						contains = true;
						lastIndex = j + 1;
						break;
					}
				}

				if (!contains)
					return false;
			}

			return true;
		}
	}
}
