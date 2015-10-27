using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

namespace Pseudo
{
	[Serializable]
	public class AudioOption : IPoolable, ICopyable<AudioOption>
	{
		public enum Types
		{
			VolumeScale,
			PitchScale,
			RandomVolume,
			RandomPitch,
			FadeIn,
			FadeOut,
			Loop,
			Clip,
			Output,
			Mute,
			Spatialize,
			BypassEffects,
			BypassListenerEffects,
			BypassReverbZones,
			Priority,
			StereoPan,
			DopplerLevel,
			MinDistance,
			MaxDistance,
			SpatialBlend,
			ReverbZoneMix,
			Spread,
			RolloffMode,
			PlayRange,
			Time,
			TimeSamples,
			VelocityUpdateMode,
			IgnoreListenerPause,
			IgnoreListenerVolume
		}

		[SerializeField]
		Types _type;
		[SerializeField]
		PDynamicValue _value;
		[SerializeField, Min]
		float _delay;

		public Types Type { get { return _type; } }
		public PDynamicValue Value { get { return _value; } }
		public float Delay { get { return _delay; } }

		public static readonly AudioOption Default = new AudioOption();

		public static AudioOption Clip(AudioClip clip, float delay = 0f)
		{
			return Create(Types.Clip, clip, delay);
		}

		public static AudioOption Output(AudioMixerGroup mixerGroup, float delay = 0f)
		{
			return Create(Types.Output, mixerGroup, delay);
		}

		public static AudioOption FadeIn(float fadeIn, Tweening.Ease ease = Tweening.Ease.OutQuad, float delay = 0f)
		{
			return Create(Types.FadeIn, new[] { fadeIn, (float)ease }, delay);
		}

		public static AudioOption FadeOut(float fadeOut, Tweening.Ease ease = Tweening.Ease.InQuad, float delay = 0f)
		{
			return Create(Types.FadeOut, new[] { fadeOut, (float)ease }, delay);
		}

		public static AudioOption VolumeScale(float volume, float time = 0f, Tweening.Ease ease = Tweening.Ease.Linear, float delay = 0f)
		{
			return Create(Types.VolumeScale, new[] { volume, time, (float)ease }, delay);
		}

		public static AudioOption PitchScale(float pitch, float time = 0f, Tweening.Ease ease = Tweening.Ease.Linear, float delay = 0f)
		{
			return Create(Types.PitchScale, new[] { pitch, time, (float)ease }, delay);
		}

		public static AudioOption RandomVolume(float randomVolume, float delay = 0f)
		{
			return Create(Types.RandomVolume, randomVolume, delay);
		}

		public static AudioOption RandomPitch(float randomPitch, float delay = 0f)
		{
			return Create(Types.RandomPitch, randomPitch, delay);
		}

		public static AudioOption Loop(bool loop, float delay = 0f)
		{
			return Create(Types.Loop, loop, delay);
		}

		public static AudioOption DopplerLevel(float dopplerLevel, float delay = 0f)
		{
			return Create(Types.DopplerLevel, dopplerLevel, delay);
		}

		public static AudioOption RolloffMode(AudioRolloffMode rolloff, float delay = 0f)
		{
			return Create(Types.RolloffMode, (int)rolloff, delay);
		}

		public static AudioOption RolloffMode(AnimationCurve rolloff, float delay = 0f)
		{
			return Create(Types.RolloffMode, rolloff, delay);
		}

		public static AudioOption MinDistance(float minDistance, float delay = 0f)
		{
			return Create(Types.MinDistance, minDistance, delay);
		}

		public static AudioOption MaxDistance(float maxDistance, float delay = 0f)
		{
			return Create(Types.MaxDistance, maxDistance, delay);
		}

		public static AudioOption Spread(float spread, float delay = 0f)
		{
			return Create(Types.Spread, spread, delay);
		}

		public static AudioOption Spread(AnimationCurve spread, float delay = 0f)
		{
			return Create(Types.Spread, spread, delay);
		}

		public static AudioOption Mute(bool mute, float delay = 0f)
		{
			return Create(Types.Mute, mute, delay);
		}

		public static AudioOption BypassEffects(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassEffects, bypass, delay);
		}

