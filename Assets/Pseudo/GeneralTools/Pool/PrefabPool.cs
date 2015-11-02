using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public abstract class PrefabPool<T> : PoolBase<T> where T : Object
	{
		protected T prefab;
		protected readonly Queue<int> timeStamps = new Queue<int>();
		protected readonly CachedValue<GameObject> cachedGameObject;
		protected readonly CachedValue<Transform> cachedTransform;

		public GameObject GameObject { get { return cachedGameObject; } }
		public Transform Transform { get { return cachedTransform; } }

		protected PrefabPool(T prefab, int startCount = 0) : base(startCount)
		{
			this.prefab = prefab;

			cachedGameObject = new CachedValue<GameObject>(() => new GameObject(prefab.name));
			cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);
		}

		public virtual T Create(Vector3 position, Transform parent = null)
		{
			T item = base.Create();
			Transform itemTransform = GetTransform(item);

			if (parent != null)
				itemTransform.parent = parent;

			itemTransform.position = position;

			return item;
		}

		public override void Clear()
		{
			while (pool.Count > 0)
				pool.Dequeue().Destroy();

			timeStamps.Clear();
		}

		protected abstract GameObject GetGameObject(T item);

		protected abstract Transform GetTransform(T item);

		protected override T CreateItem()
		{
			T item = Object.Instantiate(prefab);
			GetTransform(item).parent = cachedTransform.Value;
			GetGameObject(item).SetActive(true);

			return item;
		}

		protected override bool CanDequeue()
		{
			return base.CanDequeue() && Time.frameCount - timeStamps.Peek() > 1;
		}

		protected override void Enqueue(T item)
		{
			if (Application.isPlaying)
			{
				timeStamps.Enqueue(Time.frameCount);
				GetTransform(item).parent = cachedTransform.Value;
				GetGameObject(item).SetActive(false);

				base.Enqueue(item);
			}
			else
				item.Destroy();
		}

		protected override T Dequeue()
		{
			T item = base.Dequeue();
			timeStamps.Dequeue();
			GetGameObject(item).SetActive(true);

			return item;
		}
	}
}