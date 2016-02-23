using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Communication
{
	public class MessageDispatcher<TId>
	{
		readonly TId identifier;
		readonly Dictionary<object, Delegate> targetToReceiver = new Dictionary<object, Delegate>();

		public MessageDispatcher(TId identifier)
		{
			this.identifier = identifier;
		}

		public void Send<TArg>(object target, TArg argument)
		{
			var dispatcher = GetMethod(target);

			if (dispatcher is Action<TArg>)
				((Action<TArg>)dispatcher)(argument);
			else if (dispatcher is Action)
				((Action)dispatcher)();
			else if (dispatcher != null)
				throw new MethodSignatureMismatchException();
		}

		Delegate GetMethod(object target)
		{
			Delegate method;

			if (!targetToReceiver.TryGetValue(target, out method))
			{
				method = MessageUtility.CreateMethod(target, identifier);
				targetToReceiver[target] = method;
			}

			return method;
		}
	}
}
