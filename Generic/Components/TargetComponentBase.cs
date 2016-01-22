using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public abstract class TargetComponentBase : ComponentBehaviour
	{
		public abstract Vector3 Target { get; }
		public abstract bool HasTarget { get; }
	}
}