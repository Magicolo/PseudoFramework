using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class DisableAttribute : CustomAttributeBase
	{
		public DisableAttribute()
		{
			DisableOnPlay = true;
			DisableOnStop = true;
		}
	}
}