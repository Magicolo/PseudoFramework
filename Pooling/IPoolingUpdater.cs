using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO
{
	public interface IPoolingUpdater
	{
		IPool Pool { get; }
		bool Updating { get; }

		void Enqueue(object instance);
		object Dequeue();
		void Clear();
		void Reset();
	}
}
