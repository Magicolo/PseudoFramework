using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class AudioOnEventComponent : ComponentBehaviour
	{
		public enum SpatializationModes
		{
			None,
			Static,
			Dynamic
		}

		[Serializable]
		public struct EventData
		{
			public Events Event;
			public UIEvents UIEvent;
			public BehaviourEvents BehaviourEvent;
			public PhysicsEvents PhysicsEvent;
			public AudioSettingsBase Audio;
			public SpatializationModes Spatialization;
		}

		[InitializeContent]
		public EventData[] Events = new EventData[0];
	}
}