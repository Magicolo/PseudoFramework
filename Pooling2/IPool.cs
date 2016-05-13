using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public interface IPool<T> : IPool where T : class
	{
		new IStorage<T> Storage { get; }
		new IInitializer<T> Initializer { get; }

		new T Create();
		bool Recycle(T instance);
	}

	public interface IPool
	{
		Type Type { get; }
		IStorage Storage { get; }
		IInitializer Initializer { get; }

		object Create();
		bool Recycle(object instance);
	}
}
