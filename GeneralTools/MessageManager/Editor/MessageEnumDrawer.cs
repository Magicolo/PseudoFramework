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

		MessageEnum messageEnum;

		static MessageEnumDrawer()
		{
			InitializeEnumValues();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowEnums();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			messageEnum = property.GetValue<MessageEnum>();

			return lineHeight;
		}

		void ShowEnums()
		{
			int index = Array.IndexOf(enumValues, messageEnum.Value);

			EditorGUI.BeginChangeCheck();

			index = EditorGUI.Popup(currentPosition, currentLabel, index, enumValuesPath);

			if (EditorGUI.EndChangeCheck())
			{
				var enumValue = enumValues[index];
				var typeName = enumValue.GetType().AssemblyQualifiedName;
				var value = ((IConvertible)enumValue).ToInt32(null);
				currentProperty.SetValue("value", value);
				currentProperty.SetValue("typeName", typeName);
			}
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
					enumValuePathList.Add(new GUIContent((enumType.Name + '/' + value).Replace('_', '/')));
				}
			}

			enumValues = enumValueList.ToArray();
			enumValuesPath = enumValuePathList.ToArray();
		}
	}
}