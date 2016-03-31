using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public interface IPoolUpdater
	{
		bool Updating { get; }
		IFieldInitializer Initializer { get; set; }

		void Update();
		void Enqueue(object instance, bool initialize);
		object Dequeue();
		void Clear();
		void Reset();
	}
}
