using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public static class InjectionUtility
	{
		public static readonly object[] EmptyObjects = new object[0];
		public static readonly object[] EmptyArguments = new object[0];
		public static readonly IInjectableParameter[] EmptyParameters = new IInjectableParameter[0];
	}
}
