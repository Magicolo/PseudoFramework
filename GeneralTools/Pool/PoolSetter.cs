using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolSetter : IPoolSetter
	{
		FieldInfo field;
		object value;

		public PoolSetter(FieldInfo field, object value)
		{
			this.field = field;
			this.value = value;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			field.SetValue(instance, value);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, field.Name, field.FieldType.Name, PDebug.ToString(value));
		}
	}
}