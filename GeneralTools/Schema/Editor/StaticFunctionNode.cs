using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public class StaticFunctionNode : FunctionNodeBase
	{
		public Type Caller
		{
			get { return caller; }
		}

		[SerializeField]
		PType caller;

		public void Initialize(MethodInfo method, Schema schema)
		{
			Initialize(method.Name, method.ReturnType, method.GetParameters().Convert(p => schema.CreateParameter(p)), schema);
			caller = method.DeclaringType;
		}

		public override void Write(SchemaWriter writer)
		{
			writer.Append(Caller.FullName);
			writer.Append("." + Name + "(");

			for (int i = 0; i < Parameters.Length; i++)
			{
				Parameters[i].Write(writer);

				if (i < Parameters.Length - 1)
					writer.Append(", ");
			}

			writer.Append(")");
		}

		public override bool IsValid()
		{
			return Caller != null && base.IsValid();
		}
	}
}
