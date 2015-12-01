using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public static class EntityUtility
	{
		static Dictionary<Type, int> typeIndices = new Dictionary<Type, int>(16);
		static List<Type> types = new List<Type>(16);
		static EntityJanitor janitor;

		public static int GetComponentIndex(Type type)
		{
			int id;

			if (!typeIndices.TryGetValue(type, out id))
			{
				if (!typeof(Component).IsAssignableFrom(type))
					throw new NotSupportedException(string.Format("Type {0} must inherit from {1}.", type.Name, typeof(Component).Name));

				id = types.Count;
				types.Add(type);
				typeIndices[type] = id;
			}

			return id;
		}

		public static Type GetComponentType(int index)
		{
			return types[index];
		}

		public static BitArray ToComponentBits(Type[] types)
		{
			var indices = new int[types.Length];

			for (int i = 0; i < types.Length; i++)
				indices[i] = GetComponentIndex(types[i]);

			var array = new BitArray(EntityUtility.types.Count);

			for (int i = 0; i < indices.Length; i++)
				array[indices[i]] = true;

			return array;
		}

		public static EntityGroup CreateEntityGroup(EntityGroup.Groups group, IEntityGroupMatcher matcher)
		{
			InitializeJanitor();

			var entityGroup = new EntityGroup();
			var allEntities = EntityManager.GetAllEntities();

			for (int i = 0; i < allEntities.Count; i++)
			{
				var entity = allEntities[i];

				if (matcher.Matches(entity.Group, group))
					entityGroup.Add(entity);
			}

			return entityGroup;
		}

		public static void ClearAllEntityGroups()
		{
			EntityManager.ClearAllEntityGroups();
			GC.Collect();
		}

		static void InitializeJanitor()
		{
			if (janitor == null)
				janitor = new GameObject("Entity Manager").AddComponent<EntityJanitor>();
		}
	}
}