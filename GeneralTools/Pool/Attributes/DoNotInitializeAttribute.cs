using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = true)]
	public sealed class DoNotInitializeAttribute : Attribute { }
}