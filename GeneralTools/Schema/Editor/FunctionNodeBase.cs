using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public abstract class FunctionNodeBase : ReturnNodeBase
	{
		public ParameterNode[] Parameters
		{
			get { return parameters; }
		}

		[SerializeField]
		ParameterNode[] parameters = new ParameterNode[0];

		public void Initialize(string name, Type returnType, ParameterNode[] parameters, Schema schema)
		{
			Initialize(name, returnType, schema);
			this.parameters = parameters;
		}

		public void SetParameter(ReturnNodeBase parameter, int index)
		{
			if (IsParameterValid(parameter, index))
				parameters[index].SetParameter(parameter);
		}

		public bool IsParameterValid(ReturnNodeBase parameter, int index)
		{
			return index.IsBetween(0, parameters.Length - 1) && parameters[index].IsParameterValid(parameter);
		}

		public override bool IsValid()
		{
			return !string.IsNullOrEmpty(Name) && parameters.All(p => p.IsValid()) && base.IsValid();
		}
	}
}
