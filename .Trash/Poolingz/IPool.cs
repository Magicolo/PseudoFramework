using System;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Pooling
{
	public interface IPool<T> : IPool where T : class
	{
		new T Create();
		void Recycle(T instance);
		void RecycleElements(IList<T> elements);
		bool Contains(T instance);
	}

	public interface IPool
	{
		Type Type { get; }
		int Size { get; }

		object Create();
		void Recycle(object instance);
		void RecycleElements(IList elements);
		void Clear();
		bool Contains(object instance);
		void Reset();
	}
}