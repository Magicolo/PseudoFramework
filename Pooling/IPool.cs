using System;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Pooling2
{
	public interface IPool<T> : IPool where T : class
	{
		new IPoolingFactory<T> Factory { get; }

		new T Create();
		void Recycle(T instance);
		bool Contains(T instance);
	}

	/// <summary>
	/// Creates and recycles instances of a given type.
	/// </summary>
	public interface IPool
	{
		IPoolingFactory Factory { get; }
		IPoolingUpdater Updater { get; }
		IInitializer Initializer { get; }
		int Size { get; }
		Type Type { get; }

		object Create();
		void Recycle(object instance);
		bool Contains(object instance);
		void Clear();
		void Reset();
	}
}