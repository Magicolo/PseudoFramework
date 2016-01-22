using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class EntityExtensions
	{
		public static IEntity First(this IEntityGroup group)
		{
			return group.Count > 0 ? group[0] : null;
		}

		public static IEntity Last(this IEntityGroup group)
		{
			return group.Count > 0 ? group[group.Count - 1] : null;
		}
	}
}