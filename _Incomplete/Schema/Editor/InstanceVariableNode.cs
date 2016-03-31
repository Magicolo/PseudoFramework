using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public class InstanceVariableNode : ReturnNodeBase
	{
		public ReturnNodeBase Caller
		{
			get { return caller; }
		}

		[SerializeField]
		ReturnNodeBase caller;

		public void Initialize(FieldInfo field, Schema schema)
		{
			Initialize(field.Name, field.FieldType, schema);
			caller = null;
		}

		public void Initialize(PropertyInfo property, Schema schema)
		{
			Initialize(property.Name, property.PropertyType, schema);
			caller = null;
		}

		public void SetCaller(ReturnNodeBase caller)
		{
			if (IsCallerValid(caller))
				this.caller = caller;
		}

		public bool IsCallerValid(ReturnNodeBase caller)
		{
			return caller != null && (caller.ReturnType.GetField(Name) != null || caller.ReturnType.GetProperty(Name) != null);
		}

		public override void Write(SchemaWriter writer)
		{
			Caller.Write(writer);
			writer.Append("." + Name);
		}

		public override bool IsValid()
		{
			return IsCallerValid(caller) && base.IsValid();
		}
	}
}
