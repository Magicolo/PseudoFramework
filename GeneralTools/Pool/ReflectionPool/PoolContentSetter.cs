using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public class PoolContentSetter : IPoolSetter
	{
		List<IPoolSetter> setters;
		FieldInfo field;

		public PoolContentSetter(List<IPoolSetter> setters, FieldInfo field)
		{
			this.setters = setters;
			this.field = field;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			if (instance.GetType() != field.DeclaringType)
				throw new TypeMismatchException(string.Format("Instance type {0} doesn't match {1}.", instance.GetType().Name, field.DeclaringType.Name));

			var value = field.GetValue(instance);

			if (value != null)
			{
				for (int i = 0; i < setters.Count; i++)
					setters[i].SetValue(value);
			}
		}
	}
}