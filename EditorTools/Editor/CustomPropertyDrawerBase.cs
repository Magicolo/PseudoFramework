using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;
using System.Reflection;


namespace Pseudo.Internal.Editor
{
	public class CustomPropertyDrawerBase : PropertyDrawer
	{
		static MethodInfo getPropertyDrawerMethod;
		public static MethodInfo GetPropertyDrawerMethod
		{
			get
			{
				if (getPropertyDrawerMethod == null)
					getPropertyDrawerMethod = AssetDatabaseUtility.FindType("ScriptAttributeUtility").GetMethod("GetDrawerTypeForType", ReflectionExtensions.AllFlags);

				return getPropertyDrawerMethod;
			}
		}

		protected UnityEngine.Object target;
		protected UnityEngine.Object[] targets;
		protected SerializedProperty currentProperty;
		protected SerializedObject serializedObject;
		protected Rect currentPosition;
		protected float lineHeight;
		protected bool isArray;
		protected int index;
		protected GUIContent currentLabel = GUIContent.none;
		protected Rect initPosition;
		protected SerializedProperty arrayProperty;

		bool initialized;
		Stack<int> indentStack = new Stack<int>();
		Stack<float> labelWidthStack = new Stack<float>();
		Stack<float> fieldWidthStack = new Stack<float>();

		public virtual void Initialize(SerializedProperty property, GUIContent label)
		{
			initialized = true;
			isArray = typeof(IList).IsAssignableFrom(fieldInfo.FieldType);
			lineHeight = EditorGUIUtility.singleLineHeight;

			if (isArray)
			{
				index = GetIndexFromLabel(label);
				arrayProperty = property.GetParent();
			}
		}

		public virtual void Begin(Rect position, SerializedProperty property, GUIContent label)
		{
			initPosition = position;
			currentPosition = position;
			currentProperty = property;
			currentLabel = label;
			serializedObject = property.serializedObject;
			target = serializedObject.targetObject;
			targets = serializedObject.targetObjects;

			EditorGUI.BeginChangeCheck();
		}

		public virtual void End()
		{
			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(serializedObject.targetObject);

			if (indentStack.Count > 0)
				Debug.LogWarning("BeginIndent groups do not match EndIndent goups.");

			if (labelWidthStack.Count > 0)
				Debug.LogWarning("BeginLabelWidth groups do not match EndLabelWidth goups.");

			if (fieldWidthStack.Count > 0)
				Debug.LogWarning("BeginFieldWidth groups do not match EndFieldWidth goups.");
		}

		public void BeginIndent(int indent)
		{
			indentStack.Push(EditorGUI.indentLevel);
			EditorGUI.indentLevel = indent;
		}

		public void EndIndent()
		{
			EditorGUI.indentLevel = indentStack.Pop();
		}

		public void BeginLabelWidth(float labelWidth)
		{
			labelWidthStack.Push(labelWidth);
			EditorGUIUtility.labelWidth = labelWidth;
		}

		public void EndLabelWidth()
		{
			EditorGUIUtility.labelWidth = labelWidthStack.Pop();
		}

		public void BeginFieldWidth(float fieldWidth)
		{
			fieldWidthStack.Push(fieldWidth);
			EditorGUIUtility.fieldWidth = fieldWidth;
		}

		public void EndFieldWidth()
		{
			EditorGUIUtility.fieldWidth = fieldWidthStack.Pop();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!initialized)
				Initialize(property, label);

			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public void ToggleButton(SerializedProperty boolProperty, GUIContent trueLabel, GUIContent falseLabel)
		{
			Rect indentedPosition = EditorGUI.IndentedRect(currentPosition);
			boolProperty.SetValue(ToggleButton(indentedPosition, boolProperty.GetValue<bool>(), trueLabel, falseLabel));

			currentPosition.y += currentPosition.height + 2;
		}

		public void PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
		{
			currentPosition.height = EditorGUI.GetPropertyHeight(property, label, includeChildren);

			EditorGUI.PropertyField(currentPosition, property, label, includeChildren);

			currentPosition.y += currentPosition.height + 2f;
		}

		public void PropertyField(SerializedProperty property, GUIContent label)
		{
			PropertyField(property, label, true);
		}

		public void PropertyField(SerializedProperty property)
		{
			PropertyField(property, property.displayName.ToGUIContent(), true);
		}

		public PropertyDrawer GetPropertyDrawer(Type propertyAttributeType, params object[] arguments)
		{
			Type propertyDrawerType = GetPropertyDrawerMethod.Invoke(null, new object[] { propertyAttributeType }) as Type;

			if (propertyDrawerType != null)
			{
				PropertyAttribute propertyAttribute = (PropertyAttribute)Activator.CreateInstance(propertyAttributeType, arguments);
				PropertyDrawer propertyDrawer = (PropertyDrawer)Activator.CreateInstance(propertyDrawerType);
				propertyDrawer.SetValueToMember("m_Attribute", propertyAttribute);
				propertyDrawer.SetValueToMember("m_FieldInfo", fieldInfo);

				return propertyDrawer;
			}

			return null;
		}

		public PropertyDrawer GetPropertyDrawer(Attribute propertyAttribute, params object[] arguments)
		{
			return GetPropertyDrawer(propertyAttribute.GetType(), arguments);
		}

		public static bool ToggleButton(Rect position, bool value, GUIContent trueLabel, GUIContent falseLabel)
		{
			Rect labelPosition = new Rect(position.x - EditorGUI.indentLevel * 8f, position.y, position.width, position.height);

			value = EditorGUI.Toggle(position, value, new GUIStyle("button"));

			GUIStyle style = new GUIStyle("label");
			style.clipping = TextClipping.Overflow;
			style.alignment = TextAnchor.MiddleCenter;
			style.contentOffset = new Vector2(-1f, 0f);

			if (value)
				EditorGUI.LabelField(labelPosition, trueLabel, style);
			else
				EditorGUI.LabelField(labelPosition, falseLabel, style);

			return value;
		}

		public static int GetIndexFromLabel(GUIContent label)
		{
			string strIndex = "";

			for (int i = label.text.Length; i-- > 0;)
			{
				if (label.text[i] == 't')
					break;
				else
					strIndex += label.text[i];
			}

			strIndex = strIndex.Reverse();

			int index;
			int.TryParse(strIndex, out index);

			return index;
		}
	}
}

