using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;
using System.Reflection;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(PType), true), CanEditMultipleObjects]
	public class PTypeDrawer : PPropertyDrawer
	{
		static readonly Type[] types;
		static readonly GUIContent[] typeLabels;

		static PTypeDrawer()
		{
			types = TypeUtility.AllTypes
				.Where(t => (t.Assembly == typeof(PType).Assembly || t.Assembly == typeof(UnityEngine.Object).Assembly || t.Assembly == typeof(object).Assembly) && t.IsPublic)
				.ToArray();

			Array.Sort(types, (t1, t2) =>
			{
				var namespace1 = string.IsNullOrEmpty(t1.Namespace) ? char.MaxValue.ToString() : t1.Namespace;
				var namespace2 = string.IsNullOrEmpty(t2.Namespace) ? char.MaxValue.ToString() : t2.Namespace;

				return (namespace1 + t1.FullName).CompareTo(namespace2 + t2.FullName);
			});

			typeLabels = types.Convert(t =>
			{
				return (t.Assembly.GetName().Name + "/" + (t.Namespace == null ? "" : t.Namespace.Replace('.', '/') + "/") + t.Name).ToGUIContent();
			});
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowType();

			End();
		}

		void ShowType()
		{
			var typeNameProperty = currentProperty.FindPropertyRelative("typeName");
			var type = Type.GetType(typeNameProperty.GetValue<string>());

			int index = Array.IndexOf(types, type);

			EditorGUI.BeginProperty(currentPosition, currentLabel, typeNameProperty);
			EditorGUI.BeginChangeCheck();

			index = EditorGUI.Popup(currentPosition, currentLabel, index, typeLabels);

			if (EditorGUI.EndChangeCheck())
				typeNameProperty.SetValue(types[index].AssemblyQualifiedName);
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			return lineHeight;
		}
	}
}
