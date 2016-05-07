using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Reflection
{
	public interface ITypeWrapper
	{
		Type Type { get; }

		IConstructorWrapper[] Constructors { get; }
		IFieldWrapper[] Fields { get; }
		IPropertyWrapper[] Properties { get; }
		IMethodWrapper[] Methods { get; }
	}
}
