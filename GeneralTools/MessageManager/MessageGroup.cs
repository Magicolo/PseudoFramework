using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public static class MessageGroup<TId>
	{
		static readonly IEqualityComparer<TId> comparer = TypeUtility.GetEqualityComparer<TId>();
		static readonly Dictionary<TId, IMessageDispatcherGroup> idToDispatcherGroup = new Dictionary<TId, IMessageDispatcherGroup>(comparer);

		public static void Send<TArg1, TArg2, TArg3>(object target, TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			if (target == null || (typeof(TId).IsClass && identifier == null))
				return;

			GetDispatcherGroup(identifier).Send(target, argument1, argument2, argument3);
		}

		static IMessageDispatcherGroup GetDispatcherGroup(TId identifier)
		{
			IMessageDispatcherGroup dispatcherGroup;

			if (!idToDispatcherGroup.TryGetValue(identifier, out dispatcherGroup))
			{
				dispatcherGroup = new MessageDispatcherGroup<TId>(identifier);
				idToDispatcherGroup[identifier] = dispatcherGroup;
			}

			return dispatcherGroup;
		}
	}
}
