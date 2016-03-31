using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class EvaluationSettings
	{
		readonly Dictionary<string, IVariable> nameToVariable = new Dictionary<string, IVariable>();

		public Variable<T> GetVariable<T>(string name)
		{
			return (Variable<T>)GetVariable(name);
		}

		public IVariable GetVariable(string name)
		{
			IVariable variable;

			if (!nameToVariable.TryGetValue(name, out variable))
			{
				variable = null;
				nameToVariable[name] = variable;
			}

			return variable;
		}
	}
}
