using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;
using ICSharpCode.NRefactory;
using System.IO;
using ICSharpCode.NRefactory.Ast;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(PExpression), true), CanEditMultipleObjects]
	public class PExpressionDrawer : PPropertyDrawer
	{
		PExpression pExpression;
		SerializedProperty textProperty;
		float height;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			EditorGUI.BeginProperty(position, label, textProperty);
			EditorGUI.BeginChangeCheck();

			position = EditorGUI.PrefixLabel(position, label);
			textProperty.stringValue = EditorGUI.TextField(position, textProperty.stringValue);

			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
				pExpression.Children = CreateNodes(textProperty.stringValue);
				property.serializedObject.Update();
			}
			EditorGUI.EndProperty();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			pExpression = property.GetValue<PExpression>();
			textProperty = property.FindPropertyRelative("Text");
			int lineCount = textProperty.stringValue.Count(c => c == '\n' || c == '\r') + 1;
			height = Mathf.Max(lineCount * (lineHeight - 3f) + 4f, lineHeight);

			return height;
		}

		IExpressionNode[] CreateNodes(string code)
		{
			var block = NRefactoryUtility.ParseBlock(code);

			return CreateNodes(block.Children);
		}

		IExpressionNode[] CreateNodes(List<INode> nodes)
		{
			return nodes.Convert(n => CreateNode(n));
		}

		IExpressionNode CreateNode(INode node)
		{
			IExpressionNode expressionNode = null;

			if (node is ExpressionStatement)
			{
				var statementNode = (ExpressionStatement)node;
				expressionNode = new StatementExpressionNode
				{
					Expression = CreateNode(statementNode.Expression)
				};
			}
			else if (node is BinaryOperatorExpression)
			{
				var binaryNode = ((BinaryOperatorExpression)node);
				expressionNode = new BinaryExpressionNode
				{
					Operator = (BinaryExpressionNode.Operators)binaryNode.Op,
					Left = CreateNode(binaryNode.Left),
					Right = CreateNode(binaryNode.Right),
				};
			}
			else if (node is PrimitiveExpression)
			{
				var primitiveNode = (PrimitiveExpression)node;
				var valueType = primitiveNode.Value == null ? typeof(object) : primitiveNode.Value.GetType();
				var value = (IValueNode)Activator.CreateInstance(typeof(ValueNode<>).MakeGenericType(valueType), primitiveNode.Value);
				expressionNode = new ValueExpressionNode
				{
					Value = value,
				};
			}

			return expressionNode;
		}
	}
}
