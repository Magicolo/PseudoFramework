﻿using UnityEngine;
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

		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }

		static readonly CachedValue<GameObject> cachedGameObject = new CachedValue<GameObject>(() =>
		{
			var poolManager = new GameObject("Pool Manager");
			poolManager.AddComponent<PoolJanitor>();
			return poolManager;
		});
		static readonly CachedValue<Transform> cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);

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

				if (Application.isPlaying)
					behaviourPool.Transform.parent = parent == null ? Transform : parent;

				pool = behaviourPool;
			}
			else if (reference is GameObject)
			{
				var gameObjectPool = new GameObjectPool((GameObject)reference, startSize);

				if (Application.isPlaying)
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

		public static void ClearAllPools()
		{
			TypePoolManager.ClearPools();
			PrefabPoolManager.ClearPools();
			cachedGameObject.Reset();
			cachedTransform.Reset();
			GC.Collect();
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
			if (field.IsDefined(typeof(InitializeValueAttribute), true))
				return true;
			else if (field.IsDefined(typeof(DoNotInitializeAttribute), true))
				return false;
			else if (field.IsInitOnly || field.IsBackingField())
				return false;
			else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType) && field.IsPublic && field.DeclaringType.IsDefined(typeof(SerializableAttribute), true))
				return false;
			else
				return true;
		}
	}
}