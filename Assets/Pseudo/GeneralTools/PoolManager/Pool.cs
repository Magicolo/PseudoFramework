using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class Pool : PMonoBehaviour
	{
		public event System.Action<Object> OnCreate;
		public event System.Action<Object> OnRecycle;

		[SerializeField]
		Object prefab;
		[SerializeField, Min]
		int startCount;

		bool isPoolable;
		bool isCopyable;
		bool isGameObject;
		bool isComponent;

		public Object Prefab { get { return prefab; } }
		public int StartCount { get { return startCount; } }
		public bool IsPoolable { get { return isPoolable; } }
		public bool IsCopyable { get { return isCopyable; } }
		public bool IsGameObject { get { return isGameObject; } }
		public bool IsComponent { get { return isComponent; } }

		readonly Queue<Object> pool = new Queue<Object>(4);
		readonly Queue<int> timeStamps = new Queue<int>(4);

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
			isGameObject = prefab is GameObject;
			isComponent = prefab is Component;

			for (int i = 0; i < startCount; i++)
				Recycle(GetItem());
		}

		public T Create<T>(Vector3 position = default(Vector3), Transform parent = null) where T : Object
		{
			T item = (T)GetItem();

			if (IsCopyable)
				((ICopyable<T>)item).Copy((T)Prefab);


			GameObject itemGameObject = GetItemGameObject(item);
			itemGameObject.SetActive(true);
			itemGameObject.transform.parent = parent == null ? CachedTransform : parent;
			itemGameObject.transform.position = position;

			if (IsPoolable)
				((IPoolable)item).OnCreate();

			RaiseOnCreateEvent(item);

			return item;
		}

		public void Recycle<T>(T item) where T : Object
		{
			if (item == null)
				return;

			GameObject itemGameObject = GetItemGameObject(item);
			itemGameObject.SetActive(false);
			itemGameObject.transform.parent = transform;

			if (IsPoolable)
				((IPoolable)item).OnRecycle();

			RaiseOnRecycleEvent(item);
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
			CachedTransform.DestroyChildren();
			pool.Clear();
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

		GameObject GetItemGameObject(Object item)
		{
			GameObject itemGameObject = null;

			if (IsGameObject)
				itemGameObject = item as GameObject;
			else if (IsComponent)
				itemGameObject = (item as Component).gameObject;

			return itemGameObject;
		}

		void RaiseOnCreateEvent(Object item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		void RaiseOnRecycleEvent(Object item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}
}