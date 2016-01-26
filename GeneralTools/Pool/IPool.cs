using System;
using System.Collections;

namespace Pseudo
{
	public interface IPool
	{
		Type Type { get; }
		int Size { get; }

		object Create();
		T CreateCopy<T>(T reference) where T : class, ICopyable;
		void Recycle(object instance);
		void RecycleElements<T>(T elements) where T : class, IList;
		void Clear();
		bool Contains(object instance);
		void Reset();
	}
}