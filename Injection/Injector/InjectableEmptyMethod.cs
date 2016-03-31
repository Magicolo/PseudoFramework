using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public class InjectableEmptyMethod : IInjectableMember
	{
		public MemberInfo Member
		{
			get { return method; }
		}

		readonly MethodInfo method;
		readonly InvokerBase invoker;

		public InjectableEmptyMethod(MethodInfo method)
		{
			this.method = method;

			var action = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(method.DeclaringType), method);
			invoker = (InvokerBase)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(method.DeclaringType), action);
		}

		public void Inject(InjectionContext context)
		{
			invoker.Invoke(context.Instance);
		}
	}
}
