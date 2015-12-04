using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Entity
{
	public class Messager<T> : IMessager
	{
		readonly Action<T> action;

		public Messager(Action<T> action)
		{
			this.action = action;
		}

		public void SendMessage(T target)
		{
			action(target);
		}

		void IMessager.SendMessage(object target)
		{
			SendMessage((T)target);
		}

		void IMessager.SendMessage(object target, object argument)
		{
			SendMessage((T)target);
		}
	}

	public class Messager<T, A> : MessagerBase<A>
	{
		readonly Action<T, A> action;

		public Messager(Action<T, A> action)
		{
			this.action = action;
		}

		public void SendMessage(T target, A argument)
		{
			action(target, argument);
		}

		public override void SendMessage(object target, A argument)
		{
			SendMessage((T)target, argument);
		}
	}

	public abstract class MessagerBase<A> : IMessager
	{
		public abstract void SendMessage(object target, A argument);

		void IMessager.SendMessage(object target)
		{
			SendMessage(target, default(A));
		}

		void IMessager.SendMessage(object target, object argument)
		{
			SendMessage(target, (A)argument);
		}
	}
}