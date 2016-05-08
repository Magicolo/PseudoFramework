using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Input;

namespace Pseudo
{
	public class MessageOnInput : ComponentBehaviourBase
	{
		public enum TriggerModes
		{
			Pressed,
			Down,
			Up
		}

		[Serializable]
		public struct InputMessage
		{
			public Players Player;
			public string Action;
			public TriggerModes Trigger;
			public EntityMessage Message;
		}

		public InputMessage[] Messages = new InputMessage[0];

		[Inject]
		IInputManager inputManager = null;

		void Update()
		{
			for (int i = 0; i < Messages.Length; i++)
			{
				var message = Messages[i];
				bool triggered = false;

				switch (message.Trigger)
				{
					case TriggerModes.Pressed:
						triggered = inputManager.GetKey(message.Player, message.Action);
						break;
					case TriggerModes.Down:
						triggered = inputManager.GetKeyDown(message.Player, message.Action);
						break;
					case TriggerModes.Up:
						triggered = inputManager.GetKeyUp(message.Player, message.Action);
						break;
				}

				if (triggered)
					Entity.SendMessage(message.Message);
			}
		}
	}
}