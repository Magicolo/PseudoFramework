using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class CharacterDieIdle : PState
{
	public float FadeSpeed = 5;

	new public CharacterDie Layer { get { return ((CharacterDie)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		Layer.spriteRenderer.FadeTowards(0f, Time.deltaTime * FadeSpeed, channels: Channels.A);
	}
}
