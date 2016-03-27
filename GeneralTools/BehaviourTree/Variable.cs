using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class Variable<T> : IVariable
	{
		public string Name { get; private set; }
		public Type Type { get { return typeof(T); } }
		public T Value { get; set; }

		object IVariable.Value
		{
			get { return Value; }
			set { Value = (T)value; }
		}

		public Variable(string name)
		{
			Name = name;
		}
	}
}
