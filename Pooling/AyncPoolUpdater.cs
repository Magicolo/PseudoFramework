using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Threading;

namespace Pseudo.Pooling.Internal
{
	public class AsyncPoolUpdater : PoolUpdaterBase
	{
		static Thread updateThread;
		static readonly List<AsyncPoolUpdater> toUpdate = new List<AsyncPoolUpdater>();

		public override IFieldInitializer Initializer
		{
			get { return initializer; }
			set
			{
				if (initializer == null)
					initializer = value;
				else
					lock (initializer) initializer = value;
			}
		}

		object updateLock = new object();

		public override void Update()
		{
			lock (updateLock)
			{
				if (!Updating && initializer != null)
				{
					InitializeUpdateThread();
					lock (updateLock) Updating = true;
					lock (toUpdate) toUpdate.Add(this);
				}
			}
		}

		public override void Enqueue(object instance, bool initialize)
		{
			if (initialize)
				lock (toInitialize) { toInitialize.Enqueue(instance); }
			else
				lock (instances) { instances.Enqueue(instance); }
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
		}

		void InitializeInstances()
		{
			int count = Mathf.Max((instances.Count + toInitialize.Count) / 2, 1);

			for (int i = 0; i < count; i++)
			{
				object instance;

				lock (toInitialize)
				{
					if (toInitialize.Count == 0)
					{
						lock (updateLock) Updating = false;
						return;
					}

					instance = toInitialize.Dequeue();
				}

				lock (initializer) initializer.InitializeFields(instance);
				lock (instances) instances.Enqueue(instance);
			}
		}

		static void InitializeUpdateThread()
		{
			if (updateThread == null || updateThread.ThreadState == ThreadState.Stopped)
			{
				updateThread = new Thread(InitializeAsync);
				updateThread.Start();
			}
		}

		static void InitializeAsync()
		{
			try
			{
				while (ApplicationUtility.IsPlaying)
				{
					for (int i = toUpdate.Count - 1; i >= 0; i--)
					{
						AsyncPoolUpdater updater;

						lock (toUpdate)
						{
							if (toUpdate.Count > i)
							{
								updater = toUpdate[i];

								if (updater == null || !updater.Updating)
								{
									toUpdate.RemoveAt(i);
									continue;
								}
							}
							else
								break;
						}

						updater.InitializeInstances();
					}

					Thread.Sleep(100);
				}
			}
			catch (Exception e) { Debug.LogException(e); }
		}
	}
}
