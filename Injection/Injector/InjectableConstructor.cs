using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.Injection.Internal
{
	public class InjectableConstructor : InjectableMemberBase<ConstructorInfo>, IInjectableConstructor
	{
		public IInjectableParameter[] Parameters
		{
			get { return parameters; }
		}

		readonly IInjectableParameter[] parameters;
		readonly IConstructorWrapper wrapper;
		readonly object[] arguments;

		public InjectableConstructor(ConstructorInfo constructor, IInjectableParameter[] parameters) : base(constructor)
		{
			this.parameters = parameters;

			wrapper = constructor.CreateWrapper();
			arguments = new object[parameters.Length];
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Constructor;
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
			for (int i = 0; i < arguments.Length; i++)
				arguments[i] = parameters[i].Inject(context);

			var instance = wrapper.Invoke(arguments);

			return instance;
		}
	}
}