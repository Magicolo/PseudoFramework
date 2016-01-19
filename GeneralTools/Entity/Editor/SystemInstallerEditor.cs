using Pseudo.Internal.Editor;
using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Pseudo;
using System.Collections;

namespace Pseudo.Internal.EntityOld
{
	[CustomEditor(typeof(SystemInstaller)), CanEditMultipleObjects]
	public class SystemInstallerEditor : CustomEditorBase
	{
		static Type[] systemTypes;
		static string[] systemTypeNames;

		SystemInstaller installer;
		ReorderableList systemList;

		public override void OnEnable()
		{
			base.OnEnable();

			installer = (SystemInstaller)target;
			systemList = new ReorderableList(serializedObject, serializedObject.FindProperty("systems"))
			{
				drawHeaderCallback = ShowSystemHeader,
				drawElementCallback = ShowSystem,
				onAddDropdownCallback = OnAddSystemDropdown,
				onRemoveCallback = OnSystemRemoved
			};
		}

		public override void OnInspectorGUI()
		{
			Begin(false);

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
			EditorGUI.EndDisabledGroup();

			systemList.DoLayoutList();

			End(false);
		}

		void ShowSystemHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, systemList.serializedProperty.displayName);
		}

		void ShowSystem(Rect rect, int index, bool isActive, bool isFocused)
		{
			var property = systemList.serializedProperty.GetArrayElementAtIndex(index);
			var typeName = property.GetValue<string>();
			var type = Type.GetType(typeName);
			var displayTypeName = type == null ? "" : type.Name;

			rect.y += 2f;
			EditorGUI.LabelField(rect, displayTypeName);

			rect.x += rect.width - 16f;
			rect.width = 16f;

			if (installer.SystemManager == null || !installer.SystemManager.HasSystem(type))
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.Toggle(rect, true);
				EditorGUI.EndDisabledGroup();
				return;
			}

			var system = installer.SystemManager.GetSystem(type);
			system.Active = EditorGUI.Toggle(rect, system.Active);
		}

		void OnAddSystemDropdown(Rect buttonRect, ReorderableList list)
		{
			var dropdown = new GenericMenu();

			for (int i = 0; i < systemTypes.Length; i++)
			{
				var type = systemTypes[i];

				if (!list.serializedProperty.Contains(type.AssemblyQualifiedName))
					dropdown.AddItem(systemTypeNames[i].ToGUIContent(), false, OnSystemSelected, type);
			}

			dropdown.ShowAsContext();
		}

		void OnSystemSelected(object data)
		{
			var type = (Type)data;

			systemList.serializedProperty.Add(type.AssemblyQualifiedName);

			if (installer.SystemManager != null)
				installer.SystemManager.AddSystem(type);
		}

		void OnSystemRemoved(ReorderableList list)
		{
			var type = Type.GetType(list.serializedProperty.GetValue<string>(list.index));
			list.serializedProperty.RemoveAt(list.index);

			if (installer.SystemManager != null)
				installer.SystemManager.RemoveSystem(type);
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnScriptReload()
		{
			var typeList = new List<Type>(typeof(ISystem).GetAssignableTypes(false));

			for (int i = typeList.Count - 1; i >= 0; i--)
			{
				var type = typeList[i];

				if (type.IsAbstract || type.IsInterface || !type.IsPublic)
					typeList.RemoveAt(i);
			}

			systemTypes = typeList.ToArray();
			systemTypeNames = systemTypes.Convert(type => type.Name.Replace("System", ""));
		}
	}
}