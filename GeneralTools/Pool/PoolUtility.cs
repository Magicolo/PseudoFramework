using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;
using System.Runtime.Serialization;

namespace Pseudo.Internal.Pool
{
	public static class PoolUtility
	{
		public enum CollectionTypes
		{
			None,
			Array,
			List
		}

		static readonly CachedValue<GameObject> cachedGameObject = new CachedValue<GameObject>(() => new GameObject("Pool Manager"));
		static readonly CachedValue<Transform> cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);

		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }

		public static Pool CreatePool(Type type, int startSize, Transform parent = null)
		{
			return CreatePool(Activator.CreateInstance(type), startSize, parent);
		}

		public static Pool CreatePool(object reference, int startSize, Transform parent = null)
		{
			Pool pool;

			if (reference is Component)
			{
				var behaviourPool = new ComponentPool((Component)reference, startSize);
				behaviourPool.Transform.parent = parent == null ? Transform : parent;
				pool = behaviourPool;
			}
			else if (reference is GameObject)
			{
				var gameObjectPool = new GameObjectPool((GameObject)reference, startSize);
				gameObjectPool.Transform.parent = parent == null ? Transform : parent;
				pool = gameObjectPool;
			}
			else if (reference is ScriptableObject)
				pool = new ScriptablePool((ScriptableObject)reference, startSize);
			else
				pool = new Pool(reference, startSize);

			return pool;
		}

		public static void InitializeFields(object instance, List<IPoolSetter> setters)
		{
			for (int i = 0; i < setters.Count; i++)
				setters[i].SetValue(instance);
		}

		public static void Resize(IList array, int length)
		{
			while (array.Count > length)
				array.RemoveAt(array.Count - 1);

			while (array.Count < length)
				array.Add(null);
		}

		public static List<IPoolSetter> GetSetters(object instance)
		{
			var type = instance.GetType();
			var allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var fields = new List<IPoolSetter>(allFields.Length);

			for (int i = 0; i < allFields.Length; i++)
			{
				FieldInfo field = allFields[i];
				object value = field.GetValue(instance);

				if (ShouldInitialize(field))
					fields.Add(GetSetter(value, field));
			}

			return fields;
		}

		static IPoolSetter GetSetter(object value, FieldInfo field)
		{
			if (value == null)
				return new PoolSetter(field, value);

			if (value is IList)
				return new PoolArraySetter(field, GetElementSetters((IList)value, field));
			else if (field.IsDefined(typeof(InitializeContentAttribute), true))
				return new PoolContentSetter(field, GetSetters(value));
			else
				return new PoolSetter(field, value);
		}

		static List<IPoolElementSetter> GetElementSetters(IList array, FieldInfo field)
		{
			var setters = new List<IPoolElementSetter>(array.Count);

			for (int i = 0; i < array.Count; i++)
				setters.Add(GetElementSetter(array[i], field));

			return setters;
		}

		static IPoolElementSetter GetElementSetter(object element, FieldInfo field)
		{
			if (element != null && field.IsDefined(typeof(InitializeContentAttribute), true))
				return new PoolElementContentSetter(GetSetters(element));
			else
				return new PoolElementSetter(element);
		}

		static bool ShouldInitialize(FieldInfo field)
		{
			return !field.IsInitOnly && !field.IsDefined(typeof(DoNotInitializeAttribute), true) && !field.IsBackingField();
		}
	}
}