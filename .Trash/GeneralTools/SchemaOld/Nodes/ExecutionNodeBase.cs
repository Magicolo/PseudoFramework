using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public abstract class ExecutionNodeBase : NodeBase, IExecutionNode
	{
		IExecutionNode next;

		public virtual ExecutionResults Execute()
		{
			return Next();
		}

		public virtual IExecutionNode ConnectExecution(IExecutionNode next, int outletIndex)
		{
			this.next = next;

			return next;
		}

		public virtual IExecutionNode GetNextNode()
		{
			return next;
		}

		protected virtual ExecutionResults Next()
		{
			var nextNode = GetNextNode();

			if (nextNode != null)
				return nextNode.Execute();

			return ExecutionResults.Continue;
		}
	}
}
