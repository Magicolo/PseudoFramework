using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public class EmptyAttributeProvider : ICustomAttributeProvider
	{
		public object[] GetCustomAttributes(bool inherit)
		{
			return new object[0];
		}

		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return new object[0];
		}

		public bool IsDefined(Type attributeType, bool inherit)
		{
			return false;
		}
	}
}
