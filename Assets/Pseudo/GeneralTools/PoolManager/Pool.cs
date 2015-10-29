using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Pool")]
	public class Pool : PMonoBehaviour
	{
		[SerializeField]
		protected Object prefab;
		[SerializeField, Min]
		protected int startCount;
		protected bool isPoolable;
		protected bool isCopyable;

		protected readonly Queue<Object> pool = new Queue<Object>(4);
		protected readonly Queue<int> timeStamps = new Queue<int>(4);

		public Object Prefab { get { return prefab; } }
		public int StartCount { get { return startCount; } }

		protected virtual void Awake()
		{
			if (prefab != null)
				Initialize(prefab, startCount);
		}

		public virtual void Initialize(Object prefab, int startCount)
		{
			Clear();

			this.prefab = prefab;
			this.startCount = startCount;
			isPoolable = prefab is IPoolable;
			isCopyable = typeof(ICopyable<>).MakeGenericType(prefab.GetType()).IsAssignableFrom(prefab.GetType());

			for (int i = 0; i < startCount; i++)
				Recycle(GetItem());
		}

		public virtual T Create<T>(Vector3 position = default(Vector3), Transform parent = null) where T : Object
		{
			T item = (T)GetItem();

			if (isCopyable)
				((ICopyable<T>)item).Copy((T)prefab);


			GameObject itemGameObject = PoolManager.GetGameObject(item);
			itemGameObject.SetActive(true);
			itemGameObject.transform.parent = parent == null ? CachedTransform : parent;
			itemGameObject.transform.position = position;

			if (isPoolable)
				((IPoolable)item).OnCreate();

			return item;
		}

		public virtual void Recycle<T>(T item) where T : Object
		{
			if (item == null)
				return;

			GameObject itemGameObject = PoolManager.GetGameObject(item);
			itemGameObject.SetActive(false);
			itemGameObject.transform.parent = transform;

			if (isPoolable)
				((IPoolable)item).OnRecycle();

			pool.Enqueue(item);
			timeStamps.Enqueue(Time.frameCount);
		}

		public virtual bool Contains<T>(T item) where T : Object
		{
			return pool.Contains(item);
		}

		public virtual int Count()
		{
			return pool.Count;
		}

		public virtual void Clear()
		{
			while (pool.Count > 0)
				pool.Dequeue().Destroy();

			timeStamps.Clear();
		}

		protected virtual Object GetItem()
		{
			Object item = null;

			while (item == null)
			{
				if (pool.Count > 0 && Time.frameCount - timeStamps.Peek() > 1)
					item = pool.Dequeue();
				else
					item = Instantiate(Prefab);
			}

			return item;
		}
	}
}