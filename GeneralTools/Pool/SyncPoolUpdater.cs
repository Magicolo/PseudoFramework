using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class SyncPoolUpdater : PoolUpdaterBase
	{
		Action initializeInstances;

		public override IFieldInitializer Initializer
		{
			get { return initializer; }
			set { initializer = value; }
		}

		public SyncPoolUpdater()
		{
			initializeInstances = InitializeInstances;
		}

		public override void Update()
		{
			if (!Updating && initializer != null)
			{
				Updating = true;
				PoolUtility.ToUpdate.Add(initializeInstances);
			}
		}

		public override void Enqueue(object instance, bool initialize)
		{
			if (initialize)
				toInitialize.Enqueue(instance);
			else
				instances.Enqueue(instance);
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
					initializer.InitializeFields(instance);
					instances.Enqueue(instance);
				}

				return;
			}

			PoolUtility.ToUpdate.Remove(initializeInstances);
			Updating = false;
		}
	}
}
