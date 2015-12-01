using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct ByteFlag
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

		public bool Get(byte index)
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

		public void Set(byte index, bool value)
		{
			if (value)
				AddBit(index);
			else
				RemoveBit(index);
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
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ByteFlag))
				return false;

			ByteFlag flag = (ByteFlag)obj;

			return flag1.Equals(flag.flag1) && flag2.Equals(flag.flag2) && flag3.Equals(flag.flag3) && flag4.Equals(flag.flag4);
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

	public struct ByteFlag<T> where T : struct
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

		public ByteFlag<T> Add(T value)
		{
			flags.Set(Convert.ToByte(value), true);
			return this;
		}

		public ByteFlag<T> Remove(T value)
		{
			flags.Set(Convert.ToByte(value), false);
			return this;
		}

		public bool Has(T value)
		{
			return flags.Get(Convert.ToByte(value));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ByteFlag<T>))
				return false;

			ByteFlag<T> bigFlag = (ByteFlag<T>)obj;

			return flags.Equals(bigFlag.flags);
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

		public static ByteFlag<T> operator &(ByteFlag<T> a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a.flags & b.flags);
		}

		public static ByteFlag<T> operator ^(ByteFlag<T> a, ByteFlag<T> b)
		{
			return new ByteFlag<T>(a.flags ^ b.flags);
		}

		public static bool operator ==(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags == b.flags;
		}

		public static bool operator !=(ByteFlag<T> a, ByteFlag<T> b)
		{
			return a.flags != b.flags;
		}
	}
}