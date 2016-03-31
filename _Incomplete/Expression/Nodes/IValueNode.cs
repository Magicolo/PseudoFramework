using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IValueNode
	{
		object Value { get; }
		Type Type { get; }
		T GetValue<T>();
		void SetValue<T>(T value);

		IValueNode Add(IValueNode node, params IVariable[] variables);
		IValueNode Substract(IValueNode node, params IVariable[] variables);
		IValueNode Multiply(IValueNode node, params IVariable[] variables);
		IValueNode Divide(IValueNode node, params IVariable[] variables);
	}
}
