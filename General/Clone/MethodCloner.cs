using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class MethodCloner<T> : Cloner<T>
	{
		readonly Func<T, T> method;

		public MethodCloner(Func<T, T> method)
		{
			this.method = method;
		}

		public override T Clone(T reference)
		{
			return method(reference);
		}
	}
}
