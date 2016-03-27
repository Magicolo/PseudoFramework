using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public class BehaviourTree : ActionBase
	{
		public readonly Stack<IAction> Execution = new Stack<IAction>();

		readonly IAction root;
		readonly Dictionary<string, IVariable> nameToVariable = new Dictionary<string, IVariable>();

		public BehaviourTree(IAction root, IVariable[] variables)
		{
			this.root = root;

			for (int i = 0; i < variables.Length; i++)
			{
				var variable = variables[i];
				nameToVariable[variable.Name] = variable;
			}
		}

		public override ActionStates OnExecute(BehaviourTree tree)
		{
			if (Execution.Count == 0)
				return root.Update(this);
			else
				return Execution.Peek().Update(this);
		}

		public Variable<T> GetVariable<T>(string name)
		{
			IVariable variable;
			nameToVariable.TryGetValue(name, out variable);

			return (Variable<T>)variable;
		}
	}
}
