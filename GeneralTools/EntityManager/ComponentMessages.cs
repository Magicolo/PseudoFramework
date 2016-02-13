using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[MessageEnum]
	public enum ComponentMessages
	{
		OnEntityActivated,
		OnEntityDeactivated,
		OnAdded,
		OnRemoved,
	}
}
