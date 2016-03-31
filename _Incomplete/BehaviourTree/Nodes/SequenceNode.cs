using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public class SequenceNode : CompositeNodeBase
	{
		public override IAction CreateAction()
		{
			return new SequenceAction(CreateTasks());
		}
	}
}
