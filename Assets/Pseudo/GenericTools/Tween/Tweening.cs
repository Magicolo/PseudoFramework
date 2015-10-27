using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class Tweening
	{
		public enum Ease
		{
			Linear,
			InQuad,
			OutQuad,
			InOutQuad,
			OutInQuad,
			SmoothStep
		}

		public static readonly Action EmptyAction = () => { };
		public static readonly Action<float> EmptyFloatAction = value => { };
		public static readonly Func<float> DefaultGetDeltaTime = () => Time.unscaledDeltaTime;
		public static readonly Func<float> DefaultEditorGetDeltaTime = () => 0.01f;

		static readonly Func<float, float> linearEase = ratio => ratio;
		static readonly Func<float, float> inQuadEase = ratio => ratio * ratio;
		static readonly Func<float, float> outQuadEase = ratio => ratio * (ratio - 2f) * -1f;
		static readonly Func<float, float> inOutQuadEase = ratio =>
		{
			if (ratio < 0.5f)
				return inQuadEase(ratio * 2) / 2f;
			else
				return outQuadEase(ratio * 2 - 1f) / 2f + 0.5f;
		};
		static readonly Func<float, float> _outInQuadEase = ratio =>
		{
			if (ratio < 0.5f)
				return outQuadEase(ratio * 2) / 2f;
			else
				return inQuadEase(ratio * 2 - 1f) / 2f + 0.5f;
		};
		static readonly Func<float, float> smoothStepEase = ratio => ratio * ratio * (3f - 2f * ratio);

		public static Func<float, float> ToEaseFunction(Ease ease)
		{
			switch (ease)
			{
				default:
					return linearEase;
				case Ease.InQuad:
					return inQuadEase;
				case Ease.OutQuad:
					return outQuadEase;
				case Ease.InOutQuad:
					return inOutQuadEase;
				case Ease.OutInQuad:
					return _outInQuadEase;
				case Ease.SmoothStep:
					return smoothStepEase;
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