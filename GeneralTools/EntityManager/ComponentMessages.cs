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

	public class EntityMessagesComparer : IEqualityComparer<ComponentMessages>
	{
		public bool Equals(ComponentMessages x, ComponentMessages y)
		{
			return x == y;
		}

		public int GetHashCode(ComponentMessages obj)
		{
			return (int)obj;
		}
	}
}
