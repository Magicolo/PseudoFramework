using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.Pooling.Internal
{
	public class PoolSetter : IPoolSetter
	{
		readonly FieldInfo field;
		readonly object value;
		readonly IFieldWrapper wrapper;

		public PoolSetter(FieldInfo field, object value)
		{
			this.field = field;
			this.value = value;

			wrapper = field.CreateWrapper();
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			wrapper.Set(ref instance, value);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, field.Name, field.FieldType.Name, PDebug.ToString(value));
		}
	}
}