using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class EnumInCaster<TIn, TEnum, TUnder> : Caster<TIn, TEnum>
		where TUnder : struct, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
	{
		static readonly ICaster<TIn, TUnder> caster = Caster<TIn, TUnder>.Default;

		public override TEnum Cast(TIn value)
		{
			return Caster<TUnder, TEnum>.BitwiseCast(caster.Cast(value));
		}
	}

	public class EnumOutCaster<TEnum, TUnder, TOut> : Caster<TEnum, TOut>
		where TUnder : struct, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
	{
		static readonly ICaster<TUnder, TOut> caster = Caster<TUnder, TOut>.Default;

		public override TOut Cast(TEnum value)
		{
			return caster.Cast(Caster<TEnum, TUnder>.BitwiseCast(value));
		}
	}
}
