using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class EventRaiseNode : NodeBase, IEventRaiseNode
	{
		public IEventDefinition EventDefinition
		{
			get { return eventDefinition; }
		}

		readonly EventDefinition eventDefinition;

		public EventRaiseNode(EventDefinition eventDefinition)
		{
			this.eventDefinition = eventDefinition;
		}

		public ExecutionResults Execute()
		{
			var handlers = eventDefinition.GetHandlers();

			for (int i = 0; i < handlers.Length; i++)
				handlers[i].Execute();

			return ExecutionResults.Continue;
		}

		public IExecutionNode ConnectExecution(IExecutionNode next, int outletIndex)
		{
			return null;
		}

		public IExecutionNode GetNextNode()
		{
			return null;
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return null;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return null;
		}
	}
}
