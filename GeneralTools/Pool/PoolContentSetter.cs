﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolContentSetter : IPoolSetter
	{
		readonly FieldInfo field;
		readonly List<IPoolSetter> setters;

		public PoolContentSetter(FieldInfo field, List<IPoolSetter> setters)
		{
			this.field = field;
			this.setters = setters;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			var value = field.GetValue(instance);

			if (value != null)
			{
				for (int i = 0; i < setters.Count; i++)
					setters[i].SetValue(value);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, field.Name, field.FieldType.Name, PDebug.ToString(setters));
		}
	}
}