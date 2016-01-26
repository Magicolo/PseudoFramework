using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	[RequireComponent(typeof(TimeComponent))]
	public class LifeTimeComponent : ComponentBehaviour
	{
		public Events DieEvent;
		public float LifeTime = 2f;
		public float LifeCounter
		{
			get { return lifeCounter; }
			set { lifeCounter = value; }
		}
		public bool IsAlive
		{
			get { return LifeCounter <= LifeTime; }
		}

		float lifeCounter;
	}
}