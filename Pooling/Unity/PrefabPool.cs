using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public class PrefabPool<T> : Pool<T> where T : UnityEngine.Object
	{
		public T Prefab
		{
			get { return prefab; }
		}
		public Transform Transform
		{
			get { return transform; }
		}

		readonly T prefab;
		readonly Transform transform;

		public PrefabPool(T prefab, IInitializer<T> initializer = null, IStorage<T> storage = null)
			: this(prefab, new GameObject(prefab.name + " Pool").transform, initializer, storage) { }

		public PrefabPool(T prefab, Action<T> initializer, IStorage<T> storage = null)
			: this(prefab, initializer == null ? null : new MethodInitializer<T>(initializer), storage) { }

		PrefabPool(T prefab, Transform transform, IInitializer<T> initializer = null, IStorage<T> storage = null)
			: base(CreateFactory(prefab, transform), initializer ?? CreateInitializer(prefab, transform), storage)
		{
			this.prefab = prefab;
			this.transform = transform;
		}

		static IFactory<T> CreateFactory(T prefab, Transform transform)
		{
			if (prefab is GameObject)
				return (IFactory<T>)new GameObjectFactory(prefab as GameObject, transform);
			else if (prefab is Component)
				return new ComponentFactory<T>(prefab, transform);
			else
				return null;
		}

		static IInitializer<T> CreateInitializer(T prefab, Transform transform)
		{
			if (prefab is GameObject)
				return (IInitializer<T>)new GameObjectInitializer(transform);
			else if (prefab is Component)
				return new ComponentInitializer<T>(transform);
			else
				return null;
		}
	}
}
