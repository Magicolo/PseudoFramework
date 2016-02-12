using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class LifeTime : ComponentBehaviour, ICopyable<LifeTime>
	{
		[Min]
		public float Duration = 5f;
		public EntityMessage OnDie;
		public TimeComponent Time;

		float counter;

		void Update()
		{
			counter += Time.DeltaTime;

			if (counter >= Duration)
				Entity.SendMessage(OnDie);
		}

		public void Copy(LifeTime reference)
		{
			base.Copy(reference);

			Duration = reference.Duration;
			OnDie = reference.OnDie;
			counter = reference.counter;
		}
	}
}
