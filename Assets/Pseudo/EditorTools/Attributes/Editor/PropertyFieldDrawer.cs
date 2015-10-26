using System;
using System.Reflection;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(PropertyFieldAttribute))]
	public class PropertyFieldDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;
			Type attributeType = ((PropertyFieldAttribute)attribute).attributeType;
			object[] arguments = ((PropertyFieldAttribute)attribute).arguments;
			PropertyDrawer drawerOverride = null;

			if (fieldInfo.FieldType.IsArray)
			{
				Debug.LogError(string.Format("{0} should not be applied to arrays or lists.", attribute.GetType().Name));
				return;
			}

			if (attributeType != null)
			{
				drawerOverride = GetPropertyDrawer(attributeType, arguments);
			}

			Begin(position, property, label);

			EditorGUI.BeginChangeCheck();

			if (drawerOverride == null)
				EditorGUI.PropertyField(_currentPosition, property, label, true);
			else
				drawerOverride.OnGUI(_currentPosition, property, label);

			if (EditorGUI.EndChangeCheck())
			{
				string propertyPath = property.GetAdjustedPath();
				string[] propertyPathSplit = propertyPath.Split('.');

				propertyPathSplit[propertyPathSplit.Length - 1] = propertyPathSplit.Last().Replace("_", "").Capitalized();
				propertyPath = propertyPathSplit.Concat(".");
				property.serializedObject.ApplyModifiedProperties();
				Array.ForEach(_targets, t => t.SetValueToMemberAtPath(propertyPath, t.GetValueFromMemberAtPath(propertyPath)));
				property.serializedObject.Update();
			}

			End();
		}
	}
}
