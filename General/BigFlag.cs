using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo
{
	[Serializable]
	public struct BigFlag : IEquatable<BigFlag>
	{
		[SerializeField]
		ByteFlag f1;
		[SerializeField]
		ByteFlag f2;
		[SerializeField]
		ByteFlag f3;
		[SerializeField]
		ByteFlag f4;
		[SerializeField]
		ByteFlag f5;
		[SerializeField]
		ByteFlag f6;
		[SerializeField]
		ByteFlag f7;
		[SerializeField]
		ByteFlag f8;

		public static readonly BigFlag Nothing = new BigFlag(ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing, ByteFlag.Nothing);
		public static readonly BigFlag Everything = new BigFlag(ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything, ByteFlag.Everything);
		public const int MaxValue = 2048;

		public BigFlag(BigFlag flags)
		{
			f1 = flags.f1;
			f2 = flags.f2;
			f3 = flags.f3;
			f4 = flags.f4;
			f5 = flags.f5;
			f6 = flags.f6;
			f7 = flags.f7;
			f8 = flags.f8;
		}

		public BigFlag(int value) : this()
		{
			Add(value);
		}

		public BigFlag(int value1, int value2) : this(value1)
		{
			Add(value2);
		}

		public BigFlag(int value1, int value2, int value3) : this(value1, value2)
		{
			Add(value3);
		}

		public BigFlag(int value1, int value2, int value3, int value4) : this(value1, value2, value3)
		{
			Add(value4);
		}

		public BigFlag(params int[] values) : this()
		{
			for (int i = 0; i < values.Length; i++)
				Add(values[i]);
		}

		BigFlag(ByteFlag flag1, ByteFlag flag2, ByteFlag flag3, ByteFlag flag4, ByteFlag flag5, ByteFlag flag6, ByteFlag flag7, ByteFlag flag8)
		{
			f1 = flag1;
			f2 = flag2;
			f3 = flag3;
			f4 = flag4;
			f5 = flag5;
			f6 = flag6;
			f7 = flag7;
			f8 = flag8;
		}

		public bool this[int index]
		{
			get { return Get(index); }
			set { Set(index, value); }
		}

		public BigFlag Add(int value)
		{
			var shift = (byte)value;

			if (value < 256)
				f1.Add(shift);
			else if (value < 512)
				f2.Add(shift);
			else if (value < 768)
				f3.Add(shift);
			else if (value < 1024)
				f4.Add(shift);
			else if (value < 1280)
				f5.Add(shift);
			else if (value < 1536)
				f6.Add(shift);
			else if (value < 1792)
				f7.Add(shift);
			else
				f8.Add(shift);

			return this;
		}

		public BigFlag Remove(int value)
		{
			var shift = (byte)value;

			if (value < 256)
				f1.Remove(shift);
			else if (value < 512)
				f2.Remove(shift);
			else if (value < 768)
				f3.Remove(shift);
			else if (value < 1024)
				f4.Remove(shift);
			else if (value < 1280)
				f5.Remove(shift);
			else if (value < 1536)
				f6.Remove(shift);
			else if (value < 1792)
				f7.Remove(shift);
			else
				f8.Remove(shift);

			return this;
		}

		public int[] ToArray()
		{
			var indices = new List<int>();
			indices.AddRange(f1.ToArray().Convert(b => (int)b));
			indices.AddRange(f2.ToArray().Convert(b => b + 256));
			indices.AddRange(f3.ToArray().Convert(b => b + 512));
			indices.AddRange(f4.ToArray().Convert(b => b + 768));
			indices.AddRange(f5.ToArray().Convert(b => b + 1024));
			indices.AddRange(f6.ToArray().Convert(b => b + 1280));
			indices.AddRange(f7.ToArray().Convert(b => b + 1536));
			indices.AddRange(f8.ToArray().Convert(b => b + 1792));

			return indices.ToArray();
		}

		bool Get(int index)
		{
			var shift = (byte)index;

			if (index < 32)
				return f1[shift];
			else if (index < 64)
				return f2[shift];
			else if (index < 96)
				return f3[shift];
			else if (index < 128)
				return f4[shift];
			else if (index < 160)
				return f5[shift];
			else if (index < 192)
				return f6[shift];
			else if (index < 224)
				return f7[shift];
			else
				return f8[shift];
		}

		void Set(int index, bool value)
		{
			if (value)
				Add(index);
			else
				Remove(index);
		}

		public override int GetHashCode()
		{
			return
				f1.GetHashCode() ^
				f2.GetHashCode() ^
				f3.GetHashCode() ^
				f4.GetHashCode() ^
				f5.GetHashCode() ^
				f6.GetHashCode() ^
				f7.GetHashCode() ^
				f8.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is BigFlag))
				return false;

			return Equals((BigFlag)obj);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}

		public bool Equals(BigFlag other)
		{
			return this == other;
		}

		public static BigFlag operator ~(BigFlag a)
		{
			a.f1 = ~a.f1;
			a.f2 = ~a.f2;
			a.f3 = ~a.f3;
			a.f4 = ~a.f4;
			a.f5 = ~a.f5;
			a.f6 = ~a.f6;
			a.f7 = ~a.f7;
			a.f8 = ~a.f8;

			return a;
		}

		public static BigFlag operator |(BigFlag a, BigFlag b)
		{
			a.f1 |= b.f1;
			a.f2 |= b.f2;
			a.f3 |= b.f3;
			a.f4 |= b.f4;
			a.f5 |= b.f5;
			a.f6 |= b.f6;
			a.f7 |= b.f7;
			a.f8 |= b.f8;

			return a;
		}

		public static BigFlag operator &(BigFlag a, BigFlag b)
		{
			a.f1 &= b.f1;
			a.f2 &= b.f2;
			a.f3 &= b.f3;
			a.f4 &= b.f4;
			a.f5 &= b.f5;
			a.f6 &= b.f6;
			a.f7 &= b.f7;
			a.f8 &= b.f8;

			return a;
		}

		public static BigFlag operator ^(BigFlag a, BigFlag b)
		{
			a.f1 ^= b.f1;
			a.f2 ^= b.f2;
			a.f3 ^= b.f3;
			a.f4 ^= b.f4;
			a.f5 ^= b.f5;
			a.f6 ^= b.f6;
			a.f7 ^= b.f7;
			a.f8 ^= b.f8;

			return a;
		}

		public static bool operator ==(BigFlag a, BigFlag b)
		{
			return
				a.f1 == b.f1 &&
				a.f2 == b.f2 &&
				a.f3 == b.f3 &&
				a.f4 == b.f4 &&
				a.f5 == b.f5 &&
				a.f6 == b.f6 &&
				a.f7 == b.f7 &&
				a.f8 == b.f8;
		}

		public static bool operator !=(BigFlag a, BigFlag b)
		{
			return
				a.f1 != b.f1 ||
				a.f2 != b.f2 ||
				a.f3 != b.f3 ||
				a.f4 != b.f4 ||
				a.f5 != b.f5 ||
				a.f6 != b.f6 ||
				a.f7 != b.f7 ||
				a.f8 != b.f8;
		}
	}

	public struct BigFlag<T> : IEquatable<BigFlag<T>> where T : struct, IConvertible
	{
		BigFlag flags;

		public static BigFlag<T> Nothing
		{
			get { return new BigFlag<T>(BigFlag.Nothing); }
		}

		public static BigFlag<T> Everything
		{
			get { return new BigFlag<T>(BigFlag.Everything); }
		}

		public BigFlag(BigFlag flags)
		{
			this.flags = flags;
		}

		public BigFlag(T flag)
		{
			flags = new BigFlag(flag.ToInt32(null));
		}

		public BigFlag(params T[] flags)
		{
			this.flags = new BigFlag();

			for (int i = 0; i < flags.Length; i++)
				Add(flags[i]);
		}

		public bool this[T index]
		{
			get { return flags[index.ToInt32(null)]; }
			set { flags[index.ToInt32(null)] = value; }
		}

		public BigFlag<T> Add(T flag)
		{
			flags.Add(flag.ToInt32(null));

			return this;
		}

		public BigFlag<T> Remove(T flag)
		{
			flags.Remove(flag.ToInt32(null));

			return this;
		}

		public override int GetHashCode()
		{
			return flags.GetHashCode();
		}

		public bool Equals(BigFlag<T> other)
		{
			return flags.Equals(other.flags);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is BigFlag<T>))
				return false;

			return Equals((BigFlag<T>)obj);
		}

		public T[] ToArray()
		{
			return flags.ToArray().Convert(b => (T)Enum.ToObject(typeof(T), b));
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}

		public static BigFlag<T> operator ~(BigFlag<T> a)
		{
			return new BigFlag<T>(~a.flags);
		}

		public static BigFlag<T> operator |(BigFlag<T> a, BigFlag<T> b)
		{
			return new BigFlag<T>(a.flags | b.flags);
		}

		public static BigFlag<T> operator |(BigFlag a, BigFlag<T> b)
		{
			return new BigFlag<T>(a | b.flags);
		}

		public static BigFlag<T> operator |(BigFlag<T> a, BigFlag b)
		{
			return new BigFlag<T>(a.flags | b);
		}

		public static BigFlag<T> operator &(BigFlag<T> a, BigFlag<T> b)
		{
			return new BigFlag<T>(a.flags & b.flags);
		}

		public static BigFlag<T> operator &(BigFlag a, BigFlag<T> b)
		{
			return new BigFlag<T>(a & b.flags);
		}

		public static BigFlag<T> operator &(BigFlag<T> a, BigFlag b)
		{
			return new BigFlag<T>(a.flags & b);
		}

		public static BigFlag<T> operator ^(BigFlag<T> a, BigFlag<T> b)
		{
			return new BigFlag<T>(a.flags ^ b.flags);
		}

		public static BigFlag<T> operator ^(BigFlag a, BigFlag<T> b)
		{
			return new BigFlag<T>(a ^ b.flags);
		}

		public static BigFlag<T> operator ^(BigFlag<T> a, BigFlag b)
		{
			return new BigFlag<T>(a.flags ^ b);
		}

		public static bool operator ==(BigFlag<T> a, BigFlag<T> b)
		{
			return a.flags == b.flags;
		}

		public static bool operator ==(BigFlag a, BigFlag<T> b)
		{
			return a == b.flags;
		}

		public static bool operator ==(BigFlag<T> a, BigFlag b)
		{
			return a.flags == b;
		}

		public static bool operator !=(BigFlag<T> a, BigFlag<T> b)
		{
			return a.flags != b.flags;
		}

		public static bool operator !=(BigFlag a, BigFlag<T> b)
		{
			return a != b.flags;
		}

		public static bool operator !=(BigFlag<T> a, BigFlag b)
		{
			return a.flags != b;
		}

		public static implicit operator BigFlag(BigFlag<T> a)
		{
			return a.flags;
		}

		public static implicit operator BigFlag<T>(BigFlag a)
		{
			return new BigFlag<T>(a);
		}
	}
}