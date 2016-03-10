using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Schema
{
	public class VariableDefinition<TValue> : IVariableDefinition<TValue>
	{
		public string Name { get; private set; }
		public FunctionOut<TValue> Getter { get; private set; }
		public FunctionIn<TValue> Setter { get; private set; }

		public VariableDefinition(object instance, PropertyInfo property)
		{
			Name = property.Name;

			var getMethod = property.GetGetMethod(false);

			if (getMethod == null)
				Getter = delegate { return default(TValue); };
			else
				Getter = (FunctionOut<TValue>)Delegate.CreateDelegate(typeof(FunctionOut<TValue>), instance, getMethod);

			var setMethod = property.GetSetMethod(false);

			if (setMethod == null)
				Setter = delegate { };
			else
				Setter = (FunctionIn<TValue>)Delegate.CreateDelegate(typeof(FunctionIn<TValue>), instance, setMethod);
		}

		public VariableDefinition(string name, FunctionOut<TValue> getter, FunctionIn<TValue> setter)
		{
			Name = name;
			Getter = getter;
			Setter = setter;
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
