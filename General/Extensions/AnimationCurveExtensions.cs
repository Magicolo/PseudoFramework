using UnityEngine;

namespace Pseudo
{
	public static class AnimationCurveExtensions
	{
		public static void Copy(this AnimationCurve target, AnimationCurve source)
		{
			if (target.length != source.length)
				target.keys = source.keys;
			else
			{
				for (int i = 0; i < source.length; i++)
					target.MoveKey(i, source[i]);
			}
		}

		public static AnimationCurve Clamp(this AnimationCurve curve, float minTime, float maxTime, float minValue, float maxValue)
		{
			var keys = curve.keys;

			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];

				if (key.time < minTime || key.time > maxTime || key.value < minValue || key.value > maxValue)
				{
					var newKey = new Keyframe
					{
						time = Mathf.Clamp(key.time, minTime, maxTime),
						value = Mathf.Clamp(key.value, minValue, maxValue),
						inTangent = key.inTangent,
						outTangent = key.outTangent
					};

					curve.MoveKey(i, newKey);
				}
			}

			return curve;
		}

		public static AnimationCurve Reversed(this AnimationCurve curve)
		{
			curve.keys = curve.keys.Reversed();

			return curve;
		}
	}
}
