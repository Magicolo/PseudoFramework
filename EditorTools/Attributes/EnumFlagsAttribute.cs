<<<<<<< HEAD
﻿using System;
using Pseudo.Internal.Editor;

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
	}
=======
﻿using System;
using Pseudo.Internal.Editor;

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
			EnumType = Array.Find(TypeExtensions.GetAssignableTypes(typeof(Enum)), type => type.Name.EndsWith(enumTypeName));
		}
	}
>>>>>>> Entity2
}