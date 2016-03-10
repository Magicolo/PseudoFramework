using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Schema
{
	public class VariableGetNode<TValue> : NodeBase, IVariableGetNode
	{
		public IVariableDefinition VariableDefinition
		{
			get { return variableDefinition; }
		}

		readonly IVariableDefinition<TValue> variableDefinition;
		readonly PullValueOutlet<TValue> outlet;

		public VariableGetNode(IVariableDefinition<TValue> variableDefinition)
		{
			this.variableDefinition = variableDefinition;

			outlet = new PullValueOutlet<TValue>(variableDefinition.Getter);
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}
}
