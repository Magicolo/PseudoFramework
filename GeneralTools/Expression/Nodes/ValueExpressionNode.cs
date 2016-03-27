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
	public class ValueExpressionNode : ExpressionNodeBase
	{
		public override ExpressionTypes Type
		{
			get { return ExpressionTypes.ValueExpression; }
		}

		public IValueNode Value { get; set; }

		public override IValueNode Evaluate(params IVariable[] variables)
		{
			return Value;
		}

		protected override void Serialize(StringWriter writer)
		{
			base.Serialize(writer);

			PExpressionUtility.SerializeValue(Value, writer);
		}

		protected override void Deserialize(StringReader reader)
		{
			base.Deserialize(reader);

			Value = PExpressionUtility.DeserializeValue(reader);
		}

		public override string ToString()
		{
			return string.Format("{0}, Value{{{1}}}", Type, Value);
		}
	}
}
