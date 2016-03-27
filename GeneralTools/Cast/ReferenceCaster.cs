using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Cast
{
	public class ReferenceCaster<TIn, TOut> : Caster<TIn, TOut>
		where TIn : class
		where TOut : class
	{
		public override TOut Cast(TIn value)
		{
			return value as TOut;
		}
	}
}
