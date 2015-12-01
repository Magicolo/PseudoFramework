using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public interface IEntityGroupMatcher
	{
		bool Matches(EntityGroup.Groups group1, EntityGroup.Groups group2);
	}
}