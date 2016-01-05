using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class Trigger2DRelayer : PMonoBehaviour
{
	readonly CachedValue<PEntity> cachedEntity;
	public PEntity CachedEntity { get { return cachedEntity.Value; } }

	public Trigger2DRelayer()
	{
		cachedEntity = new CachedValue<PEntity>(GetComponent<PEntity>);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		CachedEntity.SendMessage(EntityMessages.OnTriggerEnter2D, collision);
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		CachedEntity.SendMessage(EntityMessages.OnTriggerStay2D, collision);
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		CachedEntity.SendMessage(EntityMessages.OnTriggerExit2D, collision);
	}
}
