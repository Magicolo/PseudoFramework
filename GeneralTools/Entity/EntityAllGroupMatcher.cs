using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityAllGroupMatcher : IEntityGroupMatcher
	{
		public bool Matches(EntityMatch.Groups group1, EntityMatch.Groups group2)
		{
			return (~group1 & group2) == 0;
		}
	}
}