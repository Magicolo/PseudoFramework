using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DStickIdle : State
{
	new public Character3DStick Layer { get { return ((Character3DStick)base.Layer); } }

	public override void TriggerEnter(Collider collision)
	{
		base.TriggerEnter(collision);

		if (Layer.StickyLayer == collision.gameObject.layer)
		{
			SwitchState<Character3DStickSticking>().TriggerEnter(collision);
			return;
		}
	}
}
