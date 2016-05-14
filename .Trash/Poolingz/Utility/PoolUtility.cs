using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Pooling.Internal
{
	public static class PoolUtility
	{
		public static readonly List<Action> ToUpdate = new List<Action>();
		public static PoolJanitor Janitor { get { return cachedJanitor.Value; } }
		public static Transform Transform { get { return Janitor.transform; } }

		static readonly Lazy<PoolJanitor> cachedJanitor = new Lazy<PoolJanitor>(() =>
		{
			InitializeJanitor();
			return PoolJanitor.Instance;
		});

		public static IPool CreateTypePool(Type type, int startSize = 0)
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

		public static IPool CreatePrefabPool(object reference, int startSize = 0)
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
			ToUpdate.Clear();
			TypePoolManager.ClearPools();
			PrefabPoolManager.ClearPools();
			cachedJanitor.Reset();
		}

		public static void InitializeJanitor()
		{
			if (ApplicationUtility.IsPlaying && PoolJanitor.Instance == null)
				new GameObject("Pool Manager").AddComponent<PoolJanitor>();
		}

		public static IFieldInitializer GetPoolInitializer(object instance)
		{
			//var copier = CopyUtility.GetCopier(instance.GetType());

			//if (copier == null)
				return new FieldInitializer(instance);
			//else
			//	return new CopierFieldInitializer(copier, instance);
		}
	}
}