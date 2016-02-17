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
				parameters[i].Inject(context, arguments, i);

			var instance = constructor.Invoke(arguments);
			arguments.Clear();

			return instance;
		}

		public bool CanInject(ref InjectionContext context)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].CanInject(ref context))
					return false;
			}

			return true;
		}

		void SetupContext(ref InjectionContext context)
		{
			context.Type = InjectionContext.Types.Constructor;
			context.DeclaringType = constructor.DeclaringType;
			context.Member = constructor;
			context.Optional = attribute.Optional;
			context.Identifier = attribute.Identifier;
		}
	}
}
