using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsDrawer : CustomAttributePropertyDrawerBase
	{
		Type enumType;
		ByteFlag flag;
		Array enumValues;
		string[] enumNames;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			currentPosition.height = 16f;
			EditorGUI.BeginProperty(currentPosition, label, property);

			if (fieldInfo.FieldType.IsNumeric())
				DrawNumericalFlag();
			else if (fieldInfo.FieldType == typeof(ByteFlag))
				DrawByteFlag();

			EditorGUI.EndProperty();
			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);
			enumType = ((EnumFlagsAttribute)attribute).EnumType;
			enumValues = Enum.GetValues(enumType);
			enumNames = Enum.GetNames(enumType);

			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		void DrawNumericalFlag()
		{
			EditorGUI.BeginChangeCheck();

			int value = currentProperty.GetValue<int>();

			value = EditorGUI.MaskField(currentPosition, currentLabel, value, Enum.GetNames(enumType));

			if (EditorGUI.EndChangeCheck())
				currentProperty.SetValue(value);

		}

		void DrawByteFlag()
		{
			if (Enum.GetUnderlyingType(enumType) != typeof(byte))
			{
				EditorGUI.HelpBox(currentPosition, string.Format("{0} should be of type {1}.", enumType.Name, typeof(byte)), MessageType.Error);
				return;
			}

			flag = currentProperty.GetValue<ByteFlag>();
			byte[] selected = flag.ToIndices();
			bool nothing = selected.Length == 0;
			bool everything = selected.Length == enumValues.Length;
			string popupName;

			if (nothing)
				popupName = "Nothing";
			else if (everything)
				popupName = "Everything";
			else if (selected.Length == 1)
				popupName = enumNames[FindEnumValueIndex(selected[0], enumValues)];
			else
				popupName = "Mixed ...";

			currentPosition = EditorGUI.PrefixLabel(currentPosition, currentLabel);

			bool pressed = false;
			if (GUI.Button(currentPosition, GUIContent.none, new GUIStyle()))
			{
				pressed = true;
				var menu = new GenericMenu();

				menu.AddItem("Nothing".ToGUIContent(), nothing, OnEnumSelected, -1);
				menu.AddItem("Everything".ToGUIContent(), everything, OnEnumSelected, -2);

				for (int i = 0; i < enumNames.Length; i++)
				{
					byte value = Convert.ToByte(enumValues.GetValue(i));
					menu.AddItem(enumNames[i].Replace('_', '/').ToGUIContent(), flag[value], OnEnumSelected, (int)value);
				}

				menu.DropDown(currentPosition);
			}

			EditorGUI.LabelField(currentPosition, popupName, EditorStyles.popup);

			if (pressed)
			{
				GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Native, currentPosition);
				GUIUtility.keyboardControl = GUIUtility.GetControlID(FocusType.Native, currentPosition);
			}
		}

		int FindEnumValueIndex(byte value, Array enumValues)
		{
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (Convert.ToByte(enumValues.GetValue(i)) == value)
					return i;
			}

			return -1;
		}

		bool HasAllValues(byte[] values, Array enumValues)
		{
			if (values.Length == 0 && enumValues.Length > 0)
				return false;

			bool hasValues = true;

			for (int i = 0; i < enumValues.Length; i++)
				hasValues &= Array.Exists(values, value => Convert.ToByte(enumValues.GetValue(i)) == value);

			return hasValues;
		}

		byte[] EnumValuesToBytes(Array enumValues)
		{
			byte[] bytes = new byte[enumValues.Length];

			for (int i = 0; i < enumValues.Length; i++)
				bytes[i] = Convert.ToByte(enumValues.GetValue(i));

			return bytes;
		}

		void OnEnumSelected(object data)
		{
			int value = (int)data;

			if (value == -1)
				flag = ByteFlag.Nothing;
			else if (value == -2)
				flag = new ByteFlag(EnumValuesToBytes(enumValues));
			else
				flag[(byte)value] = !flag[(byte)value];

			for (int i = 1; i < 5; i++)
			{
				string flagName = "flag" + i;
				currentProperty.FindPropertyRelative(flagName).longValue = (long)flag.GetValueFromMember<ulong>(flagName);
			}

			currentProperty.serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
}