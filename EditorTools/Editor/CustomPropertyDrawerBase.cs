using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;
using System.Reflection;
using Pseudo.Internal.Unity;

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
			isArray = fieldInfo == null ? false : typeof(IList).IsAssignableFrom(fieldInfo.FieldType);
			lineHeight = EditorGUIUtility.singleLineHeight;

			if (isArray)
			{
				index = GetIndexFromLabel(label);
				arrayProperty = property.GetParent();
			}
		}

		public virtual void Begin(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!initialized)
				Initialize(property, label);

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

		public static object ObjectField(Rect position, object value, GUIContent label)
		{
			if (value == null)
				return null;

			var serializedDummy = DummyUtility.GetSerializedDummy(value);
			EditorGUI.PropertyField(position, serializedDummy, label, true);

			return serializedDummy.GetValue();
		}

		public static void Flags(Rect position, SerializedProperty property, FlagsOption[] options, Action<FlagsOption, SerializedProperty> onSelected, GUIContent label = null)
		{
			label = label ?? property.ToGUIContent();

			EditorGUI.BeginProperty(position, label, property);

			Flags(position, options, onSelected, label, property);

			EditorGUI.EndProperty();
		}

		public static void Flags(Rect position, FlagsOption[] options, Action<FlagsOption, SerializedProperty> onSelected, GUIContent label = null, SerializedProperty property = null)
		{
			int selectedCount = options.Count(option => option.IsSelected);
			bool nothing = selectedCount == 0;
			bool everything = selectedCount == options.Length;
			GUIContent popupName;

			position = EditorGUI.PrefixLabel(position, label);

			if (nothing)
				popupName = "Nothing".ToGUIContent();
			else if (everything)
				popupName = "Everything".ToGUIContent();
			else
			{
				var name = "";

				foreach (var option in options)
				{
					if (option.IsSelected)
					{
						if (string.IsNullOrEmpty(name))
							name = option.Label.text;
						else
							name += " | " + option.Label.text;
					}
				}

				if (selectedCount > 1 && name.GetWidth(EditorStyles.miniFont) > position.width)
					popupName = string.Format("Mixed ({0}) ...", selectedCount).ToGUIContent();
				else
					popupName = name.ToGUIContent();
			}

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			bool pressed = false;
			GenericMenu.MenuFunction2 callback = data => onSelected((FlagsOption)data, property);

			if (GUI.Button(position, GUIContent.none, new GUIStyle()))
			{
				pressed = true;
				var menu = new GenericMenu();

				menu.AddItem(FlagsOption.GetNothing(nothing), callback);
				menu.AddItem(FlagsOption.GetEverything(everything), callback);

				for (int i = 0; i < options.Length; i++)
				{
					var option = options[i];
					menu.AddItem(option.Label, option.IsSelected, callback, option);
				}

				menu.DropDown(position);
			}

			EditorGUI.LabelField(position, popupName, EditorStyles.popup);

			if (pressed)
			{
				GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Native, position);
				GUIUtility.keyboardControl = GUIUtility.GetControlID(FocusType.Native, position);
			}

			EditorGUI.indentLevel = indent;
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

		public void BeginIndent(int indent)
		{
			indentStack.Push(EditorGUI.indentLevel);
			EditorGUI.indentLevel = indent;
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

		public static PropertyDrawer GetPropertyDrawer(Type propertyType, FieldInfo field)
		{
			var propertyDrawerType = ScriptAttributeUtility.GetPropertyDrawerMethod.Invoke(null, new object[] { propertyType }) as Type;

			if (propertyDrawerType != null)
			{
				var propertyDrawer = (PropertyDrawer)Activator.CreateInstance(propertyDrawerType);
				propertyDrawer.SetValueToMember("m_FieldInfo", field);

				return propertyDrawer;
			}

			return null;
		}

		public static PropertyDrawer GetPropertyAttributeDrawer(Type propertyAttributeType, FieldInfo field, params object[] arguments)
		{
			var propertyDrawerType = ScriptAttributeUtility.GetPropertyDrawerMethod.Invoke(null, new object[] { propertyAttributeType }) as Type;

			if (propertyDrawerType != null)
			{
				var propertyAttribute = (PropertyAttribute)Activator.CreateInstance(propertyAttributeType, arguments);
				var propertyDrawer = (PropertyDrawer)Activator.CreateInstance(propertyDrawerType);
				propertyDrawer.SetValueToMember("m_Attribute", propertyAttribute);
				propertyDrawer.SetValueToMember("m_FieldInfo", field);

				return propertyDrawer;
			}

			return null;
		}

		public static PropertyDrawer GetPropertyAttributeDrawer(Attribute propertyAttribute, FieldInfo field, params object[] arguments)
		{
			return GetPropertyAttributeDrawer(propertyAttribute.GetType(), field, arguments);
		}
	}
}

