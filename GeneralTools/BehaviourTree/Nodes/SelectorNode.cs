using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public class SelectorNode : CompositeNodeBase
	{
		public override IAction CreateAction()
		{
			return new SelectorAction(CreateTasks());
		}
	}
}
