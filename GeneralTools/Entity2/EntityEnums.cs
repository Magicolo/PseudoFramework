<<<<<<< HEAD:GeneralTools/Entity/EntityEnums.cs
﻿using UnityEngine;
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
		Building = 2,
		Weapon = 64,
		Bullet = 65,
		Spawner = 128,
		UI = 192,
		World = 193,
		Manager = 224
	}

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

	public enum EntityMatches
	{
		All,
		Any,
		None,
		Exact
	}
=======
﻿using UnityEngine;
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
	}
>>>>>>> Entity2:GeneralTools/Entity2/EntityEnums.cs
}