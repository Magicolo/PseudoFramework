using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using System.IO;

namespace Pseudo
{
	[Serializable]
	public class BinaryExpressionNode : ExpressionNodeBase
	{
		public enum Operators
		{
			Add = 12,
			Substract = 13,
			Multiply = 14,
			Divide = 15,
		}

		public override ExpressionTypes Type
		{
			get { return ExpressionTypes.BinaryExpression; }
		}

		public Operators Operator;
		public IExpressionNode Left;
		public IExpressionNode Right;

		public override IValueNode Evaluate(params IVariable[] variables)
		{
			switch (Operator)
			{
				default:
				case Operators.Add:
					return Left.Evaluate(variables).Add(Right.Evaluate(variables), variables);
				case Operators.Substract:
					return Left.Evaluate(variables).Substract(Right.Evaluate(variables), variables);
				case Operators.Multiply:
					return Left.Evaluate(variables).Multiply(Right.Evaluate(variables), variables);
				case Operators.Divide:
					return Left.Evaluate(variables).Divide(Right.Evaluate(variables), variables);
			}
		}

		protected override void Serialize(StringWriter writer)
		{
			base.Serialize(writer);

			PExpressionUtility.SerializeNode(Left, writer);
			PExpressionUtility.SerializeNode(Right, writer);
		}

		protected override void Deserialize(StringReader reader)
		{
			base.Deserialize(reader);

			Left = PExpressionUtility.DeserializeNode(reader);
			Right = PExpressionUtility.DeserializeNode(reader);
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator {1}, Left{{{2}}}, Right{{{3}}}", Type, Operator, Left, Right);
		}
	}
}
