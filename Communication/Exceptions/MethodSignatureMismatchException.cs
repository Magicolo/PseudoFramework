using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Communication.Internal
{
	public class MethodSignatureMismatchException : Exception
	{
		public MethodSignatureMismatchException() :
			base(string.Format("Argument signature does not exactly match target method's signature. Inheritance is not supported for AOT compiling reasons."))
		{ }
	}
}
