using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class VariableSetNode<TValue> : ExecutionNodeBase, IVariableSetNode
	{
		public IVariableDefinition VariableDefinition
		{
			get { return variableDefinition; }
		}

		readonly IVariableDefinition<TValue> variableDefinition;
		readonly ValueInlet<TValue> inlet = new ValueInlet<TValue>();

		public VariableSetNode(IVariableDefinition<TValue> variableDefinition)
		{
			this.variableDefinition = variableDefinition;
		}

		public override ExecutionResults Execute()
		{
			variableDefinition.Setter(inlet.PullValue());

			return base.Execute();
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return inlet;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}
}
