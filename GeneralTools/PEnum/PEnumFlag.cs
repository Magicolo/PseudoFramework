using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pseudo
{
	public abstract class PEnumFlag<TEnum> : PEnum<TEnum, ByteFlag>, IEnumFlag
		where TEnum : PEnumFlag<TEnum>
	{
		public static TEnum Nothing
		{
			get
			{
				if (nothing == null)
					nothing = CreateValue(ByteFlag.Nothing, "Nothing");

				return nothing;
			}
		}
		public static TEnum Everything
		{
			get
			{
				if (everything == null)
					everything = CreateValue(GetEverything(), "Everything");

				return everything;
			}
		}

		static TEnum nothing;
		static TEnum everything;

		protected PEnumFlag(params byte[] values) : base(new ByteFlag(values)) { }

		public TEnum Add(TEnum flags)
		{
			return (TEnum)(this | flags);
		}

		public TEnum Add(ByteFlag flags)
		{
			return (TEnum)(this | flags);
		}

		public TEnum Add(byte flag)
		{
			return GetValue(Value.Add(flag));
		}

		public TEnum Remove(TEnum flags)
		{
			return Remove(flags.Value);
		}

		public TEnum Remove(ByteFlag flags)
		{
			return GetValue(Value & ~flags);
		}

		public TEnum Remove(byte flag)
		{
			return GetValue(Value.Remove(flag));
		}

		public bool Has(byte flag)
		{
			return Value[flag];
		}

		public bool HasAll(TEnum flags)
		{
			return (~Value & flags.Value) == ByteFlag.Nothing;
		}

		public bool HasAny(TEnum flags)
		{
			return (Value & ~flags.Value) != Value;
		}

		public bool HasNone(TEnum flags)
		{
			return !HasAny(flags);
		}

		public bool HasAll(ByteFlag flags)
		{
			return (~Value & flags) == ByteFlag.Nothing;
		}

		public bool HasAny(ByteFlag flags)
		{
			return (Value & ~flags) != Value;
		}

		public bool HasNone(ByteFlag flags)
		{
			return !HasAny(flags);
		}

		IEnumFlag IEnumFlag.Add(IEnumFlag flags)
		{
			return Add((TEnum)flags);
		}

		IEnumFlag IEnumFlag.Add(ByteFlag flags)
		{
			return Add(flags);
		}

		IEnumFlag IEnumFlag.Add(byte flag)
		{
			return Add(flag);
		}

		IEnumFlag IEnumFlag.Remove(IEnumFlag flags)
		{
			return Remove((TEnum)flags);
		}

		IEnumFlag IEnumFlag.Remove(ByteFlag flags)
		{
			return Remove(flags);
		}

		IEnumFlag IEnumFlag.Remove(byte flag)
		{
			return Remove(flag);
		}

		bool IEnumFlag.HasAll(IEnumFlag flags)
		{
			return HasAll((TEnum)flags);
		}

		bool IEnumFlag.HasAll(ByteFlag flags)
		{
			return HasAll(flags);
		}

		bool IEnumFlag.HasAny(IEnumFlag flags)
		{
			return HasAny((TEnum)flags);
		}

		bool IEnumFlag.HasAny(ByteFlag flags)
		{
			return HasAny(flags);
		}

		bool IEnumFlag.HasNone(IEnumFlag flags)
		{
			return HasAll((TEnum)flags);
		}

		bool IEnumFlag.HasNone(ByteFlag flags)
		{
			return HasAll(flags);
		}

		IEnumFlag IEnumFlag.And(IEnumFlag flags)
		{
			return this & flags.Value;
		}

		IEnumFlag IEnumFlag.And(ByteFlag flags)
		{
			return this & flags;
		}

		IEnumFlag IEnumFlag.Or(IEnumFlag flags)
		{
			return this | flags.Value;
		}

		IEnumFlag IEnumFlag.Or(ByteFlag flags)
		{
			return this | flags;
		}

		IEnumFlag IEnumFlag.Xor(IEnumFlag flags)
		{
			return this ^ flags.Value;
		}

		IEnumFlag IEnumFlag.Xor(ByteFlag flags)
		{
			return this ^ flags;
		}

		IEnumFlag IEnumFlag.Not()
		{
			return ~this;
		}

		static ByteFlag GetEverything()
		{
			Initialize();
			var values = GetValues();
			var everything = ByteFlag.Nothing;

			for (int i = 0; i < values.Length; i++)
				everything |= values[i];

			return everything;
		}

		public static PEnumFlag<TEnum> operator ~(PEnumFlag<TEnum> a)
		{
			return GetValue(~a.Value);
		}

		public static PEnumFlag<TEnum> operator |(PEnumFlag<TEnum> a, PEnumFlag<TEnum> b)
		{
			return GetValue(a.Value | b.Value);
		}

		public static PEnumFlag<TEnum> operator |(PEnumFlag<TEnum> a, ByteFlag b)
		{
			return GetValue(a.Value | b);
		}

		public static PEnumFlag<TEnum> operator &(PEnumFlag<TEnum> a, PEnumFlag<TEnum> b)
		{
			return GetValue(a.Value & b.Value);
		}

		public static PEnumFlag<TEnum> operator &(PEnumFlag<TEnum> a, ByteFlag b)
		{
			return GetValue(a.Value & b);
		}

		public static PEnumFlag<TEnum> operator ^(PEnumFlag<TEnum> a, PEnumFlag<TEnum> b)
		{
			return GetValue(a.Value ^ b.Value);
		}

		public static PEnumFlag<TEnum> operator ^(PEnumFlag<TEnum> a, ByteFlag b)
		{
			return GetValue(a.Value ^ b);
		}
	}
}
