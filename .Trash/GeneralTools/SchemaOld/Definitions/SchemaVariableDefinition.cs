using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Schema
{
	public class SchemaVariableDefinition<TValue> : IVariableDefinition<TValue>
	{
		public string Name { get; private set; }
		public FunctionOut<TValue> Getter { get; private set; }
		public FunctionIn<TValue> Setter { get; private set; }

		TValue value;

		public SchemaVariableDefinition(string name)
		{
			Name = name;
			Getter = () => value;
			Setter = value => this.value = value;
		}

		public IVariableGetNode CreateGetNode()
		{
			return new VariableGetNode<TValue>(this);
		}

		public IVariableSetNode CreateSetNode()
		{
			return new VariableSetNode<TValue>(this);
		}
	}
}
