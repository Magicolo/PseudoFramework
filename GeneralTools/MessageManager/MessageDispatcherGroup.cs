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
		readonly Dictionary<TId, MessageDispatcher<TId>> idToDispatcherGroup = new Dictionary<TId, MessageDispatcher<TId>>(PEqualityComparer<TId>.Default);

		public void Send<TArg>(object target, TId identifier, TArg argument)
		{
			GetDispatcher(identifier).Send(target, argument);
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
