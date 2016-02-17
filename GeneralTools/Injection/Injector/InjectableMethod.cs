﻿using UnityEngine;
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

		public void Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!context.Optional || CanInject(ref context))
			{
				for (int i = 0; i < parameters.Length; i++)
					parameters[i].Inject(context, arguments, i);

				method.Invoke(context.Instance, arguments);
				arguments.Clear();
			}
		}

		bool CanInject(ref InjectionContext context)
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
			context.Type = InjectionContext.Types.Method;
			context.DeclaringType = method.DeclaringType;
			context.Member = method;
			context.Optional = attribute.Optional;
			context.Identifier = attribute.Identifier;
		}
	}
}
