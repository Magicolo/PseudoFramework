﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Zenject;

namespace Pseudo
{
	public class AudioActionOnMessage : ComponentBehaviour, IMessageable, ICopyable<AudioActionOnMessage>
	{
		public enum SpatializationModes
		{
			None,
			Static,
			Dynamic
		}

		[Serializable]
		public struct AudioData
		{
			public MessageEnum Message;
			public AudioSettingsBase Audio;
			public SpatializationModes Spatialization;
		}

		[InitializeContent]
		public List<AudioData> Actions = new List<AudioData>();

		[Inject]
		readonly IAudioManager audioManager = null;

		void PlayAudio(AudioData data)
		{
			IAudioItem item;

			switch (data.Spatialization)
			{
				default:
					item = audioManager.CreateItem(data.Audio);
					break;
				case SpatializationModes.Static:
					item = audioManager.CreateItem(data.Audio, Entity.GetTransform().position);
					break;
				case SpatializationModes.Dynamic:
					item = audioManager.CreateItem(data.Audio, Entity.GetTransform());
					break;
			}

			item.Play();
		}

		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < Actions.Count; i++)
			{
				var data = Actions[i];

				if (data.Message.Equals(data))
					PlayAudio(data);
			}
		}

		public void Copy(AudioActionOnMessage reference)
		{
			CopyUtility.CopyTo(reference.Actions, ref Actions);
		}
	}
}