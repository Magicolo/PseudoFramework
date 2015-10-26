using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class Pool<T> where T : class, IPoolable, new()
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;

		static readonly Queue<T> _pool = new Queue<T>(4);

		static bool _isScriptableObject;

		static Pool()
		{
			_isScriptableObject = typeof(ScriptableObject).IsAssignableFrom(typeof(T));
		}

		public static T Create()
		{
			T item = GetItem();

			item.OnCreate();
			RaiseOnCreateEvent(item);

			return item;
		}

		public static TC Create<TC>(TC reference) where TC : class, T, ICopyable<T>
		{
			TC item = (TC)GetItem();

			if (reference != null)
				item.Copy(reference);

			item.OnCreate();
			RaiseOnCreateEvent(item);

			return item;
		}

		public static void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<T>
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = Create(array[i]);
		}

		public static void Recycle(T item)
		{
			if (item == null)
				return;

			item.OnRecycle();
			RaiseOnRecycleEvent(item);

#if UNITY_EDITOR
			if (_isScriptableObject && !Application.isPlaying)
				(item as ScriptableObject).Destroy();
			else
#endif
				_pool.Enqueue(item);
		}

		public static void Recycle(ref T item)
		{
			Recycle(item);

			item = null;
		}

		public static void RecycleElements(IList<T> array)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
			{
				Recycle(array[i]);
				array[i] = null;
			}
		}

		public static bool Contains(T item)
		{
			return _pool.Contains(item);
		}

		public static int Count()
		{
			return _pool.Count;
		}

		public static void Clear()
		{
			_pool.Clear();
		}

		static T GetItem()
		{
			T item = null;

			while (item == null)
			{
				if (_pool.Count > 0)
					item = _pool.Dequeue();
				else if (_isScriptableObject)
					item = ScriptableObject.CreateInstance(typeof(T)) as T;
				else
					item = new T();
			}

			return item;
		}

		static void RaiseOnCreateEvent(T item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		static void RaiseOnRecycleEvent(T item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}

	public static class BehaviourPool<T> where T : MonoBehaviour, IPoolable
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;
		public static int TickDelay = 2;
		public static Transform Transform { get { return _transform; } }

		static readonly Queue<T> _pool = new Queue<T>(4);
		static readonly Queue<int> _timeStamps = new Queue<int>(4);

		static Transform _transform;
		static bool _isCopyable;

		static BehaviourPool()
		{
			GameObject pools = GameObject.Find("Pools");

			if (pools == null)
				pools = new GameObject("Pools");

			_transform = pools.transform.AddChild(typeof(T).Name);
			_isCopyable = typeof(ICopyable<T>).IsAssignableFrom(typeof(T));
		}

		public static T Create(T reference, Action<T> onPreCreate = null)
		{
			T item = GetItem(reference);

			if (_isCopyable)
				((ICopyable<T>)item).Copy(reference);

			if (onPreCreate != null)
				onPreCreate(item);

			item.transform.localPosition = reference.transform.localPosition;
			item.transform.localRotation = reference.transform.localRotation;
			item.transform.localScale = reference.transform.localScale;
			item.gameObject.SetActive(true);
			item.OnCreate();

			RaiseOnCreateEvent(item);

			return item;
		}

		public static void CreateElements(IList<T> array, Action<T> onPreCreate = null)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = Create(array[i], onPreCreate);
		}

		public static void Recycle(T item, Action<T> onPostRecycle = null)
		{
			if (item == null)
				return;

			item.OnRecycle();
			item.gameObject.SetActive(false);
			item.transform.parent = _transform;

			if (onPostRecycle != null)
				onPostRecycle(item);

			RaiseOnRecycleEvent(item);
			_pool.Enqueue(item);
			_timeStamps.Enqueue(Time.frameCount);
		}

		public static void Recycle(ref T item, Action<T> onPostRecycle = null)
		{
			Recycle(item, onPostRecycle);

			item = null;
		}

		public static void RecycleElements(IList<T> array, Action<T> onPostRecycle = null)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
			{
				Recycle(array[i], onPostRecycle);
				array[i] = null;
			}
		}

		public static bool Contains(T item)
		{
			return _pool.Contains(item);
		}

		public static int Count()
		{
			return _pool.Count;
		}

		public static void Clear()
		{
			_pool.Clear();
		}

		static T GetItem(T prefab)
		{
			T item = null;

			while (item == null)
			{
				if (_pool.Count > 0 && Time.frameCount - _timeStamps.Peek() > TickDelay)
					item = _pool.Dequeue();
				else
				{
					item = UnityEngine.Object.Instantiate(prefab);
					item.transform.parent = _transform;
				}
			}

			return item;
		}

		static void RaiseOnCreateEvent(T item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		static void RaiseOnRecycleEvent(T item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}

	public static class ComponentPool<T> where T : Component
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;

		static readonly Queue<T> _pool = new Queue<T>(4);

		public static T Create(T reference)
		{
			T item = GetItem(reference);

			item.gameObject.SetActive(true);
			RaiseOnCreateEvent(item);

			return item;
		}

		public static void Recycle(T item)
		{
			if (item == null)
				return;

			item.gameObject.SetActive(false);
			RaiseOnRecycleEvent(item);

			_pool.Enqueue(item);
		}

		public static void Recycle(ref T item)
		{
			Recycle(item);

			item = null;
		}

		public static bool Contains(T item)
		{
			return _pool.Contains(item);
		}

		public static int Count()
		{
			return _pool.Count;
		}

		public static void Clear()
		{
			_pool.Clear();
		}

		static T GetItem(T reference)
		{
			T item = null;

			while (item == null)
			{
				if (_pool.Count > 0)
					item = _pool.Dequeue();
				else
					item = UnityEngine.Object.Instantiate(reference);
			}

			return item;
		}

		static void RaiseOnCreateEvent(T item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		static void RaiseOnRecycleEvent(T item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}
}