using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;
using Pseudo.References.Internal;
using System.Reflection;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(PropertyReference), true), CanEditMultipleObjects]
	public class PropertyReferenceDrawer : PPropertyDrawer
	{
		IPropertyReference reference;
		PropertyInfo[] properties = new PropertyInfo[0];
		string[] propertyNames = new string[0];
		string[] propertyDisplayNames = new string[0];

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowFoldout();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			reference = property.GetValue<IPropertyReference>();

			return property.isExpanded ? (lineHeight + 2f) * 3f : lineHeight + 2f;
		}

		void ShowFoldout()
		{
			var foldoutLabel = string.Format("{0} | {1}",
				currentLabel.text,
				reference.Target == null ? "null" : reference.Target.name + "." + reference.Target.GetType().Name + (reference.Property == null ? "" : "." + reference.Property.Name));

			currentPosition.height = lineHeight + 2f;
			currentProperty.isExpanded = EditorGUI.Foldout(currentPosition, currentProperty.isExpanded, foldoutLabel);

			if (currentProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				currentPosition.y += currentPosition.height;

				ShowTarget();
				ShowProperties();

				EditorGUI.indentLevel--;
			}
		}

		void ShowTarget()
		{
			var targetProperty = currentProperty.FindPropertyRelative("target");
			var target = targetProperty.GetValue() as GameObject;
			var rect = new Rect(currentPosition)
			{
				width = target == null ? currentPosition.width : currentPosition.width * 0.8f,
				height = EditorGUI.GetPropertyHeight(targetProperty)
			};

			EditorGUI.PropertyField(rect, targetProperty);

			if (target != null)
			{
				var components = target.GetComponents<Component>();
				var componentNames = components.Convert(c => c.GetType().Name);

				EditorGUI.BeginProperty(currentPosition, targetProperty.ToGUIContent(), targetProperty);
				EditorGUI.BeginChangeCheck();

				BeginIndent(0);
				int index = EditorGUI.Popup(new Rect(rect) { x = rect.xMax + 6f, width = currentPosition.width - rect.width - 6f }, "", -1, componentNames);
				EndIndent();

				if (EditorGUI.EndChangeCheck())
					targetProperty.SetValue(components[index]);
				EditorGUI.EndProperty();
			}

			currentPosition.y += currentPosition.height;
		}

		void ShowProperties()
		{
			UpdateProperties();

			var propertyNameProperty = currentProperty.FindPropertyRelative("propertyName");

			EditorGUI.BeginProperty(currentPosition, propertyNameProperty.ToGUIContent(), propertyNameProperty);
			EditorGUI.BeginChangeCheck();

			int index = Array.IndexOf(propertyNames, propertyNameProperty.GetValue<string>());
			index = EditorGUI.Popup(currentPosition, "Property", index, propertyDisplayNames);
			currentPosition.y += currentPosition.height;

			if ((index < 0 && propertyNames.Length > 0) || EditorGUI.EndChangeCheck())
				propertyNameProperty.SetValue(propertyNames[Mathf.Clamp(index, 0, propertyNames.Length - 1)]);
			EditorGUI.EndProperty();
		}

		void UpdateProperties()
		{
			var target = currentProperty.GetValue<UnityEngine.Object>("target");

			if (target == null)
			{
				properties = new PropertyInfo[0];
				propertyNames = new string[0];
				propertyDisplayNames = new string[0];
			}
			else
			{
				properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(p => p.CanRead && p.CanWrite && p.PropertyType == reference.ValueType)
					.ToArray();
				propertyNames = properties.Convert(p => p.Name);
				propertyDisplayNames = properties.Convert(p => string.Format("{0} ({1})", p.Name, p.PropertyType.Name));
			}
		}
	}
}
