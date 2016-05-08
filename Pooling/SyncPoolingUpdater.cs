using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO.Internal
{
	public class SyncPoolingUpdater : PoolingUpdaterBase
	{
		public SyncPoolingUpdater(IPool pool) : base(pool) { }

		public override void Enqueue(object instance)
		{
			toInitialize.Enqueue(instance);

			RegisterUpdate();
		}

		public override object Dequeue()
		{
			if (instances.Count > 0)
				return instances.Dequeue();
			else
				return null;
		}

		public override void Clear()
		{
			instances.Clear();
			toInitialize.Clear();
			Updating = false;
		}

		public override void Reset()
		{
			while (instances.Count > 0)
				toInitialize.Enqueue(instances.Dequeue());

			RegisterUpdate();
		}

		void RegisterUpdate()
		{
			if (!Updating)
			{
				Updating = true;

				if (Application.isPlaying)
					ApplicationUtility.OnUpdate += InitializeInstances;
#if UNITY_EDITOR
				else
					UnityEditor.EditorApplication.update += InitializeInstances;
#endif
			}
		}

		void UnregisterUpdate()
		{
			if (Updating)
			{
				Updating = false;

				if (Application.isPlaying)
					ApplicationUtility.OnUpdate -= InitializeInstances;
#if UNITY_EDITOR
				else
					UnityEditor.EditorApplication.update -= InitializeInstances;
#endif
			}
		}

		void InitializeInstances()
		{
			while (toInitialize.Count > 0)
			{
				int count = Mathf.Max((instances.Count + toInitialize.Count) / 4, Mathf.Min(toInitialize.Count, 1));

				for (int i = 0; i < count; i++)
				{
					if (toInitialize.Count == 0)
						break;

					var instance = toInitialize.Dequeue();
					Pool.Initializer.Initialize(instance);
					instances.Enqueue(instance);
				}

				return;
			}

			UnregisterUpdate();
		}
	}
}
