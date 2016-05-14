using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Runtime.Serialization;

namespace Pseudo.Reflection.Internal
{
	public class EmptyValueConstructorWrapper : IConstructorWrapper
	{
		static readonly object[] defaultArguments = new object[0];

		public string Name
		{
			get { return type.Name; }
		}
		public Type Type
		{
			get { return type; }
		}
		public object[] DefaultArguments
		{
			get { return defaultArguments; }
		}

		readonly Type type;

		public EmptyValueConstructorWrapper(Type type)
		{
			this.type = type;
		}

		public object Invoke()
		{
			return Invoke(defaultArguments);
		}

		public object Invoke(params object[] arguments)
		{
			return FormatterServices.GetSafeUninitializedObject(type);
		}
	}
}
