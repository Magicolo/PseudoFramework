using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Communication
{
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple = true, Inherited = true)]
	public sealed class MessageEnumAttribute : Attribute { }
}
