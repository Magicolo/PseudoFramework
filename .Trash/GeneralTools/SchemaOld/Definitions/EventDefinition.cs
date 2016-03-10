using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class EventDefinition : IEventDefinition
	{
		public string Name
		{
			get { return name; }
		}

		readonly string name;
		IEventHandleNode[] handlers;

		public EventDefinition(string name)
		{
			this.name = name;
		}

		public IEventHandleNode CreateHandleNode()
		{
			var handler = new EventHandleNode(this);
			handlers = handlers.Joined(handler);

			return handler;
		}

		public IEventRaiseNode CreateRaiseNode()
		{
			return new EventRaiseNode(this);
		}

		public IEventHandleNode[] GetHandlers()
		{
			return handlers;
		}
	}
}
