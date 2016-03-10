using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IEventDefinition
	{
		string Name { get; }

		IEventHandleNode CreateHandleNode();
		IEventRaiseNode CreateRaiseNode();
	}
}
