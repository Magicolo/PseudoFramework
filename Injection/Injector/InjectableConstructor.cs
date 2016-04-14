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
	public class InjectableConstructor : InjectableMemberBase<ConstructorInfo>, IInjectableConstructor
	{
		public ConstructorInfo Constructor
		{
			get { return member; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return parameters; }
		}

		readonly IInjectableParameter[] parameters;
		readonly object[] arguments;

		public InjectableConstructor(ConstructorInfo constructor, IInjectableParameter[] parameters) : base(constructor)
		{
			this.parameters = parameters;

			arguments = new object[parameters.Length];
		}

		public override bool CanInject(InjectionContext context)
		{
			SetupContext(ref context);

			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].CanInject(context))
					return false;
			}

			return true;
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Constructor;
			context.DeclaringType = member.DeclaringType;
			context.Identifier = attribute.Identifier;
			context.Optional = attribute.Optional;
		}

		protected override object Inject(ref InjectionContext context)
		{
			SetupContext(ref context);

			for (int i = 0; i < parameters.Length; i++)
				arguments[i] = parameters[i].Inject(context);

			var instance = member.Invoke(arguments);
			arguments.Clear();

			return instance;
		}
	}
}
