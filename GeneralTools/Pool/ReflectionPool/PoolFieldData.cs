using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public class PoolFieldData : IPoolFieldData
	{
		FieldInfo field;
		object value;

		public PoolFieldData(FieldInfo field, object value)
		{
			this.field = field;
			this.value = value;
		}

		public void SetValue(object instance)
		{
			field.SetValue(instance, value);
		}
	}
}