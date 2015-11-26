using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public class PoolSetter : IPoolSetter
	{
		object value;
		FieldInfo field;

		public PoolSetter(object value, FieldInfo field)
		{
			this.value = value;
			this.field = field;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			if (instance.GetType() != field.DeclaringType)
				throw new TypeMismatchException(string.Format("Instance type {0} doesn't match {1}.", instance.GetType().Name, field.DeclaringType.Name));

			field.SetValue(instance, value);
		}
	}
}