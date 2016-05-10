using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pseudo.Internal;
using Pseudo.Reflection;

namespace Pseudo.Editor.Internal
{
	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsDrawer : CustomAttributePropertyDrawerBase
	{
		Type enumType;
		Array enumValues;
		string[] enumNames;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			currentPosition.height = 16f;

			if (fieldInfo.FieldType.IsEnum)
				DrawEnumFlag();
			if (fieldInfo.FieldType.IsNumerical())
				DrawNumericalFlag();
			else if (fieldInfo.FieldType.Is<ByteFlag>())
				DrawByteFlag();

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

			serializedObject.ApplyModifiedProperties();
			var value = (Enum)target.GetValueFromFieldAtPath(currentProperty.GetAdjustedPath());

			value = EditorGUI.EnumMaskField(currentPosition, currentLabel, value);

			if (EditorGUI.EndChangeCheck())
			{
				currentProperty.intValue = Convert.ToInt32(value);
				serializedObject.ApplyModifiedProperties();
			}
		}

		void DrawNumericalFlag()
		{
			var enumValue = currentProperty.GetValue<int>();
			var options = new FlagsOption[enumValues.Length];

			for (int i = 0; i < options.Length; i++)
			{
				var name = enumNames[i].Replace('_', '/').ToGUIContent();
				var value = Convert.ToInt32(enumValues.GetValue(i));
				options[i] = new FlagsOption(name, value, (enumValue & value) == value);
			}

			Flags(currentPosition, options, OnEnumFlagSelected, currentLabel, currentProperty);

		}

		void DrawByteFlag()
		{
			if (Enum.GetUnderlyingType(enumType) != typeof(byte))
			{
				EditorGUI.HelpBox(currentPosition, string.Format("Underlying type of {0} must be of type {1}.", enumType.Name, typeof(byte)), MessageType.Error);
				return;
			}

			var byteFlag = currentProperty.GetValue<ByteFlag>();
			var options = new FlagsOption[enumValues.Length];

			for (int i = 0; i < options.Length; i++)
			{
				var name = enumNames[i].Replace('_', '/').ToGUIContent();
				var value = Convert.ToByte(enumValues.GetValue(i));
				options[i] = new FlagsOption(name, value, byteFlag[value]);
			}

			Flags(currentPosition, options, OnByteFlagSelected, currentLabel, currentProperty);
		}

		void OnEnumFlagSelected(FlagsOption option, SerializedProperty property)
		{
			var enumValue = property.GetValue<int>();

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					enumValue = -1;
					break;
				case FlagsOption.OptionTypes.Nothing:
					enumValue = 0;
					break;
				case FlagsOption.OptionTypes.Custom:
					var value = (int)option.Value;

					if ((enumValue & value) == value)
						enumValue &= ~value;
					else
						enumValue |= value;
					break;
			}

			property.SetValue(enumValue);
		}

		void OnByteFlagSelected(FlagsOption option, SerializedProperty property)
		{
			var byteFlag = property.GetValue<ByteFlag>();

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					byteFlag = new ByteFlag(enumValues.Convert((object v) => Convert.ToByte(v)));
					break;
				case FlagsOption.OptionTypes.Nothing:
					byteFlag = ByteFlag.Nothing;
					break;
				case FlagsOption.OptionTypes.Custom:
					byte value = (byte)option.Value;
					byteFlag = byteFlag[value] ? byteFlag - value : byteFlag + value;
					break;
			}

			property.SetValue(byteFlag);
		}
	}
}