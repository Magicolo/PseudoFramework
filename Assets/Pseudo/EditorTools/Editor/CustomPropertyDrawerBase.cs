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
		protected UnityEngine.Object target;
		protected UnityEngine.Object[] targets;
		protected SerializedProperty currentProperty;
		protected SerializedObject serializedObject;
		protected Rect currentPosition;
		protected float lineHeight;
		protected bool isArray;
		protected int index;
		protected float scrollbarThreshold;
		protected GUIContent currentLabel = GUIContent.none;
		protected Rect initPosition;
		protected SerializedProperty arrayProperty;

		static MethodInfo _getPropertyDrawerMethod;
		public static MethodInfo GetPropertyDrawerMethod
		{
			get
			{
				if (_getPropertyDrawerMethod == null)
					_getPropertyDrawerMethod = AssetDatabaseUtility.FindType("ScriptAttributeUtility").GetMethod("GetDrawerTypeForType", ReflectionExtensions.AllFlags);

				return _getPropertyDrawerMethod;
			}
		}

		bool _initialized;

		public virtual void Initialize(SerializedProperty property, GUIContent label)
		{
			_initialized = true;
			isArray = typeof(IList).IsAssignableFrom(fieldInfo.FieldType);
			lineHeight = EditorGUIUtility.singleLineHeight;

			if (isArray)
			{
				index = AttributeUtility.GetIndexFromLabel(label);
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
			scrollbarThreshold = Screen.width - position.width > 19 ? 298 : 313;

			EditorGUI.BeginChangeCheck();
		}

		public virtual void End()
		{
			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(serializedObject.targetObject);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!_initialized)
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
				PropertyAttribute propertyAttribute = Activator.CreateInstance(propertyAttributeType, arguments) as PropertyAttribute;
				PropertyDrawer propertyDrawer = Activator.CreateInstance(propertyDrawerType) as PropertyDrawer;
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
	}
}

