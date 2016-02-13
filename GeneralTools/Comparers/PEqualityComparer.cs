using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Comparison;

namespace Pseudo
{
	public abstract class PEqualityComparer<T> : IEqualityComparer<T>
	{
		public static IEqualityComparer<T> Default
		{
			get { return defaultComparer; }
		}

		static readonly IEqualityComparer<T> defaultComparer = CreateComparer();

		static IEqualityComparer<T> CreateComparer()
		{
			if (typeof(T).IsEnum)
			{
				var type = Enum.GetUnderlyingType(typeof(T));

				if (type == typeof(byte))
					return new EnumEqualityComparer<T, byte>();
				else if (type == typeof(sbyte))
					return new EnumEqualityComparer<T, sbyte>();
				else if (type == typeof(ushort))
					return new EnumEqualityComparer<T, ushort>();
				else if (type == typeof(short))
					return new EnumEqualityComparer<T, short>();
				else if (type == typeof(uint))
					return new EnumEqualityComparer<T, uint>();
				else if (type == typeof(int))
					return new EnumEqualityComparer<T, int>();
				else if (type == typeof(ulong))
					return new EnumEqualityComparer<T, ulong>();
				else if (type == typeof(long))
					return new EnumEqualityComparer<T, long>();
			}

			return EqualityComparer<T>.Default;
		}

		public abstract bool Equals(T x, T y);
		public abstract int GetHashCode(T obj);
	}
}
