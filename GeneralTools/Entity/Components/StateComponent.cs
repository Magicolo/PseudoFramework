using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class StateComponent : PMonoBehaviour, IComponent
	{
		public IEntity StateMachine
		{
			get { return stateMachine; }
			set { stateMachine = value; }
		}

		IEntity stateMachine;
	}
}