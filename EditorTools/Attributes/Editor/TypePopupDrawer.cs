using UnityEngine;
using UnityEditor;
using System;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(TypePopupAttribute))]
	public class TypePopupDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			var types = ((TypePopupAttribute)attribute).Types;
			var typeNames = types.Convert(type => type.Name);
			var typeName = property.GetValue<string>();
			var typeIndex = Array.IndexOf(types, Type.GetType(typeName));

			EditorGUI.BeginChangeCheck();
			BeginIndent(0);

			typeIndex = EditorGUI.Popup(currentPosition, typeIndex, typeNames);

			EndIndent();
			if (EditorGUI.EndChangeCheck())
				property.SetValue(types[typeIndex].AssemblyQualifiedName);

			End();
		}
	}
}