using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Oscillation.Internal
{
	public abstract class OscillatorBase<TTarget, TValue> : IOscillator
	{
		public readonly PropertyInfo Property;
		public readonly Func<TTarget, TValue> Getter;
		public readonly Action<TTarget, TValue> Setter;

		protected OscillatorBase(PropertyInfo property)
		{
			Property = property;
			Getter = (Func<TTarget, TValue>)Delegate.CreateDelegate(typeof(Func<TTarget, TValue>), property.GetGetMethod(true));
			Setter = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), property.GetSetMethod(true));
		}

		public abstract void Oscillate(TTarget target, OscillationSettings[] settings, int flags, float time);

		void IOscillator.Oscillate(object target, OscillationSettings[] settings, int flags, float time)
		{
			Oscillate((TTarget)target, settings, flags, time);
		}
	}

}
