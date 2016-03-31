using UnityEngine;
using System;
using Pseudo.Editor.Internal;

namespace Pseudo.EntityFramework
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
	public sealed class EntityRequiresAttribute : Attribute
	{
		public bool CanBeNull = true;
		public Type[] Types;

		public EntityRequiresAttribute(params Type[] types) : this(true, types) { }

		public EntityRequiresAttribute(bool canBeNull, params Type[] types)
		{
			CanBeNull = canBeNull;
			Types = types;
		}
	}
}
