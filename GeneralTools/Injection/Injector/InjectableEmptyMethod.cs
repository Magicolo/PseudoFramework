using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableEmptyMethod : IInjectableMember
	{
		class Invoker<T> : IInvoker
		{
			readonly Action<T> action;

			public Invoker(Action<T> action)
			{
				this.action = action;
			}

			public void Invoke(T instance)
			{
				action(instance);
			}

			void IInvoker.Invoke(object instance)
			{
				Invoke((T)instance);
			}
		}

		interface IInvoker
		{
			void Invoke(object instance);
		}

		public MemberInfo Member
		{
			get { return method; }
		}

		readonly MethodInfo method;
		readonly IInvoker invoker;

		public InjectableEmptyMethod(MethodInfo method)
		{
			this.method = method;

			var action = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(method.DeclaringType), method);
			invoker = (IInvoker)Activator.CreateInstance(typeof(Invoker<>).MakeGenericType(method.DeclaringType), action);
		}

		public void Inject(InjectionContext context)
		{
			invoker.Invoke(context.Instance);
		}
	}
}
