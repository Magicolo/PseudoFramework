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
		public Transform Root
		{
			get { return root; }
		}

		readonly T prefab;
		readonly Transform root;

		public PrefabPool(T prefab, Transform root, IInitializer<T> initializer = null, IStorage<T> storage = null)
			: this(prefab, root, new PrefabFactory<T>(prefab), initializer, storage) { }

		public PrefabPool(T prefab, Transform root, Action<T> initializer, IStorage<T> storage = null)
			: this(prefab, root, initializer == null ? null : new MethodInitializer<T>(initializer), storage) { }

		PrefabPool(T prefab, Transform root, IFactory<T> factory, IInitializer<T> initializer = null, IStorage<T> storage = null)
			: base(factory, initializer ?? new PrefabInitializer<T>(), storage ?? new PrefabStorage<T>(root, factory))
		{
			this.prefab = prefab;
			this.root = root;
		}
	}
}
