using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo
{
	public partial class PEntity
	{
		readonly List<IMessageable> messageables = new List<IMessageable>();
		readonly Dictionary<byte, MessageGroup> messageGroups = new Dictionary<byte, MessageGroup>();

		public void SendMessage(EntityMessages message)
		{
			for (int i = 0; i < messageables.Count; i++)
				messageables[i].OnMessage(message);

			GetMessageGroup(message).SendMessage();
		}

		public void SendMessage(EntityMessages message, object argument)
		{
			for (int i = 0; i < messageables.Count; i++)
				messageables[i].OnMessage(message);

			GetMessageGroup(message).SendMessage(argument);
		}

		public void SendMessage<T>(EntityMessages message, T argument)
		{
			for (int i = 0; i < messageables.Count; i++)
				messageables[i].OnMessage(message);

			GetMessageGroup(message).SendMessage(argument);
		}

		void RegisterComponentToMessageGroups(IComponentOld component)
		{
			if (component is IMessageable)
				messageables.Add((IMessageable)component);

			if (messageGroups.Count > 0)
			{
				var enumerator = messageGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.TryAdd(component);

				enumerator.Dispose();
			}
		}

		void UnregisterComponentFromMessageGroups(IComponentOld component)
		{
			if (component is IMessageable)
				messageables.Remove((IMessageable)component);

			var messageEnumerator = messageGroups.GetEnumerator();

			while (messageEnumerator.MoveNext())
				messageEnumerator.Current.Value.Remove(component);

			messageEnumerator.Dispose();
		}

		MessageGroup GetMessageGroup(EntityMessages message)
		{
			MessageGroup group;

			if (!messageGroups.TryGetValue((byte)message, out group))
			{
				group = CreateMessageGroup(message);
				messageGroups[(byte)message] = group;
			}

			return group;
		}

		MessageGroup CreateMessageGroup(EntityMessages message)
		{
			var group = new MessageGroup(message.ToString());

			for (int i = 0; i < allComponents.Count; i++)
				group.TryAdd(allComponents[i]);

			return group;
		}
	}
}