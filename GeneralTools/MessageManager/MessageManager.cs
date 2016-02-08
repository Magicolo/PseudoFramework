using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using UnityEngine.Assertions;

namespace Pseudo.Internal
{
	public class MessageManager : IMessageManager
	{
		public void Send<TId>(object target, TId identifier)
		{
			MessageGroup<TId>.Send(target, identifier, (object)null, (object)null, (object)null);
		}

		public void Send<TId, TArg>(object target, TId identifier, TArg argument)
		{
			MessageGroup<TId>.Send(target, identifier, argument, (object)null, (object)null);
		}

		public void Send<TId, TArg1, TArg2>(object target, TId identifier, TArg1 argument1, TArg2 argument2)
		{
			MessageGroup<TId>.Send(target, identifier, argument1, argument2, (object)null);
		}

		public void Send<TId, TArg1, TArg2, TArg3>(object target, TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			Assert.IsNotNull(target);

			MessageGroup<TId>.Send(target, identifier, argument1, argument2, argument3);
		}
	}
}
