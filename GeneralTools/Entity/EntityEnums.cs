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
		Player,
		Enemy,
	}

	public enum EntityMatches
	{
		All,
		Any,
		None,
		Exact
	}
}