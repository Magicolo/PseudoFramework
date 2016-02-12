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
	public class Vector2Oscillator<TTarget> : OscillatorBase<TTarget, Vector2>
	{
		public Vector2Oscillator(PropertyInfo property) : base(property) { }

		public override void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time)
		{
			var value = Getter(target);
			var axes = (Axes)flags;

			if ((axes & Axes.X) != 0)
				value.x = OscillationUtility.Oscillate(settings[0], time);
			if ((axes & Axes.Y) != 0)
				value.y = OscillationUtility.Oscillate(settings[1], time);

			Setter(target, value);
		}
	}

	public class Vector3Oscillator<TTarget> : OscillatorBase<TTarget, Vector3>
	{
		public Vector3Oscillator(PropertyInfo property) : base(property) { }

		public override void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time)
		{
			var value = Getter(target);
			var axes = (Axes)flags;

			if ((axes & Axes.X) != 0)
				value.x = OscillationUtility.Oscillate(settings[0], time);
			if ((axes & Axes.Y) != 0)
				value.y = OscillationUtility.Oscillate(settings[1], time);
			if ((axes & Axes.Z) != 0)
				value.z = OscillationUtility.Oscillate(settings[2], time);

			Setter(target, value);
		}
	}

	public class Vector4Oscillator<TTarget> : OscillatorBase<TTarget, Vector4>
	{
		public Vector4Oscillator(PropertyInfo property) : base(property) { }

		public override void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time)
		{
			var value = Getter(target);
			var axes = (Axes)flags;

			if ((axes & Axes.X) != 0)
				value.x = OscillationUtility.Oscillate(settings[0], time);
			if ((axes & Axes.Y) != 0)
				value.y = OscillationUtility.Oscillate(settings[1], time);
			if ((axes & Axes.Z) != 0)
				value.z = OscillationUtility.Oscillate(settings[2], time);
			if ((axes & Axes.W) != 0)
				value.w = OscillationUtility.Oscillate(settings[3], time);

			Setter(target, value);
		}
	}
}
