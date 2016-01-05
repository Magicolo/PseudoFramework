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
		ByteFlag flags;
		Array enumValues;
		string[] enumNames;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			currentPosition.height = 16f;
			EditorGUI.BeginProperty(currentPosition, label, property);

			if (typeof(Enum).IsAssignableFrom(fieldInfo.FieldType))
				DrawEnumFlag();
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
			enumType = ((EnumFlagsAttribute)attribute).EnumType ?? fieldInfo.FieldType;
			enumValues = Enum.GetValues(enumType);
			enumNames = Enum.GetNames(enumType);

			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		void DrawEnumFlag()
		{
			if (!enumType.IsDefined(typeof(FlagsAttribute), true))
			{
				EditorGUI.HelpBox(currentPosition, string.Format("{0} must be defined as a Flag.", enumType.Name), MessageType.Error);
				return;
			}

			EditorGUI.BeginChangeCheck();

			string path = currentProperty.GetAdjustedPath();
			var value = target.GetValueFromMemberAtPath<Enum>(path);

			value = EditorGUI.EnumMaskField(currentPosition, currentLabel, value);

			if (EditorGUI.EndChangeCheck())
			{
				currentProperty.intValue = Convert.ToInt32(value);
				serializedObject.ApplyModifiedProperties();
			}
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
				EditorGUI.HelpBox(currentPosition, string.Format("Underlying type of {0} must be of type {1}.", enumType.Name, typeof(byte)), MessageType.Error);
				return;
			}

			flags = currentProperty.GetValue<ByteFlag>();
			var options = new FlagsOption[enumValues.Length];

			for (int i = 0; i < options.Length; i++)
			{
				var name = enumNames[i].Replace('_', '/').ToGUIContent();
				var value = Convert.ToByte(enumValues.GetValue(i));
				options[i] = new FlagsOption(name, value, flags[value]);
			}

			Flags(currentPosition, options, OnEnumSelected, currentLabel, currentProperty);
		}

		byte[] EnumValuesToBytes(Array enumValues)
		{
			byte[] bytes = new byte[enumValues.Length];

			for (int i = 0; i < enumValues.Length; i++)
				bytes[i] = Convert.ToByte(enumValues.GetValue(i));

			return bytes;
		}

		void OnEnumSelected(FlagsOption option, SerializedProperty property)
		{
			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					flags = new ByteFlag(EnumValuesToBytes(enumValues));
					break;
				case FlagsOption.OptionTypes.Nothing:
					flags = ByteFlag.Nothing;
					break;
				case FlagsOption.OptionTypes.Custom:
					flags[(byte)option.Value] = !flags[(byte)option.Value];
					break;
			}

			for (int i = 1; i <= 8; i++)
			{
				var flagName = "f" + i;
				property.FindPropertyRelative(flagName).intValue = flags.GetValueFromMember<int>(flagName);
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
}