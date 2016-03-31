using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public class StaticVariableNode : ReturnNodeBase
	{
		public Type Caller
		{
			get { return caller; }
		}

		[SerializeField]
		PType caller;

		public void Initialize(FieldInfo field, Schema schema)
		{
			Initialize(field.Name, field.FieldType, schema);
			caller = field.DeclaringType;
		}

		public void Initialize(PropertyInfo property, Schema schema)
		{
			Initialize(property.Name, property.PropertyType, schema);
			caller = property.DeclaringType;
		}

		public override void Write(SchemaWriter writer)
		{
			writer.Append(Caller.FullName);
			writer.Append("." + Name);
		}

		public override bool IsValid()
		{
			return Caller != null && base.IsValid();
		}
	}
}
