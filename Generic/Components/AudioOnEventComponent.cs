using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class AudioOnEventComponent : PMonoBehaviour, IComponent
	{
		[Serializable]
		public struct EventData
		{
			public Events Event;
			public AudioSettingsBase Audio;
		}

		[InitializeContent]
		public EventData[] Events = new EventData[0];
	}
}