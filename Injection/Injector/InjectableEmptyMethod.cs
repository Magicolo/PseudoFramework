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
		public IInjectableParameter[] Parameters
		{
			get { return InjectionUtility.EmptyParameters; }
		}

		readonly InvokerBase invoker;

		public InjectableEmptyMethod(MethodInfo method) : base(method)
		{
			var action = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(member.DeclaringType), member);
			invoker = (InvokerBase)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(member.DeclaringType), action);
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return true;
		}

		protected override object Inject(ref InjectionContext context)
		{
			invoker.Invoke(context.Instance);

			return null;
		}
	}
}
