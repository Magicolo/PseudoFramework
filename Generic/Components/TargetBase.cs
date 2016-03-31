using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.EntityFramework;

namespace Pseudo
{
	public abstract class TargetBase : ComponentBehaviourBase
	{
		public abstract Vector3 Target { get; }
		public abstract bool HasTarget { get; }
	}
}