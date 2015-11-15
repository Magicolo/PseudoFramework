using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Threading;
using Pseudo.Internal;
using System;

namespace Pseudo
{
	public class BehaviourPool
	{
		static readonly List<BehaviourPool> toUpdate = new List<BehaviourPool>();
		static Thread updadeThread;

		public PMonoBehaviour Prefab { get; private set; }
		public int Size { get; private set; }
		public GameObject GameObject { get { return cachedGameObject; } }
		public Transform Transform { get { return cachedTransform; } }

		protected readonly Queue<PMonoBehaviour> instances = new Queue<PMonoBehaviour>();
		protected readonly Queue<PMonoBehaviour> toInitialize = new Queue<PMonoBehaviour>();
		protected readonly CachedValue<GameObject> cachedGameObject;
		protected readonly CachedValue<Transform> cachedTransform;

		protected List<IPoolFieldData> fields;
		protected bool updating;

		static BehaviourPool()
		{
			updadeThread = new Thread(UpdateAsync);
			updadeThread.Start();
		}

		public BehaviourPool(PMonoBehaviour prefab, int startSize = 8)
		{
			Prefab = prefab;

			instances = new Queue<PMonoBehaviour>(startSize);
			toInitialize = new Queue<PMonoBehaviour>(startSize);
			cachedGameObject = new CachedValue<GameObject>(() => new GameObject(prefab.name + " Pool"));
			cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);

			Initialize(startSize);
		}

		public PMonoBehaviour Create()
		{
			PMonoBehaviour instance = GetInstance();
			instance.Prefab = Prefab;
			instance.CachedTransform.Copy(Prefab.CachedTransform);
			instance.OnCreate();

			return instance;
		}

		public void Recycle(PMonoBehaviour instance)
		{
			if (instance == null)
				return;

			instance.OnRecycle();
			Enqueue(instance, true);
			Subscribe(this);
		}

		public bool Contains(PMonoBehaviour instance)
		{
			lock (instances)
			{
				if (instances.Contains(instance))
					return true;
			}

			lock (toInitialize)
			{
				if (toInitialize.Contains(instance))
					return true;
			}

			return false;
		}

		public void Clear()
		{
			lock (instances)
			{
				while (instances.Count > 0)
					instances.Dequeue().CachedGameObject.Destroy();
			}

			lock (toInitialize)
			{
				while (toInitialize.Count > 0)
					toInitialize.Dequeue().CachedGameObject.Destroy();
			}
		}

		void Initialize(int startSize)
		{
			for (int i = 0; i < startSize; i++)
				Enqueue(CreateInstance(), false);

			ThreadPool.QueueUserWorkItem(InitializeAsync);
		}

		void InitializeAsync(object state)
		{
			fields = PoolUtility.GetFieldData(Prefab);
		}

		protected virtual PMonoBehaviour GetInstance()
		{
			PMonoBehaviour instance;

			do
			{
				if (instances.Count > 0)
					instance = Dequeue();
				else
					instance = CreateInstance();
			}
			while (instance == null);

			return instance;
		}

		protected virtual PMonoBehaviour CreateInstance()
		{
			return UnityEngine.Object.Instantiate(Prefab);
		}

		protected virtual void Enqueue(PMonoBehaviour instance, bool initialize)
		{
			instance.CachedGameObject.SetActive(false);
			instance.CachedTransform.parent = Transform;

			if (initialize)
				lock (toInitialize) { toInitialize.Enqueue(instance); }
			else
				lock (instances) { instances.Enqueue(instance); }

			Size++;
		}

		protected virtual PMonoBehaviour Dequeue()
		{
			PMonoBehaviour instance;
			lock (instances) { instance = instances.Dequeue(); }
			instance.CachedGameObject.SetActive(true);
			Size--;

			return instance;
		}

		static void Subscribe(BehaviourPool pool)
		{
			if (pool.updating)
				return;

			pool.updating = true;
			toUpdate.Add(pool);
		}

		static void Unsubscribe(BehaviourPool pool)
		{
			if (!pool.updating)
				return;

			lock (toUpdate)
			{
				pool.updating = false;
			}
		}

		static void UpdateAsync()
		{
			try
			{
				while (true)
				{
					for (int i = toUpdate.Count - 1; i >= 0; i--)
					{
						BehaviourPool pool;

						lock (toUpdate)
						{
							if (toUpdate.Count > i)
								pool = toUpdate[i];
							else
								return;
						}

						if (pool.updating)
							UpdatePoolAsync(pool);
						else
							toUpdate.RemoveAt(i);
					}

					Thread.Sleep(100);
				}
			}
			catch (Exception e) { Debug.LogError(e); }
		}

		static void UpdatePoolAsync(BehaviourPool pool)
		{
			int count = Mathf.Max(pool.Size / 10, 1);

			for (int i = 0; i < count; i++)
			{
				PMonoBehaviour instance;

				lock (pool.toInitialize)
				{
					if (pool.toInitialize.Count > 0)
						instance = pool.toInitialize.Dequeue();
					else
					{
						Unsubscribe(pool);
						return;
					}
				}

				PoolUtility.InitializeFields(instance, pool.fields);
				lock (pool.instances) pool.instances.Enqueue(instance);
			}
		}
	}
}