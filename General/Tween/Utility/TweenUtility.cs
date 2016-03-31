using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TweenUtility
	{
		public enum Ease : byte
		{
			Linear,
			InQuad,
			OutQuad,
			InOutQuad,
			OutInQuad,
			InCubic,
			OutCubic,
			InOutCubic,
			OutInCubic,
			SmoothStep
		}

		public static readonly Action EmptyAction = delegate { };
		public static readonly Action<float> EmptyFloatAction = delegate { };
		public static readonly Func<float> DefaultGetDeltaTime = () => UnityEngine.Time.unscaledDeltaTime;
		public static readonly Func<float> DefaultEditorGetDeltaTime = () => 0.01f;

		static readonly Func<float, float>[] easeFunctions =
		{
			Linear,
			InQuad,
			OutQuad,
			InOutQuad,
			OutInQuad,
			InCubic,
			OutCubic,
			InOutCubic,
			OutInCubic,
			SmoothStep
		};

		public static float Tween(Ease ease, float ratio)
		{
			return GetEaseFunction(ease)(ratio);
		}

		public static float Linear(float ratio)
		{
			return ratio;
		}

		public static float InQuad(float ratio)
		{
			return ratio * ratio;
		}

		public static float OutQuad(float ratio)
		{
			return -ratio * (ratio - 2f);
		}

		public static float InOutQuad(float ratio)
		{
			if (ratio < 0.5f)
				return InQuad(ratio * 2) / 2f;
			else
				return OutQuad(ratio * 2 - 1f) / 2f + 0.5f;
		}

		public static float OutInQuad(float ratio)
		{
			if (ratio < 0.5f)
				return OutQuad(ratio * 2) / 2f;
			else
				return InQuad(ratio * 2 - 1f) / 2f + 0.5f;
		}

		public static float InCubic(float ratio)
		{
			return InQuad(InQuad(ratio));
		}

		public static float OutCubic(float ratio)
		{
			return OutQuad(OutQuad(ratio));
		}

		public static float InOutCubic(float ratio)
		{
			if (ratio < 0.5f)
				return InCubic(ratio * 2) / 2f;
			else
				return OutCubic(ratio * 2 - 1f) / 2f + 0.5f;
		}

		public static float OutInCubic(float ratio)
		{
			if (ratio < 0.5f)
				return OutCubic(ratio * 2) / 2f;
			else
				return InCubic(ratio * 2 - 1f) / 2f + 0.5f;
		}

		public static float SmoothStep(float ratio)
		{
			return ratio * ratio * (3f - 2f * ratio);
		}

		public static Func<float, float> GetEaseFunction(Ease ease)
		{
			return easeFunctions[(byte)ease];
		}

		public static AnimationCurve ToAnimationCurve(Ease ease, int definition)
		{
			var keys = new Keyframe[definition];
			var easeFunction = GetEaseFunction(ease);

			for (int i = 0; i < definition; i++)
			{
				float ratio = (float)i / definition;
				keys[i] = new Keyframe(ratio, easeFunction(ratio));
			}

			return new AnimationCurve(keys);
		}
	}
}