using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class BehaviourComponent : IComponent
	{
		public EntityBehaviour Behaviour;

		public bool Active { get; set; }
		public IEntity Entity { get; set; }
	}
}