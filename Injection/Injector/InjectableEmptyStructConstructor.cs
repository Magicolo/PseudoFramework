using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pseudo.Injection.Internal
{
	public class InjectableEmptyStructConstructor : IInjectableConstructor
	{
		static readonly IInjectableParameter[] parameters = new IInjectableParameter[0];

		public ConstructorInfo Constructor
		{
			get { return null; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return parameters; }
		}

		readonly Type type;

		public InjectableEmptyStructConstructor(Type type)
		{
			this.type = type;
		}

		public object Inject(InjectionContext context)
		{
			return FormatterServices.GetUninitializedObject(type);
		}

		public bool CanInject(InjectionContext context)
		{
			return true;
		}
	}
}
