using System;
using System.Reflection;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEditor;
using Pseudo.Internal;
using Pseudo.Reflection;

namespace Pseudo.Editor.Internal
{
	[CustomPropertyDrawer(typeof(PropertyFieldAttribute))]
	public class PropertyFieldDrawer : CustomAttributePropertyDrawerBase
	{
		PropertyDrawer drawerOverride;
		bool hasChanged;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			if (fieldInfo.FieldType.IsArray)
			{
				Debug.LogError(string.Format("{0} must not be applied to arrays or lists.", attribute.GetType().Name));
				return;
			}

			Begin(position, property, label);

			EditorGUI.BeginChangeCheck();

			if (drawerOverride == null)
				EditorGUI.PropertyField(currentPosition, property, label, true);
			else
				drawerOverride.OnGUI(currentPosition, property, label);

			var value = property.GetValue();

			if (hasChanged)
			{
				var propertyPath = property.GetAdjustedPath();
				var propertyPathSplit = propertyPath.Split('.');

				propertyPathSplit[propertyPathSplit.Length - 1] = propertyPathSplit.Last().Replace("_", "").Capitalized();
				propertyPath = propertyPathSplit.Concat(".");
				property.serializedObject.ApplyModifiedProperties();
				Array.ForEach(targets, t => t.SetValueToMemberAtPath(propertyPath, value));
				property.serializedObject.Update();
				hasChanged = false;
			}

			hasChanged = EditorGUI.EndChangeCheck();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var attributeType = ((PropertyFieldAttribute)attribute).attributeType;
			var arguments = ((PropertyFieldAttribute)attribute).arguments;

			if (attributeType == null)
				drawerOverride = GetPropertyDrawer(fieldInfo.FieldType, fieldInfo);
			else
				drawerOverride = GetPropertyAttributeDrawer(attributeType, fieldInfo, arguments);

			if (drawerOverride == null)
				return base.GetPropertyHeight(property, label);
			else
				return drawerOverride.GetPropertyHeight(property, label);
		}
	}
}
