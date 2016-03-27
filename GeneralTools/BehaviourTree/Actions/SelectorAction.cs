using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public class SelectorAction : CompositeActionBase
	{
		int currentTaskIndex;

		public SelectorAction(IAction[] actions) : base(actions) { }

		public override ActionStates OnExecute(BehaviourTree tree)
		{
			if (currentTaskIndex >= actions.Length)
				return ActionStates.Failure;

			var result = actions[currentTaskIndex++].Update(tree);

			if (result == ActionStates.Success)
				return ActionStates.Success;
			else
				return ActionStates.Running;
		}
	}
}
