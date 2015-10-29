using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class Pool : PMonoBehaviour
	{
		[SerializeField]
		Object prefab;
		[SerializeField, Min]
		int startCount;
		bool isPoolable;
		bool isCopyable;

		readonly Queue<Object> pool = new Queue<Object>(4);
		readonly Queue<int> timeStamps = new Queue<int>(4);

		public Object Prefab { get { return prefab; } }
		public int StartCount { get { return startCount; } }

		void Awake()
		{
			if (prefab != null)
				Initialize(prefab, startCount);
		}

		public void Initialize(Object prefab, int startCount)
		{
			Clear();

			this.prefab = prefab;
			this.startCount = startCount;
			isPoolable = prefab is IPoolable;
			isCopyable = typeof(ICopyable<>).MakeGenericType(prefab.GetType()).IsAssignableFrom(prefab.GetType());

			for (int i = 0; i < startCount; i++)
				Recycle(GetItem());
		}

		public T Create<T>(Vector3 position = default(Vector3), Transform parent = null) where T : Object
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

		public void Recycle<T>(T item) where T : Object
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

		public bool Contains<T>(T item) where T : Object
		{
			return pool.Contains(item);
		}

		public int Count()
		{
			return pool.Count;
		}

		public void Clear()
		{
			while (pool.Count > 0)
				pool.Dequeue().Destroy();

			timeStamps.Clear();
		}

		Object GetItem()
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