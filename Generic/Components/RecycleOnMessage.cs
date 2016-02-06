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

	public EntityBehaviour EntityHolder { get { return cachedEntityHolder.Value; } }
	readonly Lazy<EntityBehaviour> cachedEntityHolder;

	protected RecycleOnMessage()
	{
		cachedEntityHolder = new Lazy<EntityBehaviour>(GetComponent<EntityBehaviour>);
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
