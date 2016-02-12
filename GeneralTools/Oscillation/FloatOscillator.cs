using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Oscillation
{
	public class FloatOscillator<TTarget> : OscillatorBase<TTarget, float>
	{
		public FloatOscillator(PropertyInfo property) : base(property) { }

		public override void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time)
		{
			Setter(target, OscillationUtility.Oscillate(settings[0], time));
		}
	}
}
