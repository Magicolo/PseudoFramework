using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;

public class RecycleOnMessage : ComponentBehaviour, IMessageable
{
	public MessageEnum RecycleMessage;

	bool recycle;

	[PostInject]
	void Initialize()
	{
		PDebug.LogMethod(this);
	}

	void LateUpdate()
	{
		if (recycle)
			Entity.Manager.RecycleEntity(EntityHolder);
	}

	void IMessageable.OnMessage<TId>(TId message)
	{
		recycle |= RecycleMessage.Equals(message);
	}
}
