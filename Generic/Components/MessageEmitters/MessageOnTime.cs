using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.EntityFramework;

namespace Pseudo
{
	public class MessageOnTime : ComponentBehaviourBase
	{
		public enum TriggerModes
		{
			Once,
			Repeat,
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

		public struct Timer
		{
			public float Counter;
			public bool IsDone;
			public TimeMessage Message;
		}

		public TimeMessage[] Messages = new TimeMessage[0];
		public TimeComponent Time;

		readonly List<Timer> timers = new List<Timer>();

		public override void OnAdded()
		{
			base.OnAdded();

			for (int i = 0; i < Messages.Length; i++)
				timers.Add(new Timer { Message = Messages[i] });
		}

		public override void OnRemoved()
		{
			base.OnRemoved();

			timers.Clear();
		}

		void Update()
		{
			for (int i = 0; i < timers.Count; i++)
			{
				var timer = timers[i];

				if (timer.IsDone)
					continue;
				else if (timer.Counter >= timer.Message.Delay)
				{
					switch (timer.Message.Trigger)
					{
						case TriggerModes.Once:
							timer.IsDone = true;
							break;
						case TriggerModes.Repeat:
							timer.Counter -= timer.Message.Delay;
							break;
					}

					Entity.SendMessage(timer.Message.Message);
				}
				else
					timer.Counter += Time.DeltaTime;

				timers[i] = timer;
			}
		}
	}
}