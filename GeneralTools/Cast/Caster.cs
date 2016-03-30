using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Cast;
using System.Runtime.InteropServices;

namespace Pseudo
{
	public abstract class Caster<TIn, TOut> : ICaster<TIn, TOut>
	{
		[StructLayout(LayoutKind.Explicit)]
		struct BitwiseCaster
		{
			[FieldOffset(0)]
			public readonly TIn Input;
			[FieldOffset(0)]
			public readonly TOut Output;

			public BitwiseCaster(TIn input)
			{
				Output = default(TOut);
				// Must be initialized after output, otherwise memory is overwritten.
				Input = input;
			}
		}

		public static readonly ICaster<TIn, TOut> Default = CreateCaster();

		public abstract TOut Cast(TIn value);

		public static TOut BitwiseCast(TIn value)
		{
			return new BitwiseCaster(value).Output;
		}

		static ICaster<TIn, TOut> CreateCaster()
		{
			var typeIn = typeof(TIn);
			var typeOut = typeof(TOut);
			var casterType = TypeUtility.FindType(t => t.Is<ICaster<TIn, TOut>>());

			if (casterType == null)
			{
				if (typeIn.IsEnum)
					return CreateEnumCaster(true);
				else if (typeOut.IsEnum)
					return CreateEnumCaster(false);
				else if (typeIn.Is<IConvertible>() && typeOut.Is<IConvertible>())
					return CreateConvertibleCaster();
				else if (typeIn.IsClass && typeOut.IsClass)
					return CreateCaster(typeof(ReferenceCaster<,>).MakeGenericType(typeIn, typeOut));
				else if (typeIn.IsClass != typeOut.IsClass)
					return new DefaultCaster<TIn, TOut>();
				else
					return new BitwiseCaster<TIn, TOut>();
			}
			else
				return CreateCaster(casterType);
		}

		static ICaster<TIn, TOut> CreateCaster(Type casterType)
		{
			return (ICaster<TIn, TOut>)Activator.CreateInstance(casterType);
		}

		static ICaster<TIn, TOut> CreateEnumCaster(bool input)
		{
			var type = Enum.GetUnderlyingType(typeof(TIn));

			if (type == typeof(byte))
				return CreateEnumCaster<byte>(input);
			else if (type == typeof(sbyte))
				return CreateEnumCaster<sbyte>(input);
			else if (type == typeof(ushort))
				return CreateEnumCaster<ushort>(input);
			else if (type == typeof(short))
				return CreateEnumCaster<short>(input);
			else if (type == typeof(uint))
				return CreateEnumCaster<uint>(input);
			else if (type == typeof(int))
				return CreateEnumCaster<int>(input);
			else if (type == typeof(ulong))
				return CreateEnumCaster<ulong>(input);
			else if (type == typeof(long))
				return CreateEnumCaster<long>(input);

			return null;
		}

		static ICaster<TIn, TOut> CreateEnumCaster<TUnder>(bool input)
			where TUnder : struct, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
		{
			if (input)
				return new EnumOutCaster<TIn, TUnder, TOut>();
			else
				return new EnumInCaster<TIn, TOut, TUnder>();
		}

		static ICaster<TIn, TOut> CreateConvertibleCaster()
		{
			var typeCode = Type.GetTypeCode(typeof(TIn));

			switch (typeCode)
			{
				default:
				case TypeCode.Empty:
				case TypeCode.Object:
				case TypeCode.DBNull:
					return CreateConvertibleCaster<IConvertible>();
				case TypeCode.Boolean:
					return CreateConvertibleCaster<bool>();
				case TypeCode.Char:
					return CreateConvertibleCaster<char>();
				case TypeCode.SByte:
					return CreateConvertibleCaster<sbyte>();
				case TypeCode.Byte:
					return CreateConvertibleCaster<byte>();
				case TypeCode.Int16:
					return CreateConvertibleCaster<short>();
				case TypeCode.UInt16:
					return CreateConvertibleCaster<ushort>();
				case TypeCode.Int32:
					return CreateConvertibleCaster<int>();
				case TypeCode.UInt32:
					return CreateConvertibleCaster<uint>();
				case TypeCode.Int64:
					return CreateConvertibleCaster<long>();
				case TypeCode.UInt64:
					return CreateConvertibleCaster<ulong>();
				case TypeCode.Single:
					return CreateConvertibleCaster<float>();
				case TypeCode.Double:
					return CreateConvertibleCaster<double>();
				case TypeCode.Decimal:
					return CreateConvertibleCaster<decimal>();
				case TypeCode.DateTime:
					return CreateConvertibleCaster<DateTime>();
				case TypeCode.String:
					return CreateConvertibleCaster<string>();
			}
		}

		static ICaster<TIn, TOut> CreateConvertibleCaster<TConvert>() where TConvert : IConvertible
		{
			return (ICaster<TIn, TOut>)new ConvertibleCaster<TConvert, TOut>();
		}
	}
}
