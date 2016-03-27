using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo
{
	public abstract class ExpressionNodeBase : IExpressionNode, ISerializationCallbackReceiver
	{
		static readonly IVariable[] emptyVariables = new IVariable[0];

		public abstract ExpressionTypes Type { get; }
		public IExpressionNode[] Children { get; set; }

		[SerializeField]
		string data;

		protected virtual void Serialize(StringWriter writer)
		{
			Children = Children ?? new IExpressionNode[0];
			writer.WriteLine(Children.Length);

			for (int i = 0; i < Children.Length; i++)
				PExpressionUtility.SerializeNode(Children[i], writer);
		}

		protected virtual void Deserialize(StringReader reader)
		{
			Children = new IExpressionNode[int.Parse(reader.ReadLine())];

			for (int i = 0; i < Children.Length; i++)
				Children[i] = PExpressionUtility.DeserializeNode(reader);
		}

		public IValueNode Evaluate()
		{
			return Evaluate(emptyVariables);
		}

		public abstract IValueNode Evaluate(params IVariable[] variables);

		public override string ToString()
		{
			return string.Format("{0}", Type);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			using (var writer = new StringWriter())
			{
				Serialize(writer);
				data = writer.ToString();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(data))
				return;

			using (var reader = new StringReader(data))
				Deserialize(reader);
		}
	}
}
