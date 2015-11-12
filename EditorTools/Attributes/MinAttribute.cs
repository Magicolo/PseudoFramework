using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MinAttribute : CustomAttributeBase
	{
		public float min = 0f;

		public MinAttribute() { }

		public MinAttribute(float min)
		{
			this.min = min;
		}
	}
}