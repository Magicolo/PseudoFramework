using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableConstructor : IInjectableConstructor
	{
		public ConstructorInfo Constructor
		{
			get { return constructor; }
		}

		readonly ConstructorInfo constructor;
		readonly IInjectableParameter[] parameters;
		//readonly InjectAttribute attribute;
		readonly object[] arguments;

		public InjectableConstructor(ConstructorInfo constructor, IInjectableParameter[] parameters)
		{
			this.constructor = constructor;
			this.parameters = parameters;

			//attribute = (InjectAttribute)constructor.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
			arguments = new object[parameters.Length];
		}

		public object Inject(IResolver resolver, object[] additional)
		{
			for (int i = 0; i < parameters.Length - additional.Length; i++)
				parameters[i].Inject(null, arguments, i, resolver);

			additional.CopyTo(arguments, parameters.Length - additional.Length);

			var instance = constructor.Invoke(arguments);
			arguments.Clear();

			return instance;
		}

		public bool CanInject(IResolver resolver, object[] additional)
		{
			if (additional.Length > parameters.Length)
				return false;

			bool canResolve = true;
			int index = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];

				if (canResolve)
					canResolve &= parameter.CanInject(resolver);

				if (canResolve)
					continue;
				else if (index >= additional.Length)
					return false;
				else
				{
					var argument = additional[index++];

					if (argument != null && !parameter.Parameter.ParameterType.IsAssignableFrom(argument.GetType()))
						return false;
				}
			}

			return true;
		}
	}
}
