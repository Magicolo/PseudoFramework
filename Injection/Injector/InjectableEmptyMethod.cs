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
	public class InjectableEmptyMethod : InjectableMemberBase<MethodInfo>, IInjectableMethod
	{
		static readonly IInjectableParameter[] emptyParameters = new IInjectableParameter[0];

		public MethodInfo Method
		{
			get { return member; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return emptyParameters; }
		}

		readonly InvokerBase invoker;

		public InjectableEmptyMethod(MethodInfo method) : base(method)
		{
			var action = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(member.DeclaringType), member);
			invoker = (InvokerBase)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(member.DeclaringType), action);
		}

		public override bool CanInject(InjectionContext context)
		{
			return true;
		}

		protected override void SetupContext(ref InjectionContext context) { }

		protected override object Inject(ref InjectionContext context)
		{
			invoker.Invoke(context.Instance);

			return null;
		}
	}
}
