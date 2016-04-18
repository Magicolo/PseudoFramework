using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Injection.Internal
{
	public class InjectableMethod : InjectableMemberBase<MethodInfo>, IInjectableMethod
	{
		public IInjectableParameter[] Parameters
		{
			get { return parameters; }
		}

		readonly IInjectableParameter[] parameters;
		readonly object[] arguments;

		public InjectableMethod(MethodInfo method, IInjectableParameter[] parameters) : base(method)
		{
			this.parameters = parameters;

			arguments = new object[parameters.Length];
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Method;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].CanInject(context))
					return false;
			}

			return true;
		}

		protected override object Inject(ref InjectionContext context)
		{
			for (int i = 0; i < parameters.Length; i++)
				arguments[i] = parameters[i].Inject(context);

			var returnValue = member.Invoke(context.Instance, arguments);
			arguments.Clear();

			return returnValue;
		}
	}
}
