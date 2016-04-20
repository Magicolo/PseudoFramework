using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Runtime.Serialization;

namespace Pseudo.Reflection.Internal
{
	public class EmptyConstructorWrapper : ConstructorWrapperBase
	{
		readonly Type type;

		public EmptyConstructorWrapper(Type type)
		{
			this.type = type;
		}

		public override object Construct(params object[] arguments)
		{
			return FormatterServices.GetSafeUninitializedObject(type);
		}
	}
}
