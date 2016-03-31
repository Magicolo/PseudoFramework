using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public abstract class NodeBase
	{
		public abstract IAction CreateAction();
	}
}
