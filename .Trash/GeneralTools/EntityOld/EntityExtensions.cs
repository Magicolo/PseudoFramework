using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo.Internal.EntityOld
{
	public static class EntityExtensionsOld
	{
		public static IEntityOld GetEntity(this Component component)
		{
			return EntityUtility.GetEntity(component);
		}
	}
}