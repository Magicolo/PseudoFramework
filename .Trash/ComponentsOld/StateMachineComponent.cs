using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class StateMachineComponent : ComponentBehaviour
	{
		public int InitialStateIndex;
		[EntityRequires(typeof(StateComponent), CanBeNull = false), DoNotInitialize]
		public EntityBehaviour[] States;

		public EntityBehaviour CurrentState
		{
			get { return currentState; }
			set { currentState = value; }
		}

		EntityBehaviour currentState;
	}
}