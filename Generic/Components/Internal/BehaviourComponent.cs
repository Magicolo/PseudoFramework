using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class BehaviourComponent : ComponentBase
	{
		public EntityBehaviour Behaviour;

		public BehaviourComponent(EntityBehaviour behaviour)
		{
			Behaviour = behaviour;
		}
	}
}