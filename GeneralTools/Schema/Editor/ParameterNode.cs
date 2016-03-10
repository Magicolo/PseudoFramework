using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public class ParameterNode : ReturnNodeBase
	{
		public ReturnNodeBase Parameter
		{
			get { return parameter; }
		}

		[SerializeField]
		ReturnNodeBase parameter;

		public void Initialize(ParameterInfo parameter, Schema schema)
		{
			Initialize(parameter.Name, parameter.ParameterType, schema);
			parameter = null;
		}

		public void SetParameter(ReturnNodeBase parameter)
		{
			if (IsParameterValid(parameter))
				this.parameter = parameter;
		}

		public bool IsParameterValid(ReturnNodeBase parameter)
		{
			return parameter == null || parameter.ReturnType.Is(ReturnType);
		}

		public override void Write(SchemaWriter writer)
		{
			if (parameter == null)
				writer.Append(string.Format("default({0})", ReturnType.FullName));
			else
				parameter.Write(writer);
		}

		public override bool IsValid()
		{
			return IsParameterValid(parameter) && base.IsValid();
		}
	}
}
