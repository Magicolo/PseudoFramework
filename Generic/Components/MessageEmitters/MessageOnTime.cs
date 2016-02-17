using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class MessageOnTime : ComponentBehaviour
	{
		public enum TriggerModes
		{
			Once,
			Continuous
		}

		[Serializable]
		public struct TimedMessage
		{
			public EntityMessage Message;
			[Min]
			public float Delay;
			public TriggerModes Trigger;
		}

		public TimedMessage[] Messages = new TimedMessage[0];
		public TimeComponent Time;

		readonly List<TimedMessage> scheduledMessages = new List<TimedMessage>();

		[Message(ComponentMessages.OnAdded)]
		void OnAdded()
		{
			scheduledMessages.AddRange(Messages);
		}

		[Message(ComponentMessages.OnRemoved)]
		void OnRemoved()
		{
			scheduledMessages.Clear();
		}

		void Update()
		{
			for (int i = 0; i < scheduledMessages.Count; i++)
			{
				var message = scheduledMessages[i];
				message.Delay -= Time.DeltaTime;
				scheduledMessages[i] = message;

				if (message.Delay < 0f)
				{
					switch (message.Trigger)
					{
						case TriggerModes.Once:
							scheduledMessages.RemoveAt(i--);
							break;
						case TriggerModes.Continuous:
							break;
					}

					Entity.SendMessage(message.Message);
				}
			}
		}
	}
}