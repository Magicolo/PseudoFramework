using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal;

namespace Pseudo
{
	public interface IState : IStateMachineCallable, IStateMachineStateable, IStateMachineSwitchable
	{
		IStateLayer Layer { get; }
		IStateMachine Machine { get; }
	}
}

