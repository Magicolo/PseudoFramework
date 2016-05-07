using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Reflection.Internal
{
	public class TypeWrapper : ITypeWrapper
	{
		public Type Type { get; set; }

		public IConstructorWrapper[] Constructors { get; set; }
		public IFieldWrapper[] Fields { get; set; }
		public IPropertyWrapper[] Properties { get; set; }
		public IMethodWrapper[] Methods { get; set; }
	}
}
