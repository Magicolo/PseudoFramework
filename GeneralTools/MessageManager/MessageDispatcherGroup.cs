using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Communication
{
	public class MessageDispatcherGroup<TId>
	{
		static readonly bool isValueType = typeof(TId).IsValueType;

		readonly Dictionary<TId, MessageDispatcher<TId>> idToDispatcherGroup = new Dictionary<TId, MessageDispatcher<TId>>(PEqualityComparer<TId>.Default);
		readonly List<IMessageable<TId>> receivers = new List<IMessageable<TId>>();

		public void Subscribe(IMessageable<TId> receiver)
		{
			receivers.Add(receiver);
		}

		public void Unsubscribe(IMessageable<TId> receiver)
		{
			receivers.Remove(receiver);
		}

		public void Send<TArg>(object target, TId identifier, TArg argument)
		{
			if (!isValueType && identifier == null)
				return;

			if (target is IMessageable)
				((IMessageable)target).OnMessage(identifier);

			if (target is IMessageable<TId>)
				((IMessageable<TId>)target).OnMessage(identifier);

			GetDispatcher(identifier).Send(target, argument);

			for (int i = 0; i < receivers.Count; i++)
				receivers[i].OnMessage(identifier);
		}

		MessageDispatcher<TId> GetDispatcher(TId identifier)
		{
			MessageDispatcher<TId> dispatcherGroup;

			if (!idToDispatcherGroup.TryGetValue(identifier, out dispatcherGroup))
			{
				dispatcherGroup = new MessageDispatcher<TId>(identifier);
				idToDispatcherGroup[identifier] = dispatcherGroup;
			}

			return dispatcherGroup;
		}
	}
}
