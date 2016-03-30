﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class AudioOnMessage : ComponentBehaviour, IMessageable
	{
		public enum SpatializationModes
		{
			None,
			Static,
			Dynamic
		}

		[Serializable]
		public struct AudioAction
		{
			public AudioSettingsBase Audio;
			public SpatializationModes Spatialization;
			public MessageEnum Message;
		}

		public AudioAction[] Actions = new AudioAction[0];

		[Inject]
		readonly IAudioManager audioManager = null;

		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < Actions.Length; i++)
			{
				var data = Actions[i];

				if (data.Message.Equals(data))
				{
					switch (data.Spatialization)
					{
						default:
							audioManager.CreateItem(data.Audio).Play();
							break;
						case SpatializationModes.Static:
							audioManager.CreateItem(data.Audio, Entity.GetTransform().position).Play();
							break;
						case SpatializationModes.Dynamic:
							audioManager.CreateItem(data.Audio, Entity.GetTransform()).Play();
							break;
					}
				}
			}
		}
	}
}