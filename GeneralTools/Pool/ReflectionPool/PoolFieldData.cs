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
		bool isDefault;

		public PoolFieldData(FieldInfo field, object value, bool isDefault)
		{
			this.field = field;
			this.value = value;
			this.isDefault = isDefault;
		}

		public void SetValue(object instance, bool initializeDefault)
		{
			if (!isDefault || initializeDefault)
				field.SetValue(instance, value);
		}
	}
}