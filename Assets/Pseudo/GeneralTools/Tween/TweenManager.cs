using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TweenManager
	{
		public enum Ease
		{
			Linear,
			InQuad,
			OutQuad,
			InOutQuad,
			OutInQuad,
		}

		public static readonly Action EmptyAction = () => { };
		public static readonly Action<float> EmptyFloatAction = value => { };
		public static readonly Func<float> DefaultGetDeltaTime = () => Time.unscaledDeltaTime;
		public static readonly Func<float> DefaultEditorGetDeltaTime = () => 0.01f;

		static readonly Func<float, float> _linearEase = ratio => ratio;
		static readonly Func<float, float> _inQuadEase = ratio => ratio * ratio;
		static readonly Func<float, float> _outQuadEase = ratio => ratio * (ratio - 2f) * -1f;
		static readonly Func<float, float> _inOutQuadEase = ratio =>
		{
			if (ratio < 0.5f)
				return _inQuadEase(ratio * 2) / 2f;
			else
				return _outQuadEase(ratio * 2 - 1f) / 2f + 0.5f;
		};
		static readonly Func<float, float> _outInQuadEase = ratio =>
		{
			if (ratio < 0.5f)
				return _outQuadEase(ratio * 2) / 2f;
			else
				return _inQuadEase(ratio * 2 - 1f) / 2f + 0.5f;
		};

		public static IEnumerator GetWaitRoutine(float time, Func<float> getDeltaTime, float delay = 0f, Action startCallback = null, Action<float> updateCallback = null, Action endCallback = null)
		{
			if (Application.isPlaying)
				getDeltaTime = getDeltaTime ?? DefaultGetDeltaTime;
			else
				getDeltaTime = getDeltaTime ?? DefaultEditorGetDeltaTime;
			startCallback = startCallback ?? EmptyAction;
			updateCallback = updateCallback ?? EmptyFloatAction;
			endCallback = endCallback ?? EmptyAction;

			for (float counter = 0f; counter < delay; counter += getDeltaTime())
				yield return 0f;

			startCallback();

			for (float counter = 0f; counter < time; counter += getDeltaTime())
			{
				float completion = Mathf.Clamp01(counter / time);

				yield return counter;
				updateCallback(completion);
			}

			yield return time;

			endCallback();
		}

		public static IEnumerator GetRampValueRoutine(Func<float> getValue, Action<float> setValue, float end, float time, Func<float, float> easeFunction = null, Func<float> getDeltaTime = null, float delay = 0f, Action startCallback = null, Action<float> updateCallback = null, Action endCallback = null)
		{
			setValue = setValue ?? EmptyFloatAction;
			easeFunction = easeFunction ?? _linearEase;
			if (Application.isPlaying)
				getDeltaTime = getDeltaTime ?? DefaultGetDeltaTime;
			else
				getDeltaTime = getDeltaTime ?? DefaultEditorGetDeltaTime;
			startCallback = startCallback ?? EmptyAction;
			updateCallback = updateCallback ?? EmptyFloatAction;
			endCallback = endCallback ?? EmptyAction;

			for (float counter = 0f; counter < delay; counter += getDeltaTime())
				yield return getValue();

			float start = getValue();

			startCallback();
			setValue(start);

			for (float counter = 0f; counter < time; counter += getDeltaTime())
			{
				float completion = Mathf.Clamp01(counter / time);
				float ratio = easeFunction(completion);
				float current = (end - start) * ratio + start;

				setValue(current);
				yield return current;
				updateCallback(completion);
			}

			setValue(end);
			yield return end;

			endCallback();
		}

		public static Func<float, float> ToEaseFunction(Ease ease)
		{
			switch (ease)
			{
				default:
					return _linearEase;
				case Ease.InQuad:
					return _inQuadEase;
				case Ease.OutQuad:
					return _outQuadEase;
				case Ease.InOutQuad:
					return _inOutQuadEase;
				case Ease.OutInQuad:
					return _outInQuadEase;
			}
		}

		public static AnimationCurve ToCurve(Ease ease, int definition)
		{
			Keyframe[] keys = new Keyframe[definition];
			Func<float, float> easeFunction = ToEaseFunction(ease);

			for (int i = 0; i < definition; i++)
			{
				float ratio = (float)i / definition;
				keys[i] = new Keyframe(ratio, easeFunction(ratio));
			}

			return new AnimationCurve(keys);
		}
	}
}