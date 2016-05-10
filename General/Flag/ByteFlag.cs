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
	public struct ByteFlag : IFlag<ByteFlag, byte>
	{
		public static readonly ByteFlag Nothing = new ByteFlag(0, 0, 0, 0, 0, 0, 0, 0);
		public static readonly ByteFlag Everything = new ByteFlag(-1, -1, -1, -1, -1, -1, -1, -1);

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

		public ByteFlag(byte flag) : this()
		{
			Set(flag);
		}

		public ByteFlag(byte flag1, byte flag2) : this(flag1)
		{
			Set(flag2);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3) : this(flag1, flag2)
		{
			Set(flag3);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3, byte flag4) : this(flag1, flag2, flag3)
		{
			Set(flag4);
		}

		public ByteFlag(byte flag1, byte flag2, byte flag3, byte flag4, byte flag5) : this(flag1, flag2, flag3, flag4)
		{
			Set(flag5);
		}

		public ByteFlag(params byte[] flags) : this()
		{
			for (int i = 0; i < flags.Length; i++)
				Set(flags[i]);
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

		public bool this[byte flag]
		{
			get { return Get(flag); }
		}

		public bool HasAll(ByteFlag flags)
		{
			return (this & flags) == flags;
		}

		public bool HasAny(ByteFlag flags)
		{
			return (this & flags) != Nothing;
		}

		public bool HasNone(ByteFlag flags)
		{
			return (this & flags) == Nothing;
		}

		public byte[] ToArray()
		{
			var indices = new List<byte>();

			for (int i = 0; i <= byte.MaxValue; i++)
			{
				if (this[(byte)i])
					indices.Add((byte)i);
			}

			return indices.ToArray();
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

		bool Get(byte flag)
		{
			var shift = 1 << (flag % 32);

			if (flag < 32)
				return (f1 & shift) != 0;
			else if (flag < 64)
				return (f2 & shift) != 0;
			else if (flag < 96)
				return (f3 & shift) != 0;
			else if (flag < 128)
				return (f4 & shift) != 0;
			else if (flag < 160)
				return (f5 & shift) != 0;
			else if (flag < 192)
				return (f6 & shift) != 0;
			else if (flag < 224)
				return (f7 & shift) != 0;
			else
				return (f8 & shift) != 0;
		}

		void Set(byte flag)
		{
			var shift = 1 << (flag % 32);

			if (flag < 32)
				f1 |= shift;
			else if (flag < 64)
				f2 |= shift;
			else if (flag < 96)
				f3 |= shift;
			else if (flag < 128)
				f4 |= shift;
			else if (flag < 160)
				f5 |= shift;
			else if (flag < 192)
				f6 |= shift;
			else if (flag < 224)
				f7 |= shift;
			else
				f8 |= shift;
		}

		void Unset(byte flag)
		{
			var shift = 1 << (flag % 32);

			if (flag < 32)
				f1 &= ~shift;
			else if (flag < 64)
				f2 &= ~shift;
			else if (flag < 96)
				f3 &= ~shift;
			else if (flag < 128)
				f4 &= ~shift;
			else if (flag < 160)
				f5 &= ~shift;
			else if (flag < 192)
				f6 &= ~shift;
			else if (flag < 224)
				f7 &= ~shift;
			else
				f8 &= ~shift;
		}

		ByteFlag IFlag<ByteFlag, byte>.Add(byte flag)
		{
			return this + flag;
		}

		ByteFlag IFlag<ByteFlag, byte>.Subtract(byte flag)
		{
			return this - flag;
		}

		bool IFlag<ByteFlag, byte>.Has(byte flag)
		{
			return this[flag];
		}

		ByteFlag IFlag<ByteFlag>.Add(ByteFlag flags)
		{
			return this + flags;
		}

		ByteFlag IFlag<ByteFlag>.Subtract(ByteFlag flags)
		{
			return this - flags;
		}

		ByteFlag IFlag<ByteFlag>.Not()
		{
			return ~this;
		}

		ByteFlag IFlag<ByteFlag>.And(ByteFlag other)
		{
			return this & other;
		}

		ByteFlag IFlag<ByteFlag>.Or(ByteFlag other)
		{
			return this | other;
		}

		ByteFlag IFlag<ByteFlag>.Xor(ByteFlag other)
		{
			return this ^ other;
		}

		bool IEquatable<ByteFlag>.Equals(ByteFlag other)
		{
			return this == other;
		}

		public static ByteFlag operator +(ByteFlag a, byte b)
		{
			a.Set(b);

			return a;
		}

		public static ByteFlag operator +(byte a, ByteFlag b)
		{
			b.Set(a);

			return b;
		}

		public static ByteFlag operator +(ByteFlag a, ByteFlag b)
		{
			return a | b;
		}

		public static ByteFlag operator -(ByteFlag a, byte b)
		{
			a.Unset(b);

			return a;
		}

		public static ByteFlag operator -(byte a, ByteFlag b)
		{
			b.Unset(a);

			return b;
		}

		public static ByteFlag operator -(ByteFlag a, ByteFlag b)
		{
			return a & ~b;
		}

		public static ByteFlag operator ~(ByteFlag a)
		{
			return new ByteFlag(
				~a.f1,
				~a.f2,
				~a.f3,
				~a.f4,
				~a.f5,
				~a.f6,
				~a.f7,
				~a.f8);
		}

		public static ByteFlag operator &(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(
				a.f1 & b.f1,
				a.f2 & b.f2,
				a.f3 & b.f3,
				a.f4 & b.f4,
				a.f5 & b.f5,
				a.f6 & b.f6,
				a.f7 & b.f7,
				a.f8 & b.f8);
		}

		public static ByteFlag operator |(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(
				a.f1 | b.f1,
				a.f2 | b.f2,
				a.f3 | b.f3,
				a.f4 | b.f4,
				a.f5 | b.f5,
				a.f6 | b.f6,
				a.f7 | b.f7,
				a.f8 | b.f8);
		}

		public static ByteFlag operator ^(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(
				a.f1 ^ b.f1,
				a.f2 ^ b.f2,
				a.f3 ^ b.f3,
				a.f4 ^ b.f4,
				a.f5 ^ b.f5,
				a.f6 ^ b.f6,
				a.f7 ^ b.f7,
				a.f8 ^ b.f8);
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

	public struct ByteFlag<T> : IFlag<ByteFlag, T>, IFlag<ByteFlag<T>, T> where T : struct, IConvertible
	{
		public static readonly ByteFlag<T> Nothing = new ByteFlag<T>(ByteFlag.Nothing);
		public static readonly ByteFlag<T> Everything = new ByteFlag<T>(ByteFlag.Everything);

		static readonly ICaster<T, byte> byteCaster = Caster<T, byte>.Default;
		static readonly ICaster<byte, T> genericCaster = Caster<byte, T>.Default;

		readonly ByteFlag flags;

		public ByteFlag(T flag)
		{
			flags = new ByteFlag(byteCaster.Cast(flag));
		}

		public ByteFlag(T flag1, T flag2) : this(flag1)
		{
			flags = new ByteFlag(byteCaster.Cast(flag1), byteCaster.Cast(flag2));
		}

		public ByteFlag(T flag1, T flag2, T flag3) : this(flag1, flag2)
		{
			flags = new ByteFlag(byteCaster.Cast(flag1), byteCaster.Cast(flag2), byteCaster.Cast(flag3));
		}

		public ByteFlag(T flag1, T flag2, T flag3, T flag4) : this(flag1, flag2, flag3)
		{
			flags = new ByteFlag(byteCaster.Cast(flag1), byteCaster.Cast(flag2), byteCaster.Cast(flag3), byteCaster.Cast(flag4));
		}

		public ByteFlag(T flag1, T flag2, T flag3, T flag4, T flag5) : this(flag1, flag2, flag3, flag4)
		{
			flags = new ByteFlag(byteCaster.Cast(flag1), byteCaster.Cast(flag2), byteCaster.Cast(flag3), byteCaster.Cast(flag4), byteCaster.Cast(flag5));
		}

		public ByteFlag(params T[] flags)
		{
			this.flags = new ByteFlag(flags.Convert(f => byteCaster.Cast(f)));
		}

		public ByteFlag(ByteFlag flags)
		{
			this.flags = flags;
		}

		public ByteFlag(ByteFlag<T> flags)
		{
			this.flags = flags.flags;
		}

		public bool this[T flag]
		{
			get { return flags[byteCaster.Cast(flag)]; }
		}

		public bool HasAll(ByteFlag<T> flags)
		{
			return this.flags.HasAll(flags.flags);
		}

		public bool HasAny(ByteFlag<T> flags)
		{
			return this.flags.HasAny(flags.flags);
		}

		public bool HasNone(ByteFlag<T> flags)
		{
			return this.flags.HasNone(flags.flags);
		}

		public override int GetHashCode()
		{
			return flags.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is ByteFlag)
				return this == (ByteFlag)obj;
			else if (obj is ByteFlag<T>)
				return this == (ByteFlag<T>)obj;
			else
				return false;
		}

		public T[] ToArray()
		{
			return flags
				.ToArray()
				.Convert(b => genericCaster.Cast(b));
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(ToArray()));
		}

		ByteFlag IFlag<ByteFlag, T>.Add(T flag)
		{
			return flags + byteCaster.Cast(flag);
		}

		ByteFlag IFlag<ByteFlag, T>.Subtract(T flag)
		{
			return flags - byteCaster.Cast(flag);
		}

		bool IFlag<ByteFlag, T>.Has(T flag)
		{
			return this[flag];
		}

		ByteFlag IFlag<ByteFlag>.Add(ByteFlag flags)
		{
			return this + flags;
		}

		ByteFlag IFlag<ByteFlag>.Subtract(ByteFlag flags)
		{
			return this - flags;
		}

		bool IFlag<ByteFlag>.HasAll(ByteFlag flags)
		{
			return flags.HasAll(flags);
		}

		bool IFlag<ByteFlag>.HasAny(ByteFlag flags)
		{
			return flags.HasAny(flags);
		}

		bool IFlag<ByteFlag>.HasNone(ByteFlag flags)
		{
			return flags.HasNone(flags);
		}

		ByteFlag IFlag<ByteFlag>.Not()
		{
			return ~flags;
		}

		ByteFlag IFlag<ByteFlag>.And(ByteFlag other)
		{
			return flags & other;
		}

		ByteFlag IFlag<ByteFlag>.Or(ByteFlag other)
		{
			return flags | other;
		}

		ByteFlag IFlag<ByteFlag>.Xor(ByteFlag other)
		{
			return flags ^ other;
		}

		bool IEquatable<ByteFlag>.Equals(ByteFlag other)
		{
			return this == other;
		}

		ByteFlag<T> IFlag<ByteFlag<T>, T>.Add(T flag)
		{
			return flags + byteCaster.Cast(flag);
		}

		ByteFlag<T> IFlag<ByteFlag<T>, T>.Subtract(T flag)
		{
			return flags - byteCaster.Cast(flag);
		}

		bool IFlag<ByteFlag<T>, T>.Has(T flag)
		{
			return this[flag];
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.Add(ByteFlag<T> flags)
		{
			return this + flags;
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.Subtract(ByteFlag<T> flags)
		{
			return this - flags;
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.Not()
		{
			return ~flags;
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.And(ByteFlag<T> other)
		{
			return flags & other;
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.Or(ByteFlag<T> other)
		{
			return flags | other;
		}

		ByteFlag<T> IFlag<ByteFlag<T>>.Xor(ByteFlag<T> other)
		{
			return flags ^ other;
		}

		bool IEquatable<ByteFlag<T>>.Equals(ByteFlag<T> other)
		{
			return this == other;
		}

		public static ByteFlag<T> operator +(ByteFlag<T> a, T b)
		{
			return a.flags + byteCaster.Cast(b);
		}

		public static ByteFlag<T> operator +(T a, ByteFlag<T> b)
		{
			return byteCaster.Cast(a) + b.flags;
		}

		public static ByteFlag<T> operator +(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags + b.flags;
		}

		public static ByteFlag<T> operator +(ByteFlag a, ByteFlag<T> b)
		{
			return a + b.flags;
		}

		public static ByteFlag<T> operator +(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags + b;
		}

		public static ByteFlag<T> operator -(ByteFlag<T> a, T b)
		{
			return a.flags - byteCaster.Cast(b);
		}

		public static ByteFlag<T> operator -(T a, ByteFlag<T> b)
		{
			return byteCaster.Cast(a) - b.flags;
		}

		public static ByteFlag<T> operator -(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags - b.flags;
		}

		public static ByteFlag<T> operator -(ByteFlag a, ByteFlag<T> b)
		{
			return a - b.flags;
		}

		public static ByteFlag<T> operator -(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags - b;
		}

		public static ByteFlag<T> operator ~(ByteFlag<T> a)
		{
			return ~a.flags;
		}

		public static ByteFlag<T> operator |(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags | b.flags;
		}

		public static ByteFlag<T> operator |(ByteFlag a, ByteFlag<T> b)
		{
			return a | b.flags;
		}

		public static ByteFlag<T> operator |(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags | b;
		}

		public static ByteFlag<T> operator &(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags & b.flags;
		}

		public static ByteFlag<T> operator &(ByteFlag a, ByteFlag<T> b)
		{
			return a & b.flags;
		}

		public static ByteFlag<T> operator &(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags & b;
		}

		public static ByteFlag<T> operator ^(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags ^ b.flags;
		}

		public static ByteFlag<T> operator ^(ByteFlag a, ByteFlag<T> b)
		{
			return a ^ b.flags;
		}

		public static ByteFlag<T> operator ^(ByteFlag<T> a, ByteFlag b)
		{
			return a.flags ^ b;
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