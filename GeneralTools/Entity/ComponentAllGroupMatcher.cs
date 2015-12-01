using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class ComponentAllGroupMatcher : IComponentGroupMatcher
	{
		public bool Matches(PEntity entity, BitArray componentBits)
		{
			for (int i = 0; i < componentBits.Count; i++)
			{
				if (componentBits[i] && !entity.HasComponent(EntityUtility.GetComponentType(i)))
					return false;
			}

			return true;
		}
	}
}