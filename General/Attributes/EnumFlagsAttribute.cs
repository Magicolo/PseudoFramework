using System;
using Pseudo.Editor.Internal;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class EnumFlagsAttribute : CustomAttributeBase
	{
		public Type EnumType;

		public EnumFlagsAttribute() { }

		public EnumFlagsAttribute(Type enumType)
		{
			EnumType = enumType;
		}

		public EnumFlagsAttribute(string enumTypeName)
		{
			EnumType = TypeUtility.FindType(t => t.Is<Enum>() && t.Name.EndsWith(enumTypeName));
		}
	}
}