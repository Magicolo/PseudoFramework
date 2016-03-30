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
		public IInjectableParameter[] Parameters
		{
			get { return parameters; }
		}

		readonly ConstructorInfo constructor;
		readonly IInjectableParameter[] parameters;
		readonly InjectAttribute attribute;
		readonly object[] arguments;

		public InjectableConstructor(ConstructorInfo constructor, IInjectableParameter[] parameters)
		{
			this.constructor = constructor;
			this.parameters = parameters;

			attribute = (InjectAttribute)constructor.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
			arguments = new object[parameters.Length];
		}

		public object Inject(InjectionContext context)
		{
			SetupContext(ref context);

			for (int i = 0; i < parameters.Length; i++)
				arguments[i] = parameters[i].Resolve(context);

			var instance = constructor.Invoke(arguments);
			arguments.Clear();

			return instance;
		}

		public bool CanInject(InjectionContext context)
		{
			SetupContext(ref context);

			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].CanResolve(context))
					return false;
			}

			return true;
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Constructor;
			context.Member = constructor;
			context.Attribute = attribute;
		}
	}
}
