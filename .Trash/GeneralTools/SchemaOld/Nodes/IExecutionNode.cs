using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public enum ExecutionResults
	{
		Continue,
		Break,
		Return
	}

	public interface IExecutionNode : INode
	{
		ExecutionResults Execute();
		IExecutionNode ConnectExecution(IExecutionNode next, int outletIndex);
		IExecutionNode GetNextNode();
	}
}
