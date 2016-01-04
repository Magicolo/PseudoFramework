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
		OnDeselected = 4,
		OnGrow = 5,
		OnMove = 6,
		OnLevelChanged = 7,
		Spawn = 64,
	}
}