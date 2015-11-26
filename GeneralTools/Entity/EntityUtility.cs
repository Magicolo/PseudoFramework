using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public static class EntityUtility
	{
		static readonly Dictionary<Type, int> typeIdentifiers = new Dictionary<Type, int>();

		static EntityUtility()
		{
			var types = typeof(Component).GetSubclasses();

			for (int i = 0; i < types.Length; i++)
				typeIdentifiers[types[i]] = i;
		}

		public static int GetTotalComponentCount()
		{
			return typeIdentifiers.Count;
		}

		public static int GetComponentIndex(Type type)
		{
			return typeIdentifiers[type];
		}
	}
}