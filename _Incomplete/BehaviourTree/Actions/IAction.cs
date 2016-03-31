using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public enum ActionStates
	{
		None,
		Success,
		Failure,
		Running,
	}

	public interface IAction
	{
		ActionStates State { get; }

		void OnBegin();
		ActionStates OnExecute(BehaviourTree tree);
		void OnEnd();
		ActionStates Update(BehaviourTree tree);
	}
}
