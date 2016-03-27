using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class VariableNode<TValue> : ValueNodeBase, IVariableNode
	{
		public override Type Type
		{
			get { return variable.Type; }
		}

		public override IValueNode Result
		{
			get { return result ?? (result = new ValueNode<TValue>()); }
		}

		public override object Value
		{
			get { return variable.Value; }
		}

		Variable<TValue> variable { get; set; }
		ValueNode<TValue> result;

		public override T GetValue<T>()
		{
			return Caster<TValue, T>.Default.Cast(variable.Value);
		}

		public override void SetValue<T>(T value)
		{
			variable.Value = Caster<T, TValue>.Default.Cast(value);
		}

		public void SetVariable(IVariable variable)
		{
			this.variable = (Variable<TValue>)variable;
		}
	}
}
