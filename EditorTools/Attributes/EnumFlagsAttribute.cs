using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class EnumFlagsAttribute : CustomAttributeBase
	{
		public Type EnumType;

		public EnumFlagsAttribute(Type enumType)
		{
			EnumType = enumType;
		}
	}
}