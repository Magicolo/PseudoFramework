using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class MethodCopier<T> : Copier<T> where T : class
	{
		readonly Action<T, T> method;

		public MethodCopier(Action<T, T> method)
		{
			this.method = method;
		}

		public override void CopyTo(T source, T target)
		{
			method(source, target);
		}
	}
}