		public static AudioOption BypassListenerEffects(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassListenerEffects, bypass, delay);
		}

		public static AudioOption BypassReverbZones(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassReverbZones, bypass, delay);
		}

		public static AudioOption Priority(int priority, float delay = 0f)
		{
			return Create(Types.Priority, priority, delay);
		}

		public static AudioOption StereoPan(float stereoPan, float delay = 0f)
		{
			return Create(Types.StereoPan, stereoPan, delay);
		}

		public static AudioOption SpatialBlend(float spatialBlend, float delay = 0f)
		{
			return Create(Types.SpatialBlend, spatialBlend, delay);
		}

		public static AudioOption SpatialBlend(AnimationCurve spatialBlend, float delay = 0f)
		{
			return Create(Types.SpatialBlend, spatialBlend, delay);
		}

		public static AudioOption ReverbZoneMix(float reverbZoneMix, float delay = 0f)
		{
			return Create(Types.ReverbZoneMix, reverbZoneMix, delay);
		}

		public static AudioOption ReverbZoneMix(AnimationCurve reverbZoneMix, float delay = 0f)
		{
			return Create(Types.ReverbZoneMix, reverbZoneMix, delay);
		}

		public static AudioOption PlayRange(float start, float end, float delay = 0f)
		{
			return Create(Types.Time, new Vector2(start, end), delay);
		}

		public static AudioOption Time(float time, float delay = 0f)
		{
			return Create(Types.Time, time, delay);
		}

		public static AudioOption TimeSamples(int timeSamples, float delay = 0f)
		{
			return Create(Types.TimeSamples, timeSamples, delay);
		}

		public static AudioOption VelocityUpdateMode(AudioVelocityUpdateMode updateMode, float delay = 0f)
		{
			return Create(Types.VelocityUpdateMode, (int)updateMode, delay);
		}

		public static AudioOption IgnoreListenerPause(bool ignore, float delay = 0f)
		{
			return Create(Types.IgnoreListenerPause, ignore, delay);
		}

		public static AudioOption IgnoreListenerVolume(bool ignore, float delay = 0f)
		{
			return Create(Types.IgnoreListenerVolume, ignore, delay);
		}

		public static AudioOption Spatialize(bool spatialize, float delay = 0f)
		{
			return Create(Types.Spatialize, spatialize, delay);
		}

		static AudioOption Create(Types type, object value, float delay = 0f)
		{
			AudioOption option = Pool<AudioOption>.Create(Default);

			option.Initialize(type, value, delay);

			return option;
		}

		public object GetValue()
		{
			PDynamicValue.ValueTypes valueType;
			bool isArray;

			ToValueType(_type, HasCurve(), out valueType, out isArray);

			if (_value.GetValueType() != valueType || _value.IsArray != isArray)
				_value.SetValue(GetDefaultValue(_type));

			return _value.GetValue();
		}

		public T GetValue<T>()
		{
			return (T)GetValue();
		}

		public bool HasCurve()
		{
			if (_type == Types.SpatialBlend || _type == Types.ReverbZoneMix || _type == Types.Spread || _type == Types.RolloffMode)
				return _value.GetValue() is AnimationCurve;
			else
				return false;
		}

		public void Initialize(Types type, object value, float delay = 0f)
		{
			_type = type;
			_value = Pool<PDynamicValue>.Create(_value);
			_value.SetValue(value);
			_delay = delay;
		}

		public void OnCreate()
		{
			_value = Pool<PDynamicValue>.Create(_value);
		}

		public void OnRecycle()
		{
			Pool<PDynamicValue>.Recycle(ref _value);
		}

		public void Copy(AudioOption reference)
		{
			_type = reference._type;
			_value = reference._value;
			_delay = reference._delay;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType().Name, _type, _value);
		}

		public static object GetDefaultValue(Types type, bool hasCurve = false)
		{
			object defaultValue = null;

			switch (type)
			{
				case Types.VolumeScale:
					defaultValue = new float[] { 1f, 0f, 0f };
					break;
				case Types.PitchScale:
					defaultValue = new float[] { 1f, 0f, 0f };
					break;
				case Types.RandomVolume:
					defaultValue = 0f;
					break;
				case Types.RandomPitch:
					defaultValue = 0f;
					break;
				case Types.FadeIn:
					defaultValue = new float[] { 0f, 0f };
					break;
				case Types.FadeOut:
					defaultValue = new float[] { 0f, 0f };
					break;
				case Types.Loop:
					defaultValue = false;
					break;
				case Types.Clip:
					defaultValue = null;
					break;
				case Types.Output:
					defaultValue = null;
					break;
				case Types.DopplerLevel:
					defaultValue = 1f;
					break;
				case Types.RolloffMode:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = AudioRolloffMode.Logarithmic;
					break;
				case Types.MinDistance:
					defaultValue = 1f;
					break;
				case Types.MaxDistance:
					defaultValue = 500f;
					break;
				case Types.Spread:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 0f;
					break;
				case Types.Mute:
					defaultValue = false;
					break;
				case Types.BypassEffects:
					defaultValue = false;
					break;
				case Types.BypassListenerEffects:
					defaultValue = false;
					break;
				case Types.BypassReverbZones:
					defaultValue = false;
					break;
				case Types.Priority:
					defaultValue = 128;
					break;
				case Types.StereoPan:
					defaultValue = 0f;
					break;
				case Types.SpatialBlend:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 0f;
					break;
				case Types.ReverbZoneMix:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 1f;
					break;
				case Types.PlayRange:
					defaultValue = Vector2.zero;
					break;
				case Types.Time:
					defaultValue = 0f;
					break;
				case Types.TimeSamples:
					defaultValue = 0;
					break;
				case Types.VelocityUpdateMode:
					defaultValue = AudioVelocityUpdateMode.Auto;
					break;
				case Types.IgnoreListenerPause:
					defaultValue = false;
					break;
				case Types.IgnoreListenerVolume:
					defaultValue = false;
					break;
				case Types.Spatialize:
					defaultValue = false;
					break;
			}

			return defaultValue;
		}

		public static void ToValueType(Types type, bool hasCurve, out PDynamicValue.ValueTypes valueType, out bool isArray)
		{
			valueType = PDynamicValue.ValueTypes.Null;
			isArray = false;

			switch (type)
			{
				case Types.VolumeScale:
					valueType = PDynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.PitchScale:
					valueType = PDynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.RandomVolume:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.RandomPitch:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.FadeIn:
					valueType = PDynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.FadeOut:
					valueType = PDynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.Loop:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.Clip:
					valueType = PDynamicValue.ValueTypes.Object;
					break;
				case Types.Output:
					valueType = PDynamicValue.ValueTypes.Object;
					break;
				case Types.DopplerLevel:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.RolloffMode:
					valueType = hasCurve ? PDynamicValue.ValueTypes.AnimationCurve : PDynamicValue.ValueTypes.Int;
					break;
				case Types.MinDistance:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.MaxDistance:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.Spread:
					valueType = hasCurve ? PDynamicValue.ValueTypes.AnimationCurve : PDynamicValue.ValueTypes.Float;
					break;
				case Types.Mute:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassEffects:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassListenerEffects:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassReverbZones:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.Priority:
					valueType = PDynamicValue.ValueTypes.Int;
					break;
				case Types.StereoPan:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.SpatialBlend:
					valueType = hasCurve ? PDynamicValue.ValueTypes.AnimationCurve : PDynamicValue.ValueTypes.Float;
					break;
				case Types.ReverbZoneMix:
					valueType = hasCurve ? PDynamicValue.ValueTypes.AnimationCurve : PDynamicValue.ValueTypes.Float;
					break;
				case Types.PlayRange:
					valueType = PDynamicValue.ValueTypes.Vector2;
					break;
				case Types.Time:
					valueType = PDynamicValue.ValueTypes.Float;
					break;
				case Types.TimeSamples:
					valueType = PDynamicValue.ValueTypes.Int;
					break;
				case Types.VelocityUpdateMode:
					valueType = PDynamicValue.ValueTypes.Int;
					break;
				case Types.IgnoreListenerPause:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.IgnoreListenerVolume:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
				case Types.Spatialize:
					valueType = PDynamicValue.ValueTypes.Bool;
					break;
			}
		}
	}
}