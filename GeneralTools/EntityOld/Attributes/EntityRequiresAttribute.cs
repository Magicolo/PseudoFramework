using UnityEngine;
using System;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.EntityOld
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
	public sealed class EntityRequiresAttribute : Attribute
	{
		public bool CanBeNull = true;
		public Type[] Types;

		public EntityRequiresAttribute(bool canBeNull, params Type[] types) : this(types)
		{
			CanBeNull = canBeNull;
		}

		public EntityRequiresAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}
