﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[RequireComponent(typeof(TimeComponent))]
public class LifeTime : ComponentBehaviour
{
	public float Duration = 5f;
	public MessageEnum OnDieMessage;

	float counter;

	void Update()
	{
		counter += Entity.GetTime().DeltaTime;

		if (counter >= Duration)
			Entity.SendMessage(OnDieMessage.Value);
	}
}
