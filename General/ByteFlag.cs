using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Text;

namespace Pseudo
{
	[Serializable]
	public struct ByteFlag : IEquatable<ByteFlag>
	{
		[SerializeField]
		int f1;
		[SerializeField]
		int f2;
		[SerializeField]
		int f3;
		[SerializeField]
		int f4;
		[SerializeField]
		int f5;
		[SerializeField]
		int f6;
		[SerializeField]
		int f7;
		[SerializeField]
		int f8;

		public static readonly ByteFlag Nothing = new ByteFlag(0, 0, 0, 0, 0, 0, 0, 0);
		public static readonly ByteFlag Everything = new ByteFlag(-1, -1, -1, -1, -1, -1, -1, -1);

		public ByteFlag(byte flag) : this()
		{
			Add(flag);
		}

		public ByteFlag(byte flag1, byte flag2) : this(flag1)
		{
			Add(flag2);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3) : this(flag1, flag2)
		{
			Add(flag3);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3, byte flag4) : this(flag1, flag2, flag3)
		{
			Add(flag4);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3, byte flag4, byte flag5) : this(flag1, flag2, flag3, flag4)
		{
			Add(flag5);
		}

		public ByteFlag(params byte[] flags) : this()
		{
			for (int i = 0; i < flags.Length; i++)
				Add(flags[i]);
		}

		public ByteFlag(ByteFlag flags)
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

		ByteFlag(int flag1, int flag2, int flag3, int flag4, int flag5, int flag6, int flag7, int flag8)
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

		public bool this[byte index]
		{
			get { return Get(index); }
			set { Set(index, value); }
		}

		public ByteFlag Add(ByteFlag flags)
		{
			return this | flags;
		}

		public ByteFlag Add(byte flag)
		{
			var shift = flag % 32;

			if (flag < 32)
				f1 |= 1 << shift;
			else if (flag < 64)
				f2 |= 1 << shift;
			else if (flag < 96)
				f3 |= 1 << shift;
			else if (flag < 128)
				f4 |= 1 << shift;
			else if (flag < 160)
				f5 |= 1 << shift;
			else if (flag < 192)
				f6 |= 1 << shift;
			else if (flag < 224)
				f7 |= 1 << shift;
			else
				f8 |= 1 << shift;

			return this;
		}

		public ByteFlag Remove(ByteFlag flags)
		{
			return this & ~flags;
		}

		public ByteFlag Remove(byte flag)
		{
			var shift = flag % 32;

			if (flag < 32)
				f1 &= ~(1 << shift);
			else if (flag < 64)
				f2 &= ~(1 << shift);
			else if (flag < 96)
				f3 &= ~(1 << shift);
			else if (flag < 128)
				f4 &= ~(1 << shift);
			else if (flag < 160)
				f5 &= ~(1 << shift);
			else if (flag < 192)
				f6 &= ~(1 << shift);
			else if (flag < 224)
				f7 &= ~(1 << shift);
			else
				f8 &= ~(1 << shift);

			return this;
		}

		public byte[] ToArray()
		{
			var indices = new List<byte>();

			for (int i = 0; i <= byte.MaxValue; i++)
			{
				if (Get((byte)i))
					indices.Add((byte)i);
			}

			return indices.ToArray();
		}

		bool Get(byte index)
		{
			var shift = index % 32;

			if (index < 32)
				return (f1 & (1 << shift)) != 0;
			else if (index < 64)
				return (f2 & (1 << shift)) != 0;
			else if (index < 96)
				return (f3 & (1 << shift)) != 0;
			else if (index < 128)
				return (f4 & (1 << shift)) != 0;
			else if (index < 160)
				return (f5 & (1 << shift)) != 0;
			else if (index < 192)
				return (f6 & (1 << shift)) != 0;
			else if (index < 224)
				return (f7 & (1 << shift)) != 0;
			else
				return (f8 & (1 << shift)) != 0;
		}

		void Set(byte index, bool value)
		{
			if (value)
				Add(index);
			else
				Remove(index);
		}

		public override int GetHashCode()
		{
			return
				f1 ^
				f2 ^
				f3 ^
				f4 ^
				f5 ^
				f6 ^
				f7 ^
				f8;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ByteFlag))
				return false;

