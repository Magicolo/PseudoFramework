using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Runtime.Serialization;

namespace Pseudo.Reflection.Internal
{
	public class EmptyConstructorWrapper : IConstructorWrapper
	{
		public string Name
		{
			get { return type.Name; }
		}
		public Type Type
		{
			get { return type; }
		}

		readonly Type type;

		public EmptyConstructorWrapper(Type type)
		{
			this.type = type;
		}

		public object Invoke()
		{
			return Invoke(ReflectionUtility.EmptyArguments);
		}

		public object Invoke(params object[] arguments)
		{
			return FormatterServices.GetSafeUninitializedObject(type);
		}
	}
}
