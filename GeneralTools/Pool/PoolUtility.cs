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

		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }

		static readonly Lazy<GameObject> cachedGameObject = new Lazy<GameObject>(() =>
		{
			InitializeJanitor();
			return PoolJanitor.Instance.CachedGameObject;
		});
		static readonly Lazy<Transform> cachedTransform = new Lazy<Transform>(() => cachedGameObject.Value.transform);

		public static IPool CreateTypePool(Type type, int startSize)
		{
			IPool pool;

			if (typeof(Component).IsAssignableFrom(type))
			{
				var gameObject = new GameObject(type.Name);
				var reference = gameObject.AddComponent(type);
				gameObject.SetActive(false);

				Transform poolTransform = null;

				if (ApplicationUtility.IsPlaying)
				{
					poolTransform = Transform.AddChild(reference.name + " Pool");
					reference.transform.parent = poolTransform;
				}

				var poolType = typeof(ComponentPool<>).MakeGenericType(type);
				pool = (IPool)Activator.CreateInstance(poolType, reference, poolTransform, startSize);
			}
			else if (typeof(GameObject).IsAssignableFrom(type))
			{
				var reference = new GameObject(type.Name);
				reference.SetActive(false);

				Transform poolTransform = null;

				if (ApplicationUtility.IsPlaying)
				{
					poolTransform = Transform.AddChild(reference.name + " Pool");
					reference.transform.parent = poolTransform;
				}

				pool = new GameObjectPool(reference, poolTransform, startSize);
			}
			else if (typeof(ScriptableObject).IsAssignableFrom(type))
			{
				var poolType = typeof(ScriptablePool<>).MakeGenericType(type);
				pool = (IPool)Activator.CreateInstance(poolType, startSize);
			}
			else
			{
				var reference = Activator.CreateInstance(type);
				var poolType = typeof(Pool<>).MakeGenericType(type);
				pool = (IPool)Activator.CreateInstance(poolType, reference, startSize);
			}

			return pool;
		}

		public static IPool CreatePrefabPool(object reference, int startSize)
		{
			IPool pool;

			if (reference is Component)
			{
				var component = (Component)reference;
				Transform poolTransform = null;

				if (ApplicationUtility.IsPlaying)
					poolTransform = Transform.AddChild(component.name + " Pool");

				var poolType = typeof(ComponentPool<>).MakeGenericType(reference.GetType());
				pool = (IPool)Activator.CreateInstance(poolType, reference, poolTransform, startSize);
			}
			else if (reference is GameObject)
			{
				var gameObject = (GameObject)reference;
				Transform poolTransform = null;

				if (ApplicationUtility.IsPlaying)
					poolTransform = Transform.AddChild(gameObject.name + " Pool");

				pool = new GameObjectPool(gameObject, poolTransform, startSize);
			}
			else if (reference is ScriptableObject)
			{
				var poolType = typeof(ScriptablePool<>).MakeGenericType(reference.GetType());
				pool = (IPool)Activator.CreateInstance(poolType, reference, startSize);
			}
			else
			{
				var poolType = typeof(Pool<>).MakeGenericType(reference.GetType());
				pool = (IPool)Activator.CreateInstance(poolType, reference, startSize);
			}

			return pool;
		}

		public static void InitializeFields(object instance, IPoolSetter[] setters)
		{
			bool isInitializable = instance is IPoolInitializable;

			if (isInitializable)
				((IPoolInitializable)instance).OnPrePoolInitialize();

			for (int i = 0; i < setters.Length; i++)
				setters[i].SetValue(instance);

			if (isInitializable)
				((IPoolInitializable)instance).OnPostPoolInitialize();
		}

		public static void Resize(IList array, Type elementType, int length)
		{
			var defaultValue = TypeUtility.GetDefaultValue(elementType);

			while (array.Count > length)
				array.RemoveAt(array.Count - 1);

			while (array.Count < length)
				array.Add(defaultValue);
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
			if (ApplicationUtility.IsPlaying && PoolJanitor.Instance == null)
				new GameObject("Pool Manager").AddComponent<PoolJanitor>();
		}

		public static IPoolInitializer GetInitializer(object instance)
		{
			var copier = CopyUtility.GetCopier(instance.GetType());

			if (copier == null)
				return new PoolInitializer(GetSetters(instance, new List<object> { instance }));
			else
				return new PoolCopierInitializer(copier, instance);
		}

		static IPoolSetter[] GetSetters(object instance, List<object> toIgnore)
		{
			var type = instance.GetType();
			var allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var setters = new List<IPoolSetter>(allFields.Length);
			bool isInitializable = instance is IPoolSettersInitializable;

			if (isInitializable)
				((IPoolSettersInitializable)instance).OnPrePoolSettersInitialize();

			for (int i = 0; i < allFields.Length; i++)
			{
				var field = allFields[i];
				var value = field.GetValue(instance);

				if (ShouldInitialize(field, value))
					setters.Add(GetSetter(value, field, toIgnore));
			}

			if (isInitializable)
				((IPoolSettersInitializable)instance).OnPostPoolSettersInitialize(setters);

			return setters.ToArray();
		}

		static IPoolSetter GetSetter(object value, FieldInfo field, List<object> toIgnore)
		{
			if (value == null)
				return new PoolSetter(field, value);
			else if (toIgnore.Contains(value))
				throw new InitializationCycleException(field);

			var copier = CopyUtility.GetCopier(value.GetType());

			if (copier != null)
				return new PoolCopierSetter(copier, field, value);
			else if (value is IList)
				return new PoolArraySetter(field, value.GetType(), GetElementSetters((IList)value, field, toIgnore));
			else if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				if (!(value is ValueType))
					toIgnore.Add(value);

				return new PoolContentSetter(field, value.GetType(), GetSetters(value, toIgnore));
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
			if (element == null)
				return new PoolElementSetter(element);
			else if (toIgnore.Contains(element))
				throw new InitializationCycleException(field);

			if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				if (!(element is ValueType))
					toIgnore.Add(element);

				return new PoolElementContentSetter(element.GetType(), GetSetters(element, toIgnore));
			}
			else
				return new PoolElementSetter(element);
		}

		static bool ShouldInitialize(FieldInfo field, object value)
		{
			if (value == null)
				return true;
			else if (field.IsDefined(typeof(InitializeValueAttribute), true) || field.IsDefined(typeof(InitializeContentAttribute), true))
				return true;
			else if (field.IsDefined(typeof(DoNotInitializeAttribute), true))
				return false;
			else if (field.IsInitOnly || field.IsBackingField())
				return false;
			else if ((value is UnityEngine.Object) && (field.IsPublic || field.IsDefined(typeof(SerializeField), true)) && field.DeclaringType.IsDefined(typeof(SerializableAttribute), true))
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
}