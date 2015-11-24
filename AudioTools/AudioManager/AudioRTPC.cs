using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	[Serializable, Copy]
	public class AudioRTPC : IPoolable, ICopyable<AudioRTPC>
	{
		public enum RTPCTypes
		{
			Volume,
			Pitch
		}

		public enum RTPCScope
		{
			Local,
			Global
		}

		static readonly Dictionary<string, AudioValue<float>> rtpcValues = new Dictionary<string, AudioValue<float>>();
		public static readonly Pool Pool = new Pool(new AudioRTPC(), 16);

		AudioValue<float> value;
		float lastValue;
		float lastRatio;

		public string Name;
		public RTPCTypes Type;
		public RTPCScope Scope;
		public float MinValue;
		public float MaxValue = 1f;
		[Clamp]
		public AnimationCurve Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public AudioValue<float> Value { get { return value; } }

		public float GetAdjustedValue()
		{
			float ratio = GetRatio();
			float value;

			if (ratio == lastRatio)
				value = lastValue;
			else
				value = Curve.Evaluate(ratio);

			lastRatio = ratio;
			lastValue = value;

			return value;
		}

		public void SetValue(float value)
		{
			this.value.Value = value;
		}

		float GetRatio()
		{
			return Mathf.Clamp01((value.Value - MinValue) / (MaxValue - MinValue));
		}

		public virtual void OnCreate()
		{
			if (Scope == RTPCScope.Local)
				value = AudioValue<float>.Pool.Create();
			else
				value = GetGlobalRTPCValue(Name);

			lastValue = Curve.Evaluate(GetRatio());
		}

		public virtual void OnRecycle()
		{
			if (Scope == RTPCScope.Local)
				AudioValue<float>.Pool.Recycle(ref value);
		}

		public void Copy(AudioRTPC reference)
		{
			value = reference.value;
			lastValue = reference.lastValue;
			lastRatio = reference.lastRatio;
			Name = reference.Name;
			Type = reference.Type;
			Scope = reference.Scope;
			MinValue = reference.MinValue;
			MaxValue = reference.MaxValue;
			Curve = reference.Curve;
		}

		public static AudioValue<float> GetGlobalRTPCValue(string name)
		{
			AudioValue<float> value;

			if (!rtpcValues.TryGetValue(name, out value))
			{
				value = AudioValue<float>.Pool.Create();
				rtpcValues[name] = value;
			}

			return value;
		}

		public static void SetGlobalRTPCValue(string name, float value)
		{
			GetGlobalRTPCValue(name).Value = value;
		}
	}
}