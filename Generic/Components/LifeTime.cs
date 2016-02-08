using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[RequireComponent(typeof(TimeComponent))]
public class LifeTime : ComponentBehaviour, ICopyable<LifeTime>
{
	[Min]
	public float Duration = 5f;
	public EntityMessage OnDie;

	float counter;

	void Update()
	{
		counter += Entity.GetTime().DeltaTime;

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
