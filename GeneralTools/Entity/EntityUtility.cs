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
		public static byte IdCount { get { return (byte)types.Count; } }

		static Dictionary<Component, IEntity> entities = new Dictionary<Component, IEntity>();
		static Dictionary<Type, byte> typeIds = new Dictionary<Type, byte>();
		static List<Type> types = new List<Type>();

		public static IEntity GetEntity(Component component)
		{
			IEntity entity;

			if (!entities.TryGetValue(component, out entity))
			{
				entity = component.GetComponentInParent<IEntity>();
				entities[component] = entity;
			}

			return entity;
		}

		public static void SetEntity(Component component, IEntity entity)
		{
			entities[component] = entity;
		}

		public static byte GetOrAddComponentId(Type componentType)
		{
			byte id;

			if (!typeIds.TryGetValue(componentType, out id))
			{
				id = (byte)types.Count;
				typeIds[componentType] = id;
				types.Add(componentType);

				if (types.Count >= byte.MaxValue)
					Debug.LogWarning("Maximum component identifier count reached.");
			}

			return id;
		}

		public static Type GetComponentType(byte id)
		{
			return types[id];
		}

		public static ByteFlag GetComponentFlags(Type componentType)
		{
			var flag = new ByteFlag();
			flag[GetOrAddComponentId(componentType)] = true;

			return flag;
		}

		public static ByteFlag GetComponentFlags(Type[] componentTypes)
		{
			var flag = new ByteFlag();

			for (int i = 0; i < componentTypes.Length; i++)
				flag[GetOrAddComponentId(componentTypes[i])] = true;

			return flag;
		}

		public static void ClearAll()
		{
			entities.Clear();
			typeIds.Clear();
			types.Clear();
		}
	}
}