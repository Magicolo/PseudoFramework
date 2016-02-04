using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(MessageEnum))]
	public class MessageEnumDrawer : CustomPropertyDrawerBase
	{
		static Type[] enumTypes;
		static Enum[] enumValues;
		static GUIContent[] enumValuesPath;

		MessageEnum dynamicEnum;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowEnums();

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			InitializeEnumValues();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			dynamicEnum = property.GetValue<MessageEnum>();

			return EditorGUIUtility.singleLineHeight;
		}

		void ShowEnums()
		{
			int index = Array.IndexOf(enumValues, dynamicEnum.Value);

			EditorGUI.BeginChangeCheck();

			index = EditorGUI.Popup(currentPosition, currentLabel, index, enumValuesPath);

			if (EditorGUI.EndChangeCheck())
				dynamicEnum.Value = enumValues[index];
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void InitializeEnumValues()
		{
			enumTypes = TypeUtility.GetAssignableTypes(typeof(Enum), false);

			var enumValueList = new List<Enum>();
			var enumValuePathList = new List<GUIContent>();

			for (int i = 0; i < enumTypes.Length; i++)
			{
				var enumType = enumTypes[i];

				if (!enumType.IsDefined(typeof(MessageEnumAttribute), true))
					continue;

				var values = Enum.GetValues(enumType);

				for (int j = 0; j < values.Length; j++)
				{
					var value = values.GetValue(j);
					enumValueList.Add((Enum)value);
					enumValuePathList.Add(new GUIContent(enumType.Name + "/" + value));
				}
			}

			enumValues = enumValueList.ToArray();
			enumValuesPath = enumValuePathList.ToArray();
		}
	}
}