using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MaxAttribute : CustomAttributeBase
	{
		public float max = 1f;

		public MaxAttribute() { }

		public MaxAttribute(float max)
		{
			this.max = max;
		}
	}
}