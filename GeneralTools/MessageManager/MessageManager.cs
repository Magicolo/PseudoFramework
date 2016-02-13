using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using UnityEngine.Assertions;
using Pseudo.Internal.Communication;

namespace Pseudo
{
	public class MessageManager
	{
		readonly Dictionary<Type, object> typeToDispatcherGroup = new Dictionary<Type, object>();

		public void Send<TId>(object target, TId identifier)
		{
			Send(target, identifier, (object)null);
		}

		public void Send<TId, TArg>(object target, TId identifier, TArg argument)
		{
			Assert.IsNotNull(target);

			GetDispatcherGroup<TId>().Send(target, identifier, argument);
		}

		MessageDispatcherGroup<TId> GetDispatcherGroup<TId>()
		{
			var type = typeof(TId);
			object dispatcherGroup;

			if (!typeToDispatcherGroup.TryGetValue(type, out dispatcherGroup))
			{
				dispatcherGroup = new MessageDispatcherGroup<TId>();
				typeToDispatcherGroup[type] = dispatcherGroup;
			}

			return (MessageDispatcherGroup<TId>)dispatcherGroup;
		}
	}
}
