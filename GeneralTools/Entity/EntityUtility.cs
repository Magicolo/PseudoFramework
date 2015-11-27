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
		static readonly Dictionary<Type, int> typeIdentifiers = new Dictionary<Type, int>
		{
			{typeof(Transform), 0 },
			{typeof(BoxCollider2D), 1 },
			{typeof(CircleCollider2D), 2 },
			{typeof(PolygonCollider2D), 3 },
			{typeof(Rigidbody2D), 4 },
			{typeof(SpriteRenderer), 5 },
			{typeof(LineRenderer), 6 },
			{typeof(ParticleSystem), 7 },
			{typeof(ParticleSystemRenderer), 8 },
		};

		static EntityUtility()
		{
			var types = typeof(PComponent).GetSubclasses();

			for (int i = 0; i < types.Length; i++)
				typeIdentifiers[types[i]] = typeIdentifiers.Count;
		}

		public static int GetTotalComponentCount()
		{
			return typeIdentifiers.Count;
		}

		public static int GetComponentIndex(Type type)
		{
			int index;

			if (!typeIdentifiers.TryGetValue(type, out index))
				Debug.LogError(string.Format("Type {0} has no registered identifier.", type.Name));

			return index;
		}
	}
}