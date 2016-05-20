using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class AllMatcher<T> : IMatcher<T>
	{
		public virtual bool Matches(T value, ITemplate<T> template)
		{
			for (int i = 0; i < template.Elements.Length; i++)
			{
				if (!template.Elements[i].Matches(value))
					return false;
			}

			return true;
		}
	}
}
