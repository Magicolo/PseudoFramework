using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Scripting;

namespace Pseudo.Communication
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class MessageAttribute : PreserveAttribute
	{
		public readonly object Identifier;

		public MessageAttribute(object identifier)
		{
			Identifier = identifier;
		}
	}
}
