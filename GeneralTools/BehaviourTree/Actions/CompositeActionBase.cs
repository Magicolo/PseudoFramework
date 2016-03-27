using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public abstract class CompositeActionBase : ActionBase
	{
		protected readonly IAction[] actions;

		protected CompositeActionBase(IAction[] actions)
		{
			this.actions = actions;
		}
	}
}
