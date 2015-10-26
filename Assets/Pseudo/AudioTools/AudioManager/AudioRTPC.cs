using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	[Serializable]
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

		AudioValue<float> _value;
		float _lastValue;
		float _lastRatio;

		public string Name;
		public RTPCTypes Type;
		public RTPCScope Scope;
		public float MinValue;
		public float MaxValue = 1f;
		[Clamp]
		public AnimationCurve Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public AudioValue<float> Value { get { return _value; } }

		static Dictionary<string, AudioValue<float>> _rtpcValues = new Dictionary<string, AudioValue<float>>();

		public float GetAdjustedValue()
		{
			float ratio = GetRatio();
			float value;

			if (ratio == _lastRatio)
				value = _lastValue;
			else
				value = Curve.Evaluate(ratio);

			_lastRatio = ratio;
			_lastValue = value;

			return value;
		}

		public void SetValue(float value)
		{
			_value.Value = value;
		}

		float GetRatio()
		{
			return Mathf.Clamp01((_value.Value - MinValue) / (MaxValue - MinValue));
		}

		public virtual void OnCreate()
		{
			if (Scope == RTPCScope.Local)
				_value = Pool<AudioValue<float>>.Create();
			else
				_value = GetGlobalRTPCValue(Name);

			_lastValue = Curve.Evaluate(GetRatio());
		}

		public virtual void OnRecycle()
		{
			if (Scope == RTPCScope.Local)
				Pool<AudioValue<float>>.Recycle(ref _value);
		}

		public void Copy(AudioRTPC reference)
		{
			_value = reference._value;
			_lastValue = reference._lastValue;
			_lastRatio = reference._lastRatio;
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

			if (!_rtpcValues.ContainsKey(name))
			{
				value = Pool<AudioValue<float>>.Create();
				_rtpcValues[name] = value;
			}
			else
				value = _rtpcValues[name];

			return value;
		}

		public static void SetGlobalRTPCValue(string name, float value)
		{
			GetGlobalRTPCValue(name).Value = value;
		}
	}
}