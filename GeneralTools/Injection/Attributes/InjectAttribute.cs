using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Scripting;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class InjectAttribute : PreserveAttribute
	{
		public string Identifier;
		public bool Optional;
	}
}
