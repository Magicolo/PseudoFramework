using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class MessageDispatcher<T> : MessageDispatcherBase
	{
		Action<T> message = delegate { };

		public MessageDispatcher(Action<T> message)
		{
			this.message = message;
		}

		public override void Send(object target)
		{
			message((T)target);
		}
	}

	public class MessageDispatcher<T, TArg> : MessageDispatcherBase<TArg>
	{
		Action<T, TArg> message = delegate { };

		public MessageDispatcher(Action<T, TArg> message)
		{
			this.message = message;
		}

		public override void Send(object target, TArg argument)
		{
			message((T)target, argument);
		}
	}

	public class MessageDispatcher<T, TArg1, TArg2> : MessageDispatcherBase<TArg1, TArg2>
	{
		Action<T, TArg1, TArg2> message = delegate { };

		public MessageDispatcher(Action<T, TArg1, TArg2> message)
		{
			this.message = message;
		}

		public override void Send(object target, TArg1 argument1, TArg2 argument2)
		{
			message((T)target, argument1, argument2);
		}
	}

	public class MessageDispatcher<T, TArg1, TArg2, TArg3> : MessageDispatcherBase<TArg1, TArg2, TArg3>
	{
		Action<T, TArg1, TArg2, TArg3> message = delegate { };

		public MessageDispatcher(Action<T, TArg1, TArg2, TArg3> message)
		{
			this.message = message;
		}

		public override void Send(object target, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			message((T)target, argument1, argument2, argument3);
		}
	}
}
