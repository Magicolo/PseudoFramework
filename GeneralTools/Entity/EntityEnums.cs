using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public enum EntityGroups : byte
	{
		Player = 0,
		Enemy = 1,
		Weapon = 64,
		Bullet = 65,
		Spawner = 128,
		UI = 192,
		World = 193
	}

	public enum EntityMessages : byte
	{
		OnDamage = 0,
		OnDamaged = 1,
		OnCollide = 2,
		OnDie = 3,
		Spawn = 64,
	}

	public enum EntityMatches
	{
		All,
		Any,
		None,
		Exact
	}
}