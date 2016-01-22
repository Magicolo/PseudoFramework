﻿using Pseudo.Internal.Editor;
using System;
using System.Collections.Generic;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class TypePopupAttribute : CustomAttributeBase
	{
		public readonly Type[] Types;

		public TypePopupAttribute(params Type[] types)
		{
			Types = types;
		}

		public TypePopupAttribute(Type baseType, bool includeSelf, params Type[] excluding)
		{
			var types = new List<Type>(baseType.GetAssignableTypes(includeSelf));
			types.RemoveRange(excluding);
			Types = types.ToArray();
		}
	}
}