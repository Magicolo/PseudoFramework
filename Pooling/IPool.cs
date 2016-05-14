using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public interface IPool<T> : IPool where T : class
	{
		new IFactory<T> Factory { get; }
		new IInitializer<T> Initializer { get; }
		new IStorage<T> Storage { get; }

		new T Create();
		bool Recycle(T instance);
	}

	public interface IPool
	{
		Type Type { get; }
		IFactory Factory { get; }
		IInitializer Initializer { get; }
		IStorage Storage { get; }

		object Create();
		bool Recycle(object instance);
	}
}
