using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public class SequenceAction : CompositeActionBase
	{
		int currentTaskIndex;

		public SequenceAction(IAction[] actions) : base(actions) { }

		public override void OnBegin()
		{
			base.OnBegin();

			currentTaskIndex = 0;
		}

		public override ActionStates OnExecute(BehaviourTree tree)
		{
			if (currentTaskIndex >= actions.Length)
				return ActionStates.Success;

			var result = actions[currentTaskIndex++].Update(tree);

			if (result == ActionStates.Failure)
				return ActionStates.Failure;
			else
				return ActionStates.Running;
		}
	}
}
