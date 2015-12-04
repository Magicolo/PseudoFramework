using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;
using System.Runtime.Serialization;
using System.Threading;

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

		public static bool IsPlaying;
		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }

		static readonly CachedValue<GameObject> cachedGameObject = new CachedValue<GameObject>(() =>
		{
			InitializeJanitor();
			return PoolJanitor.Instance.CachedGameObject;
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

		public static void ClearAllPools()
		{
			TypePoolManager.ClearPools();
			PrefabPoolManager.ClearPools();
			cachedGameObject.Reset();
			cachedTransform.Reset();
			GC.Collect();
		}

		public static void InitializeJanitor()
		{
			if (Application.isPlaying && PoolJanitor.Instance == null)
				new GameObject("Pool Manager").AddComponent<PoolJanitor>();
		}

		public static List<IPoolSetter> GetSetters(object instance)
		{
			return GetSetters(instance, new List<object>());
		}

		static List<IPoolSetter> GetSetters(object instance, List<object> toIgnore)
		{
			var type = instance.GetType();
			var allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var fields = new List<IPoolSetter>(allFields.Length);

			for (int i = 0; i < allFields.Length; i++)
			{
				FieldInfo field = allFields[i];
				object value = field.GetValue(instance);

				if (ShouldInitialize(field))
					fields.Add(GetSetter(value, field, toIgnore));
			}

			return fields;
		}

		static IPoolSetter GetSetter(object value, FieldInfo field, List<object> toIgnore)
		{
			if (value == null || value.GetType().IsValueType)
				return new PoolSetter(field, value);
			else if (toIgnore.Contains(value))
				throw new InitializationCycleException(field);

			if (value is IList)
				return new PoolArraySetter(field, GetElementSetters((IList)value, field, toIgnore));
			else if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				toIgnore.Add(value);
				return new PoolContentSetter(field, GetSetters(value, toIgnore));
			}
			else
				return new PoolSetter(field, value);
		}

		static List<IPoolElementSetter> GetElementSetters(IList array, FieldInfo field, List<object> toIgnore)
		{
			var setters = new List<IPoolElementSetter>(array.Count);

			for (int i = 0; i < array.Count; i++)
				setters.Add(GetElementSetter(array[i], field, toIgnore));

			return setters;
		}

		static IPoolElementSetter GetElementSetter(object element, FieldInfo field, List<object> toIgnore)
		{
			if (element == null || element.GetType().IsValueType)
				return new PoolElementSetter(element);
			else if (toIgnore.Contains(element))
				throw new InitializationCycleException(field);

			if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				toIgnore.Add(element);
				return new PoolElementContentSetter(GetSetters(element, toIgnore));
			}
			else
				return new PoolElementSetter(element);
		}

		static bool ShouldInitialize(FieldInfo field)
		{
			if (field.IsDefined(typeof(InitializeValueAttribute), true) || field.IsDefined(typeof(InitializeContentAttribute), true))
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

	public class InitializationCycleException : Exception
	{
		public InitializationCycleException(FieldInfo field) :
			base(string.Format("Initialization cycle detected on field {0}. You might be initializing the content of a field that references back to the its owner.", field.DeclaringType.Name + "." + field.Name))
		{ }
	}

	public class TypeMismatchException : Exception
	{
		public TypeMismatchException(string message) : base(message) { }
	}
}