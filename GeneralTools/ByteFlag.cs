using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct ByteFlag : IEquatable<ByteFlag>
	{
		[SerializeField]
		ulong flag1;
		[SerializeField]
		ulong flag2;
		[SerializeField]
		ulong flag3;
		[SerializeField]
		ulong flag4;

		public static ByteFlag Nothing
		{
			get { return new ByteFlag(0uL, 0uL, 0uL, 0uL); }
		}

		public static ByteFlag Everything
		{
			get { return new ByteFlag(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue); }
		}

		public ByteFlag(ByteFlag flag)
		{
			this.flag1 = flag.flag1;
			this.flag2 = flag.flag2;
			this.flag3 = flag.flag3;
			this.flag4 = flag.flag4;
		}

		public ByteFlag(byte[] indices) : this()
		{
			for (int i = 0; i < indices.Length; i++)
				AddBit(indices[i]);
		}

		public ByteFlag(bool[] values) : this()
		{
			int count = Math.Min(values.Length, 255);

			for (byte i = 0; i < count; i++)
				Set(i, values[i]);
		}

		ByteFlag(ulong flag1, ulong flag2, ulong flag3, ulong flag4)
		{
			this.flag1 = flag1;
			this.flag2 = flag2;
			this.flag3 = flag3;
			this.flag4 = flag4;
		}

		public bool this[byte index]
		{
			get { return Get(index); }
			set { Set(index, value); }
		}

		public byte[] ToIndices()
		{
			List<byte> indices = new List<byte>();

			for (byte i = 0; i < 255; i++)
			{
				if (Get(i))
					indices.Add(i);
			}

			return indices.ToArray();
		}

		public bool[] ToValues()
		{
			bool[] values = new bool[255];

			for (byte i = 0; i < 255; i++)
				values[i] = Get(i);

			return values;
		}

		bool Get(byte index)
		{
			if (index < 64)
				return (flag1 & (1uL << index)) != 0;
			else if (index < 128)
				return (flag2 & (1uL << (index - 64))) != 0;
			else if (index < 192)
				return (flag3 & (1uL << (index - 128))) != 0;
			else
				return (flag4 & (1uL << (index - 192))) != 0;
		}

		void Set(byte index, bool value)
		{
			if (value)
				AddBit(index);
			else
				RemoveBit(index);
		}

		void AddBit(byte index)
		{
			if (index < 64)
				flag1 |= 1uL << index;
			else if (index < 128)
				flag2 |= 1uL << (index - 64);
			else if (index < 192)
				flag3 |= 1uL << (index - 128);
			else
				flag4 |= 1uL << (index - 192);
		}

		void RemoveBit(byte value)
		{
			if (value < 64)
				flag1 &= ~(1uL << value);
			else if (value < 128)
				flag2 &= ~(1uL << (value - 64));
			else if (value < 192)
				flag3 &= ~(1uL << (value - 128));
			else
				flag4 &= ~(1uL << (value - 192));
		}

		public override int GetHashCode()
		{
			return flag1.GetHashCode() ^ flag2.GetHashCode() ^ flag3.GetHashCode() ^ flag4.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ByteFlag))
				return false;

			return Equals(flag1, (ByteFlag)obj);
		}

		public override string ToString()
		{
			var log = new System.Text.StringBuilder();
			log.Append(GetType().Name + "(");
			bool first = true;

			for (byte i = 0; i < 255; i++)
			{
				if (Get(i))
				{
					if (first)
						first = false;
					else
						log.Append(", ");

					log.Append(i);
				}
			}

			log.Append(")");

			return log.ToString();
		}

		public bool Equals(ByteFlag other)
		{
			return flag1.Equals(other.flag1) && flag2.Equals(other.flag2) && flag3.Equals(other.flag3) && flag4.Equals(other.flag4);
		}

		public static ByteFlag operator ~(ByteFlag a)
		{
			return new ByteFlag(~a.flag1, ~a.flag2, ~a.flag3, ~a.flag4);
		}

		public static ByteFlag operator |(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(a.flag1 | b.flag1, a.flag2 | b.flag2, a.flag3 | b.flag3, a.flag4 | b.flag4);
		}

		public static ByteFlag operator &(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(a.flag1 & b.flag1, a.flag2 & b.flag2, a.flag3 & b.flag3, a.flag4 & b.flag4);
		}

		public static ByteFlag operator ^(ByteFlag a, ByteFlag b)
		{
			return new ByteFlag(a.flag1 ^ b.flag1, a.flag2 ^ b.flag2, a.flag3 ^ b.flag3, a.flag4 ^ b.flag4);
		}

		public static bool operator ==(ByteFlag a, ByteFlag b)
		{
			return a.flag1 == b.flag1 && a.flag2 == b.flag2 && a.flag3 == b.flag3 && a.flag4 == b.flag4;
		}

		public static bool operator !=(ByteFlag a, ByteFlag b)
		{
			return a.flag1 != b.flag1 || a.flag2 != b.flag2 || a.flag3 != b.flag3 || a.flag4 != b.flag4;
		}
	}

	public struct ByteFlag<T> : IEquatable<ByteFlag<T>> where T : struct
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

		public bool this[T index]
		{
			get { return flags[Convert.ToByte(index)]; }
			set { flags[Convert.ToByte(value)] = value; }
		}

		public void Add(T flag)
		{
			flags[Convert.ToByte(flag)] = true;
		}

		public void Remove(T flag)
		{
			flags[Convert.ToByte(flag)] = false;
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

		public override string ToString()
		{
			var values = flags.ToIndices().Convert(b => (T)(object)b);
			var log = new System.Text.StringBuilder();

			log.Append(GetType().Name + "(");
			bool first = true;

			for (byte i = 0; i < values.Length; i++)
			{
				if (first)
					first = false;
				else
					log.Append(", ");

				log.Append(values[i]);
			}

			log.Append(")");

			return log.ToString();
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