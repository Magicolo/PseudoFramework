using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MaskAttribute : CustomAttributeBase
	{
		public int Filter = -1;

		public MaskAttribute() { }

		public MaskAttribute(object filter)
		{
			this.Filter = (int)filter;
		}
	}
}