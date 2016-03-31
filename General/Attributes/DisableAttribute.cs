using System;
using Pseudo.Editor.Internal;

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