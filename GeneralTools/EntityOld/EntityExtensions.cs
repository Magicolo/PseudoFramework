using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo
{
	public static class EntityExtensions
	{
		public static IEntityOld GetEntity(this Component component)
		{
			return EntityUtility.GetEntity(component);
		}
	}
}