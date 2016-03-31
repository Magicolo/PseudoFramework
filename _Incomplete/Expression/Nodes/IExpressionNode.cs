using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public enum ExpressionTypes
	{
		Null,
		BlockExpression,
		StatementExpression,
		UnaryExpression,
		BinaryExpression,
		TernaryExpression,
		IdentifierExpression,
		ValueExpression,
	}

	public interface IExpressionNode
	{
		ExpressionTypes Type { get; }
		IExpressionNode[] Children { get; set; }

		IValueNode Evaluate();
		IValueNode Evaluate(params IVariable[] variables);
	}
}
