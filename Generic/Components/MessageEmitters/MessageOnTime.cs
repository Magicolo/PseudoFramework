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
		public struct TimeMessage
		{
			[Min]
			public float Delay;
			public TriggerModes Trigger;
			public EntityMessage Message;
		}

		public TimeMessage[] Messages = new TimeMessage[0];
		public TimeComponent Time;

		readonly List<TimeMessage> scheduledMessages = new List<TimeMessage>();

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