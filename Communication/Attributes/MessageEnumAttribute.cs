using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Communication
{
	[AttributeUsage(AttributeTargets.Enum, Inherited = true)]
	public sealed class MessageEnumAttribute : Attribute { }
}
