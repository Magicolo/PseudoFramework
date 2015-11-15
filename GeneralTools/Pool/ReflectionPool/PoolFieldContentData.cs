using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public class PoolFieldContentData : IPoolFieldData
	{
		FieldInfo field;
		List<IPoolFieldData> subFields;

		public PoolFieldContentData(FieldInfo field, List<IPoolFieldData> subFields)
		{
			this.field = field;
			this.subFields = subFields;
		}

		public void SetValue(object instance)
		{
			object value = field.GetValue(instance);

			if (value != null)
			{
				for (int i = 0; i < subFields.Count; i++)
					subFields[i].SetValue(value);
			}
		}
	}
}