using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Threading;

namespace Pseudo.Pooling2.Internal
{
	public class AsyncPoolingUpdater : PoolingUpdaterBase
	{
		object updateLock = new object();
		Action updateAsync;
		AsyncCallback endUpdateAsync;

		public AsyncPoolingUpdater(IPool pool) : base(pool)
		{
			updateAsync = UpdateAsync;
			endUpdateAsync = EndUpdateAsync;
		}

		public override void Enqueue(object instance)
		{
			lock (toInitialize) toInitialize.Enqueue(instance);

			BeginUpdateAsync();
		}

		public override object Dequeue()
		{
			lock (instances)
			{
				if (instances.Count > 0)
					return instances.Dequeue();
				else
					return null;
			}
		}

		public override void Clear()
		{
			lock (instances) instances.Clear();
			lock (toInitialize) toInitialize.Clear();
			lock (updateLock) Updating = false;
		}

		public override void Reset()
		{
			lock (toInitialize)
			{
				lock (instances)
				{
					while (instances.Count > 0)
						toInitialize.Enqueue(instances.Dequeue());
				}
			}

			BeginUpdateAsync();
		}

		void BeginUpdateAsync()
		{
			lock (updateLock)
			{
				if (!Updating)
				{
					Updating = true;
					updateAsync.BeginInvoke(endUpdateAsync, null);
				}
			}
		}

		void UpdateAsync()
		{
			while (toInitialize.Count > 0)
			{
				object instance;

				lock (toInitialize)
				{
					if (toInitialize.Count == 0)
						break;

					instance = toInitialize.Dequeue();
				}

				Pool.Initializer.Initialize(instance);
				lock (instances) instances.Enqueue(instance);
			}

		}

		void EndUpdateAsync(IAsyncResult result)
		{
			updateAsync.EndInvoke(result);
			lock (updateLock) Updating = false;
		}
	}
}
