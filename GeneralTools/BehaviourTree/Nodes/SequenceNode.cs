using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public class SequenceNode : CompositeNodeBase
	{
		public override IAction CreateAction()
		{
			return new SequenceAction(CreateTasks());
		}
	}
}
