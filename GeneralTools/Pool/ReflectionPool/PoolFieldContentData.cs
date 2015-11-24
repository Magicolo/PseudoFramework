using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pseudo.Internal
{
	public class PoolFieldContentData : IPoolFieldData
	{
		FieldInfo field;
		object value;
		bool isDefault;
		List<IPoolFieldData> subFields;

		public PoolFieldContentData(FieldInfo field, object value, bool isDefault, List<IPoolFieldData> subFields)
		{
			this.field = field;
			this.value = value;
			this.isDefault = isDefault;
			this.subFields = subFields;
		}

		public void SetValue(object instance, bool initializeDefault)
		{
			if (!isDefault || initializeDefault)
			{
				if (isDefault)
					field.SetValue(instance, this.value);
				else
				{
					object value = field.GetValue(instance);

					if (value == null)
					{
						value = FormatterServices.GetUninitializedObject(field.FieldType);
						initializeDefault = true;
					}

					for (int i = 0; i < subFields.Count; i++)
						subFields[i].SetValue(value, initializeDefault);
				}
			}
		}
	}
}