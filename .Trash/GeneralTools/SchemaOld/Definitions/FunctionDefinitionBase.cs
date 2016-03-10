using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Schema
{
	public abstract class FunctionDefinitionBase<TFunction> : IFunctionDefinition where TFunction : class
	{
		public string Name { get; private set; }
		public readonly TFunction Method;

		protected FunctionDefinitionBase(object instance, MethodInfo method)
		{
			Name = method.Name;
			Method = Delegate.CreateDelegate(typeof(TFunction), instance, method) as TFunction;
		}

		protected FunctionDefinitionBase(TFunction method)
		{
			Method = method;
		}

		public abstract IFunctionNode CreateNode();
	}

}
