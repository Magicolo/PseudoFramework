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
		readonly Dictionary<object, Delegate> targetToMethod = new Dictionary<object, Delegate>();

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
				throw new ArgumentException(string.Format("Argument signature does not exactly match target method's signature. Inheritance is not supported for AOT compiling readons."));

			if (target is IMessageable)
				((IMessageable)target).OnMessage(identifier);

			if (target is IMessageable<TId>)
				((IMessageable<TId>)target).OnMessage(identifier);
		}

		Delegate GetMethod(object target)
		{
			Delegate method;

			if (!targetToMethod.TryGetValue(target, out method))
			{
				method = MessageUtility.CreateMethod(target, identifier);
				targetToMethod[target] = method;
			}

			return method;
		}
	}
}
