using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public abstract class FunctionNodeBase<TFunction> : ExecutionNodeBase, IFunctionNode where TFunction : class, IFunctionDefinition
	{
		public IFunctionDefinition FunctionDefinition
		{
			get { return functionDefinition; }
		}

		protected readonly TFunction functionDefinition;

		protected FunctionNodeBase(TFunction functionDefinition)
		{
			this.functionDefinition = functionDefinition;
		}
	}
}
