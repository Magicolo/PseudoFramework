using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo.Oscillation.Internal
{
	public class ColorOscillator<TTarget> : OscillatorBase<TTarget, Color>
	{
		public ColorOscillator(PropertyInfo property) : base(property) { }

		public override void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time)
		{
			var value = Getter(target);
			var channels = (Channels)flags;

			if ((channels & Channels.R) != 0)
				value.r = OscillationUtility.Oscillate(settings[0], time);
			if ((channels & Channels.G) != 0)
				value.g = OscillationUtility.Oscillate(settings[1], time);
			if ((channels & Channels.B) != 0)
				value.b = OscillationUtility.Oscillate(settings[2], time);
			if ((channels & Channels.A) != 0)
				value.a = OscillationUtility.Oscillate(settings[3], time);

			Setter(target, value);
		}
	}
}
