using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo
{
	[Serializable]
	public class StatementExpressionNode : ExpressionNodeBase
	{
		public override ExpressionTypes Type
		{
			get { return ExpressionTypes.StatementExpression; }
		}

		public IExpressionNode Expression { get; set; }

		public override IValueNode Evaluate(params IVariable[] variables)
		{
			return Expression.Evaluate(variables);
		}

		protected override void Serialize(StringWriter writer)
		{
			base.Serialize(writer);

			PExpressionUtility.SerializeNode(Expression, writer);
		}

		protected override void Deserialize(StringReader reader)
		{
			base.Deserialize(reader);

			Expression = PExpressionUtility.DeserializeNode(reader);
		}

		public override string ToString()
		{
			return string.Format("{0}, Expression{{{1}}}", Type, Expression);
		}
	}
}
