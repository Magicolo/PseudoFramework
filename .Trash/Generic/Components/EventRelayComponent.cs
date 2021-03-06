﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class EventRelayComponent : ComponentBehaviour
	{
		[Serializable]
		public struct EventData
		{
			public Events Event;
			public EntityBehaviour Relay;
		}

		[InitializeContent]
		public EventData[] Events = new EventData[0];
	}
}