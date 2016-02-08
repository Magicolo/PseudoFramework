using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class MessageDispatcherBase : IMessageDispatcher
	{
		public abstract void Send(object target);

		void IMessageDispatcher.Send(object target, object argument1, object argument2, object argument3)
		{
			Send(target);
		}
	}

	public abstract class MessageDispatcherBase<TArg> : IMessageDispatcher
	{
		public abstract void Send(object target, TArg argument);

		void IMessageDispatcher.Send(object target, object argument1, object argument2, object argument3)
		{
			Send(target, argument1 is TArg ? (TArg)argument1 : default(TArg));
		}
	}

	public abstract class MessageDispatcherBase<TArg1, TArg2> : IMessageDispatcher
	{
		public abstract void Send(object target, TArg1 argument1, TArg2 argument2);

		void IMessageDispatcher.Send(object target, object argument1, object argument2, object argument3)
		{
			Send(target,
				argument1 is TArg1 ? (TArg1)argument1 : default(TArg1),
				argument2 is TArg2 ? (TArg2)argument1 : default(TArg2));
		}
	}

	public abstract class MessageDispatcherBase<TArg1, TArg2, TArg3> : IMessageDispatcher
	{
		public abstract void Send(object target, TArg1 argument1, TArg2 argument2, TArg3 argument3);

		void IMessageDispatcher.Send(object target, object argument1, object argument2, object argument3)
		{
			Send(target,
				argument1 is TArg1 ? (TArg1)argument1 : default(TArg1),
				argument2 is TArg2 ? (TArg2)argument1 : default(TArg2),
				argument3 is TArg3 ? (TArg3)argument1 : default(TArg3));
		}
	}
}
