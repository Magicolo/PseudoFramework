using System;
using System.Collections;

namespace Pseudo.Pooling
{
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