using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityExactGroupMatcher : IEntityGroupMatcher
	{
		public bool Matches(EntityGroup.Groups group1, EntityGroup.Groups group2)
		{
			return (group1 ^ group2) == 0;
		}
	}
}