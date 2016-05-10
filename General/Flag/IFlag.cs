using System;
using System.Collections.Generic;

namespace Pseudo
{
	public interface IFlag<T, TFlag> : IFlag<T>
	{
		T Add(TFlag flag);
		T Subtract(TFlag flag);
		bool Has(TFlag flag);
	}

	public interface IFlag<T> : IEquatable<T>
	{
		T Add(T flags);
		T Subtract(T flags);
		bool HasAll(T flags);
		bool HasAny(T flags);
		bool HasNone(T flags);
		T Not();
		T And(T other);
		T Or(T other);
		T Xor(T other);
	}
}