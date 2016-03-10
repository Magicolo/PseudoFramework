using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class EventHandleNode : ExecutionNodeBase, IEventHandleNode
	{
		public IEventDefinition EventDefinition
		{
			get { return eventDefinition; }
		}

		readonly EventDefinition eventDefinition;

		public EventHandleNode(EventDefinition eventDefinition)
		{
			this.eventDefinition = eventDefinition;
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
