using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Scripting;

namespace Pseudo.Injection
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class InjectAttribute : PreserveAttribute
	{
		public readonly object Identifier;
		public readonly bool Optional;

		public InjectAttribute(object identifier = null, bool optional = false)
		{
			Identifier = identifier;
			Optional = optional;
		}
	}
}
