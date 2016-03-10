using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class ConditionNode : NodeBase, IExecutionNode
	{
		ValueInlet<bool>[] inlets = { new ValueInlet<bool>() };
		IExecutionNode[] nextNodes = new IExecutionNode[2];

		public IExecutionNode ConnectExecution(IExecutionNode next, int outletIndex = 0)
		{
			nextNodes[outletIndex] = next;

			return next;
		}

		public ExecutionResults Execute()
		{
			var next = GetNextNode();

			if (next != null)
				return next.Execute();

			return ExecutionResults.Continue;
		}

		public IExecutionNode GetNextNode()
		{
			for (int i = 0; i < inlets.Length; i++)
			{
				if (inlets[i].PullValue())
					return nextNodes[i];
			}

			return nextNodes[nextNodes.Length - 1];
		}

		public void SetConditionCount(int count)
		{
			if (inlets.Length == count || count <= 1)
				return;
			else if (inlets.Length < count)
			{
				var missing = new ValueInlet<bool>[count - inlets.Length];
				missing.Fill(i => new ValueInlet<bool>());
				inlets = inlets.Joined(missing);
			}
			else if (inlets.Length > count)
				Array.Resize(ref inlets, count);

			Array.Resize(ref nextNodes, count + 1);
		}

		public override ValueInlet GetValueInlet(int index)
		{
			throw new NotImplementedException();
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}
}
