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
	public class MessageManager : IMessageManager
	{
		readonly Dictionary<Type, object> typeToDispatcherGroup = new Dictionary<Type, object>();
		readonly List<IMessageable> receivers = new List<IMessageable>();

		public void SubscribeAll(IMessageable receiver)
		{
			receivers.Add(receiver);
		}

		public void Subscribe<TId>(IMessageable<TId> receiver)
		{
			Assert.IsNotNull(receiver);

			GetDispatcherGroup<TId>().Subscribe(receiver);
		}

		public void UnsubscribeAll(IMessageable receiver)
		{
			receivers.Remove(receiver);
		}

		public void Unsubscribe<TId>(IMessageable<TId> receiver)
		{
			Assert.IsNotNull(receiver);

			GetDispatcherGroup<TId>().Unsubscribe(receiver);
		}

		public void Send<TId>(object target, TId identifier)
		{
			Send(target, identifier, (object)null);
		}

		public void Send<TId, TArg>(object target, TId identifier, TArg argument)
		{
			Assert.IsNotNull(target);

			GetDispatcherGroup<TId>().Send(target, identifier, argument);

			for (int i = 0; i < receivers.Count; i++)
				receivers[i].OnMessage(identifier);
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
