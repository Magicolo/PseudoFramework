using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class ConvertibleCaster<TIn, TOut> : Caster<TIn, TOut>
		where TIn : IConvertible
	{
		static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TOut));

		public override TOut Cast(TIn value)
		{
			switch (typeCode)
			{
				default:
				case TypeCode.Empty:
				case TypeCode.Object:
				case TypeCode.DBNull:
					return default(TOut);
				case TypeCode.Boolean:
					return Caster<bool, TOut>.BitwiseCast(value.ToBoolean(null));
				case TypeCode.Char:
					return Caster<char, TOut>.BitwiseCast(value.ToChar(null));
				case TypeCode.SByte:
					return Caster<sbyte, TOut>.BitwiseCast(value.ToSByte(null));
				case TypeCode.Byte:
					return Caster<byte, TOut>.BitwiseCast(value.ToByte(null));
				case TypeCode.Int16:
					return Caster<short, TOut>.BitwiseCast(value.ToInt16(null));
				case TypeCode.UInt16:
					return Caster<ushort, TOut>.BitwiseCast(value.ToUInt16(null));
				case TypeCode.Int32:
					return Caster<int, TOut>.BitwiseCast(value.ToInt32(null));
				case TypeCode.UInt32:
					return Caster<uint, TOut>.BitwiseCast(value.ToUInt32(null));
				case TypeCode.Int64:
					return Caster<long, TOut>.BitwiseCast(value.ToInt64(null));
				case TypeCode.UInt64:
					return Caster<ulong, TOut>.BitwiseCast(value.ToUInt64(null));
				case TypeCode.Single:
					return Caster<float, TOut>.BitwiseCast(value.ToSingle(null));
				case TypeCode.Double:
					return Caster<double, TOut>.BitwiseCast(value.ToDouble(null));
				case TypeCode.Decimal:
					return Caster<decimal, TOut>.BitwiseCast(value.ToDecimal(null));
				case TypeCode.DateTime:
					return Caster<DateTime, TOut>.BitwiseCast(value.ToDateTime(null));
				case TypeCode.String:
					return Caster<string, TOut>.BitwiseCast(value.ToString(null));
			}
		}
	}
}
