using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class DefaultCaster<TIn, TOut> : Caster<TIn, TOut>
	{
		public override TOut Cast(TIn value)
		{
			if (value is TOut)
				return (TOut)(object)value;
			else
				return default(TOut);
		}
	}
}
