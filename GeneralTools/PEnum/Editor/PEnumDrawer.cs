using Pseudo.Internal.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(PEnum), true)]
	public class PEnumDrawer : CustomPropertyDrawerBase
	{
		bool isFlag;
		IEnum enumValue;
		Array enumValues;
		string[] enumNames;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			if (isFlag)
				ShowEnumFlag();
			else
				ShowEnum();

			End();
		}

		void ShowEnum()
		{
			int index = Mathf.Max(Array.IndexOf(enumValues, enumValue), 0);

			EditorGUI.BeginChangeCheck();

			index = EditorGUI.Popup(currentPosition, currentProperty.displayName, index, enumNames);

			if (EditorGUI.EndChangeCheck())
				currentProperty.SetValue((IEnum)enumValues.GetValue(index));
		}

		void ShowEnumFlag()
		{
			var enumFlag = (IEnumFlag)enumValue;
			var options = new FlagsOption[enumValues.Length];

			for (int i = 0; i < options.Length; i++)
			{
				var flags = (IEnumFlag)enumValues.GetValue(i);
				options[i] = new FlagsOption(enumNames[i].ToGUIContent(), flags, enumFlag.HasAll(flags));
			}

			Flags(currentPosition, currentProperty, options, OnEnumSelected, currentLabel);
		}

		void OnEnumSelected(FlagsOption option, SerializedProperty property)
		{
			var enumValue = property.GetValue<IEnumFlag>();

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					foreach (var value in enumValues)
						enumValue = enumValue.Add((IEnumFlag)value);
					break;
				case FlagsOption.OptionTypes.Nothing:
					enumValue = enumValue.Remove(ByteFlag.Everything);
					break;
				case FlagsOption.OptionTypes.Custom:
					if (option.IsSelected)
						enumValue = enumValue.Remove((IEnumFlag)option.Value);
					else
						enumValue = enumValue.Add((IEnumFlag)option.Value);
					break;
			}

			property.SetValue(enumValue);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			isFlag = typeof(IEnumFlag).IsAssignableFrom(fieldInfo.FieldType);
			enumValue = property.GetValue<IEnum>();

			var type = typeof(PEnum<,>).MakeGenericType(enumValue.GetType(), enumValue.ValueType);
			enumValues = (Array)type.GetMethod("GetValues").Invoke(null, null);
			enumNames = ((string[])type.GetMethod("GetNames").Invoke(null, null)).Convert(name => name.Replace('_', '/'));

			return 16f;
		}
	}
}
