using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public class InstanceFunctionNode : FunctionNodeBase
	{
		public ReturnNodeBase Caller
		{
			get { return caller; }
		}

		[SerializeField]
		ReturnNodeBase caller;

		public void Initialize(MethodInfo method, Schema schema)
		{
			Initialize(method.Name, method.ReturnType, method.GetParameters().Convert(p => schema.CreateParameter(p)), schema);
		}

		public override void Write(SchemaWriter writer)
		{
			caller.Write(writer);
			writer.Append("." + Name + "(");

			for (int i = 0; i < Parameters.Length; i++)
			{
				Parameters[i].Write(writer);

				if (i < Parameters.Length - 1)
					writer.Append(", ");
			}

			writer.Append(")");
		}

		public void SetCaller(ReturnNodeBase caller)
		{
			if (IsCallerValid(caller))
				this.caller = caller;
		}

		public bool IsCallerValid(ReturnNodeBase caller)
		{
			return caller != null && caller.ReturnType.GetMethod(Name) != null;
		}

		public override bool IsValid()
		{
			return IsCallerValid(caller) && base.IsValid();
		}
	}
}
