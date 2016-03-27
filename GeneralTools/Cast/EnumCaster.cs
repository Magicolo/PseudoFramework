using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Cast
{
	public class EnumInCaster<TIn, TEnum, TUnder> : Caster<TIn, TEnum>
	{
		static readonly ICaster<TIn, TUnder> caster = Caster<TIn, TUnder>.Default;

		public override TEnum Cast(TIn value)
		{
			return Caster<TUnder, TEnum>.BitwiseCast(caster.Cast(value));
		}
	}

	public class EnumOutCaster<TEnum, TUnder, TOut> : Caster<TEnum, TOut>
	{
		static readonly ICaster<TUnder, TOut> caster = Caster<TUnder, TOut>.Default;

		public override TOut Cast(TEnum value)
		{
			return caster.Cast(Caster<TEnum, TUnder>.BitwiseCast(value));
		}
	}
}
