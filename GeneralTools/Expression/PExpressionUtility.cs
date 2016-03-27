using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo
{
	public static class PExpressionUtility
	{
		public static IExpressionNode CreateNode(ExpressionTypes type)
		{
			IExpressionNode node = null;

			switch (type)
			{
				case ExpressionTypes.BlockExpression:
					node = new PExpression();
					break;
				case ExpressionTypes.StatementExpression:
					node = new StatementExpressionNode();
					break;
				case ExpressionTypes.UnaryExpression:
					break;
				case ExpressionTypes.BinaryExpression:
					node = new BinaryExpressionNode();
					break;
				case ExpressionTypes.TernaryExpression:
					break;
				case ExpressionTypes.IdentifierExpression:
					break;
				case ExpressionTypes.ValueExpression:
					node = new ValueExpressionNode();
					break;
			}

			return node;
		}

		public static IValueNode CreateValue(object value)
		{
			var valueType = value == null ? typeof(object) : value.GetType();
			var valueNode = (IValueNode)Activator.CreateInstance(typeof(ValueNode<>).MakeGenericType(valueType), value);

			return valueNode;
		}

		public static void SerializeNode(IExpressionNode node, StringWriter writer)
		{
			if (node == null)
				writer.WriteLine((int)ExpressionTypes.Null);
			else
			{
				writer.WriteLine((int)node.Type);
				writer.WriteLine(JsonUtility.ToJson(node));
			}
		}

		public static void SerializeValue(IValueNode value, StringWriter writer)
		{
			if (value == null || value.Value == null)
				writer.WriteLine();
			else
			{
				writer.WriteLine(value.Type.FullName);
				writer.WriteLine(Convert.ToString(value.Value));
			}
		}

		public static IExpressionNode DeserializeNode(StringReader reader)
		{
			var type = (ExpressionTypes)int.Parse(reader.ReadLine());

			if (type == ExpressionTypes.Null)
				return null;
			else
			{
				var node = CreateNode(type);
				JsonUtility.FromJsonOverwrite(reader.ReadLine(), node);

				return node;
			}
		}

		public static IValueNode DeserializeValue(StringReader reader)
		{
			var typeName = reader.ReadLine();

			if (string.IsNullOrEmpty(typeName))
				return null;
			else
			{
				var type = TypeUtility.GetType(typeName);
				var value = Convert.ChangeType(reader.ReadLine(), type);

				return CreateValue(value);
			}
		}
	}
}
