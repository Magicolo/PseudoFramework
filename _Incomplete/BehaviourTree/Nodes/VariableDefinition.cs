using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.BehaviourTree.Internal
{
	public class VariableDefinition : ScriptableObject
	{
		public string Name;
		public PType Type;

		public IVariable CreateVariable()
		{
			return (IVariable)Activator.CreateInstance(typeof(Variable<>).MakeGenericType(Type), Name);
		}
	}
}
