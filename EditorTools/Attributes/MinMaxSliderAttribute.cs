using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MinMaxSliderAttribute : CustomAttributeBase
	{
		public float min = 0f;
		public float max = 1f;
		public string minLabel;
		public string maxLabel;

		public MinMaxSliderAttribute() { }

		public MinMaxSliderAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public MinMaxSliderAttribute(float min, float max, string minLabel, string maxLabel)
		{
			this.min = min;
			this.max = max;
			this.minLabel = minLabel;
			this.maxLabel = maxLabel;
		}
	}
}