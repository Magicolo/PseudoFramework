using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class SliderAttribute : CustomAttributeBase
	{
		public float min = 0f;
		public float max = 1f;

		public SliderAttribute() { }

		public SliderAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}