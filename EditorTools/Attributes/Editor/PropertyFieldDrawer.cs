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
		PropertyDrawer drawerOverride;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			if (fieldInfo.FieldType.IsArray)
			{
				Debug.LogError(string.Format("{0} should not be applied to arrays or lists.", attribute.GetType().Name));
				return;
			}

			Begin(position, property, label);

			EditorGUI.BeginChangeCheck();

			if (drawerOverride == null)
				EditorGUI.PropertyField(currentPosition, property, label, true);
			else
				drawerOverride.OnGUI(currentPosition, property, label);

			if (EditorGUI.EndChangeCheck())
			{
				string propertyPath = property.GetAdjustedPath();
				string[] propertyPathSplit = propertyPath.Split('.');

				propertyPathSplit[propertyPathSplit.Length - 1] = propertyPathSplit.Last().Replace("_", "").Capitalized();
				propertyPath = propertyPathSplit.Concat(".");
				property.serializedObject.ApplyModifiedProperties();
				Array.ForEach(targets, t => t.SetValueToMemberAtPath(propertyPath, t.GetValueFromMemberAtPath(propertyPath)));
				property.serializedObject.Update();
			}

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Type attributeType = ((PropertyFieldAttribute)attribute).attributeType;
			object[] arguments = ((PropertyFieldAttribute)attribute).arguments;

			if (attributeType != null)
				drawerOverride = GetPropertyDrawer(attributeType, arguments);

			if (drawerOverride == null)
				return base.GetPropertyHeight(property, label);
			else
				return drawerOverride.GetPropertyHeight(property, label);
		}
	}
}
