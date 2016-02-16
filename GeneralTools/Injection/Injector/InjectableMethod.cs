using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableMethod : IInjectableMember
	{
		public MemberInfo Member
		{
			get { return method; }
		}

		readonly MethodInfo method;
		readonly IInjectableParameter[] parameters;
		readonly InjectAttribute attribute;
		readonly object[] arguments;

		public InjectableMethod(MethodInfo method, IInjectableParameter[] parameters)
		{
			this.method = method;
			this.parameters = parameters;

			attribute = (InjectAttribute)method.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
			arguments = new object[parameters.Length];
		}

		public void Inject(object instance, IResolver resolver)
		{
			if (!attribute.Optional || CanInject(resolver))
			{
				for (int i = 0; i < parameters.Length; i++)
					parameters[i].Inject(instance, arguments, i, resolver);

				method.Invoke(instance, arguments);
				arguments.Clear();
			}
		}

		public bool CanInject(IResolver resolver)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].CanInject(resolver))
					return false;
			}

			return true;
		}
	}
}
