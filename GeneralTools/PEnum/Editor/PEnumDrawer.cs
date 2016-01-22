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
				currentProperty.SetValue("value", ((IEnum)enumValues.GetValue(index)).Value);
		}

		void ShowEnumFlag()
		{
			var enumFlagValue = currentProperty.GetValue<IEnumFlag>();
			var options = new FlagsOption[enumValues.Length];

			for (int i = 0; i < options.Length; i++)
			{
				var flags = (IEnumFlag)enumValues.GetValue(i);
				options[i] = new FlagsOption(enumNames[i].ToGUIContent(), flags, enumFlagValue.HasAll(flags));
			}

			Flags(currentPosition, currentProperty, options, OnEnumSelected, currentLabel);
		}

		void OnEnumSelected(FlagsOption option, SerializedProperty property)
		{
			var flagsProperty = property.FindPropertyRelative("value");
			var flags = property.GetValue<IEnumFlag>().Value;

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					foreach (IEnumFlag value in enumValues)
						flags = flags.Add(value.Value);
					break;
				case FlagsOption.OptionTypes.Nothing:
					flags = ByteFlag.Nothing;
					break;
				case FlagsOption.OptionTypes.Custom:
					if (option.IsSelected)
						flags = flags.Remove(((IEnumFlag)option.Value).Value);
					else
						flags = flags.Add(((IEnumFlag)option.Value).Value);
					break;
			}

			for (int i = 1; i <= 8; i++)
			{
				var flagName = "f" + i;
				flagsProperty.FindPropertyRelative(flagName).intValue = flags.GetValueFromMember<int>(flagName);
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			enumValue = property.GetValue<IEnum>();
			enumValues = enumValue.GetValues();
			enumNames = enumValue.GetNames().Convert(name => name.Replace('_', '/'));
			isFlag = enumValue is IEnumFlag;

			return 16f;
		}
	}
}
