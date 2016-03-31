using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public class BehaviourTreeAsset : NodeBase
	{
		public NodeBase Root;
		public List<VariableDefinition> Variables = new List<VariableDefinition>();

		public override IAction CreateAction()
		{
			return new BehaviourTree(Root.CreateAction(), CreateVariables());
		}

		public IVariable[] CreateVariables()
		{
			var variables = new IVariable[Variables.Count];

			for (int i = 0; i < variables.Length; i++)
				variables[i] = Variables[i].CreateVariable();

			return variables;
		}
	}
}
