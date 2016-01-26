using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.EntityOld
{
	public class Messager<T> : MessagerBase
	{
		readonly Action<T> action;

		public Messager(Action<T> action)
		{
			this.action = action;
		}

		public override void SendMessage(object target)
		{
			action((T)target);
		}
	}

	public class Messager<T, A> : MessagerBase<A>
	{
		readonly Action<T, A> action;

		public Messager(Action<T, A> action)
		{
			this.action = action;
		}

		public override void SendMessage(object target, A argument)
		{
			action((T)target, argument);
		}
	}

	public abstract class MessagerBase : IMessager
	{
		public abstract void SendMessage(object target);

		void IMessager.SendMessage(object target)
		{
			SendMessage(target);
		}

		void IMessager.SendMessage(object target, object argument)
		{
			SendMessage(target);
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