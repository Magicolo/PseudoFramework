using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public abstract class ActionBase : IAction
	{
		public ActionStates State { get; private set; }

		public virtual void OnBegin() { State = ActionStates.Running; }
		public abstract ActionStates OnExecute(BehaviourTree tree);
		public virtual void OnEnd() { }

		public ActionStates Update(BehaviourTree tree)
		{
			if (State == ActionStates.None)
			{
				if (tree != null)
					tree.Execution.Push(this);

				OnBegin();
			}

			// Check in case an Update is called after the task is done.
			if (State == ActionStates.Running)
				State = OnExecute(tree);

			if (State != ActionStates.Running)
			{
				OnEnd();

				if (tree != null)
				{
					tree.Execution.Pop();

					if (tree.Execution.Count > 0)
						return tree.Execution.Peek().Update(tree);
				}
			}

			return State;
		}
	}
}
