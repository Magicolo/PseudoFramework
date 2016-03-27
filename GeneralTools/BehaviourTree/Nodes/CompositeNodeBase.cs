using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.BehaviourTree
{
	public abstract class CompositeNodeBase : NodeBase
	{
		public List<NodeBase> Children = new List<NodeBase>();

		public IAction[] CreateTasks()
		{
			var tasks = new IAction[Children.Count];

			for (int i = 0; i < Children.Count; i++)
				tasks[i] = Children[i].CreateAction();

			return tasks;
		}
	}
}
