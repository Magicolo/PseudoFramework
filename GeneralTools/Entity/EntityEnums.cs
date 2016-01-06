using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public enum EntityMessages : byte
	{
		OnDamage = 0,
		OnDamaged = 1,
		OnCollide = 2,
		OnDie = 3,
		Spawn = 64,
		OnTriggerEnter2D = 128,
		OnTriggerStay2D = 129,
		OnTriggerExit2D = 130,
		OnStartAttacking = 140,
		OnStopAttacking = 141,
	}
}