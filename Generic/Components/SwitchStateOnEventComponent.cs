using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	[RequireComponent(typeof(StateComponent))]
	public class SwitchStateOnEventComponent : PMonoBehaviour, IComponent
	{
		[Serializable]
		public struct EventData
		{
			public Events Event;
			public int State;
		}

		[InitializeContent]
		public EventData[] Events = new EventData[0];
	}
}