using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class ComponentPool<T> where T : Component
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;

		static readonly Queue<T> pool = new Queue<T>(4);

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

			pool.Enqueue(item);
		}

		public static void Recycle(ref T item)
		{
			Recycle(item);

			item = null;
		}

		public static bool Contains(T item)
		{
			return pool.Contains(item);
		}

		public static int Count()
		{
			return pool.Count;
		}

		public static void Clear()
		{
			pool.Clear();
		}

		static T GetItem(T reference)
		{
			T item = null;

			while (item == null)
			{
				if (pool.Count > 0)
					item = pool.Dequeue();
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