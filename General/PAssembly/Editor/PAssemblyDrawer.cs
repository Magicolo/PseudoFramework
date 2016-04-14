using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;
using System.Reflection;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(PAssembly), true), CanEditMultipleObjects]
	public class PAssemblyDrawer : PPropertyDrawer
	{
		static readonly Assembly[] assemblies;
		static readonly GUIContent[] assemblyLabels;

		static PAssemblyDrawer()
		{
			assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Array.Sort(assemblies, (a1, a2) => a1.GetName().Name.CompareTo(a2.GetName().Name));
			assemblyLabels = assemblies.Convert(a => a.GetName().Name.ToGUIContent());
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowAssembly();

			End();
		}

		void ShowAssembly()
		{
			var assemblyNameProperty = currentProperty.FindPropertyRelative("assemblyName");
			var assembly = TypeUtility.GetAssembly(assemblyNameProperty.GetValue<string>());
			int index = Array.IndexOf(assemblies, assembly);

			EditorGUI.BeginProperty(currentPosition, currentLabel, assemblyNameProperty);
			EditorGUI.BeginChangeCheck();

			index = EditorGUI.Popup(currentPosition, currentLabel, index, assemblyLabels);

			if (EditorGUI.EndChangeCheck())
				assemblyNameProperty.SetValue(assemblies[index].FullName);
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			return lineHeight;
		}
	}
}
