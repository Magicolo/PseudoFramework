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
		public IFieldOrPropertyWrapper[] Fields { get; set; }
		public IFieldOrPropertyWrapper[] Properties { get; set; }
		public IMethodWrapper[] Methods { get; set; }
	}
}
