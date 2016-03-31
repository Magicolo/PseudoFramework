using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public abstract class ValueNodeBase : IValueNode
	{
		public abstract IValueNode Result { get; }
		public abstract Type Type { get; }
		public abstract object Value { get; }

		public IValueNode Add(IValueNode node, params IVariable[] variables)
		{
			Result.SetValue(GetValue<double>() + node.GetValue<double>());

			return Result;
		}

		public IValueNode Substract(IValueNode node, params IVariable[] variables)
		{
			Result.SetValue(GetValue<double>() - node.GetValue<double>());

			return Result;
		}

		public IValueNode Multiply(IValueNode node, params IVariable[] variables)
		{
			Result.SetValue(GetValue<double>() * node.GetValue<double>());

			return Result;
		}

		public IValueNode Divide(IValueNode node, params IVariable[] variables)
		{
			Result.SetValue(GetValue<double>() / node.GetValue<double>());

			return Result;
		}

		public abstract T GetValue<T>();

		public abstract void SetValue<T>(T value);

		public override string ToString()
		{
			return string.Format("Type {0}, Value {1}", Type, Value);
		}

	}
}
