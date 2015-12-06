using UnityEngine;
using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class RequiresAttribute : Attribute
	{
		public bool CanBeNull = true;
		public Type[] Types;

		public RequiresAttribute(bool canBeNull, params Type[] types) : this(types)
		{
			CanBeNull = canBeNull;
		}

		public RequiresAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}
