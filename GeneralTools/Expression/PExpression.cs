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
	public class PExpression : ExpressionNodeBase
	{
		public string Text;

		public override ExpressionTypes Type
		{
			get { return ExpressionTypes.BlockExpression; }
		}

		public override IValueNode Evaluate(params IVariable[] variables)
		{
			IValueNode result = null;

			for (int i = 0; i < Children.Length; i++)
				result = Children[i].Evaluate(variables);

			return result;
		}

		public override string ToString()
		{
			return string.Format("{0}, Text {1}, Children{{{2}}}", Type, Text, string.Join(", ", Children.Convert(c => c.ToString())));
		}
	}
}
