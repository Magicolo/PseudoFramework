using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class MessageAttribute : Attribute
	{
		public object Identifier;

		public MessageAttribute(object identifier)
		{
			Identifier = identifier;
		}
	}
}
