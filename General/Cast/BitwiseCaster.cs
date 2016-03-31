using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class BitwiseCaster<TIn, TOut> : Caster<TIn, TOut>
	{
		public override TOut Cast(TIn value)
		{
			return BitwiseCast(value);
		}
	}
}