			return Equals((ByteFlag)obj);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}

		public bool Equals(ByteFlag other)
		{
			return this == other;
		}

		public static ByteFlag operator ~(ByteFlag a)
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

		public static ByteFlag operator |(ByteFlag a, ByteFlag b)
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

		public static ByteFlag operator &(ByteFlag a, ByteFlag b)
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

		public static ByteFlag operator ^(ByteFlag a, ByteFlag b)
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

		public static bool operator ==(ByteFlag a, ByteFlag b)
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

		public static bool operator !=(ByteFlag a, ByteFlag b)
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

	public struct ByteFlag<T> : IEquatable<ByteFlag<T>> where T : struct, IConvertible
	{
		ByteFlag flags;

		public static ByteFlag<T> Nothing
		{
			get { return new ByteFlag<T>(ByteFlag.Nothing); }
		}

		public static ByteFlag<T> Everything
		{
			get { return new ByteFlag<T>(ByteFlag.Everything); }
		}

		public ByteFlag(ByteFlag flags)
		{
			this.flags = flags;
		}

		public ByteFlag(T flag)
		{
			flags = new ByteFlag(flag.ToByte(null));
		}

		public ByteFlag(params T[] flags)
		{
			this.flags = new ByteFlag();

			for (int i = 0; i < flags.Length; i++)
				this.flags.Add(flags[i].ToByte(null));
		}

		public bool this[T index]
		{
			get { return flags[index.ToByte(null)]; }
			set { flags[index.ToByte(null)] = value; }
		}

		public ByteFlag<T> Add(T flag)
		{
			flags.Add(flag.ToByte(null));

			return this;
		}

		public ByteFlag<T> Add(ByteFlag<T> flags)
		{
			return this | flags;
		}

		public ByteFlag<T> Remove(T flag)
		{
			flags.Remove(flag.ToByte(null));

			return this;
		}

		public ByteFlag<T> Remove(ByteFlag<T> flag)
		{
			return this & ~flag;
		}

		public override int GetHashCode()
		{
			return flags.GetHashCode();
		}

		public bool Equals(ByteFlag<T> other)
		{
			return flags.Equals(other.flags);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ByteFlag<T>))
				return false;

			return Equals((ByteFlag<T>)obj);
		}

		public T[] ToArray()
		{
			return flags.ToArray().Convert(b => (T)Enum.ToObject(typeof(T), b));
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}

		public static ByteFlag<T> operator ~(ByteFlag<T> a)
		{
			return new ByteFlag<T>(~a.flags);
		}

		public static ByteFlag<T> operator |(ByteFlag<T> a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a.flags | b.flags);
		}

		public static ByteFlag<T> operator |(ByteFlag a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a | b.flags);
		}

		public static ByteFlag<T> operator |(ByteFlag<T> a, ByteFlag b)
		{
			return new ByteFlag<T>(a.flags | b);
		}

		public static ByteFlag<T> operator &(ByteFlag<T> a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a.flags & b.flags);
		}

		public static ByteFlag<T> operator &(ByteFlag a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a & b.flags);
		}

		public static ByteFlag<T> operator &(ByteFlag<T> a, ByteFlag b)
		{
			return new ByteFlag<T>(a.flags & b);
		}

		public static ByteFlag<T> operator ^(ByteFlag<T> a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a.flags ^ b.flags);
		}

		public static ByteFlag<T> operator ^(ByteFlag a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a ^ b.flags);
		}

		public static ByteFlag<T> operator ^(ByteFlag<T> a, ByteFlag b)
		{
			return new ByteFlag<T>(a.flags ^ b);
		}

		public static bool operator ==(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags == b.flags;
		}

		public static bool operator ==(ByteFlag a, ByteFlag<T> b)
		{
			return a == b.flags;
		}

		public static bool operator ==(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags == b;
		}

		public static bool operator !=(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags != b.flags;
		}

		public static bool operator !=(ByteFlag a, ByteFlag<T> b)
		{
			return a != b.flags;
		}

		public static bool operator !=(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags != b;
		}

		public static implicit operator ByteFlag(ByteFlag<T> a)
		{
			return a.flags;
		}

		public static implicit operator ByteFlag<T>(ByteFlag a)
		{
			return new ByteFlag<T>(a);
		}
	}
}