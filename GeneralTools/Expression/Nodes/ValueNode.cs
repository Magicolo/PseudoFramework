using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ValueNode<TValue> : ValueNodeBase
	{
		public override IValueNode Result
		{
			get { return result ?? (result = new ValueNode<TValue>()); }
		}

		public override Type Type
		{
			get { return typeof(TValue); }
		}

		public override object Value
		{
			get { return value; }
		}

		TValue value;
		ValueNode<TValue> result;

		public ValueNode() { }

		public ValueNode(TValue value)
		{
			this.value = value;
		}

		public override T GetValue<T>()
		{
			return Caster<TValue, T>.Default.Cast(value);
		}

		public override void SetValue<T>(T value)
		{
			this.value = Caster<T, TValue>.Default.Cast(value);
		}
	}
}
