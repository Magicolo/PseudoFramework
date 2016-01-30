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
			systemList = new ReorderableList(serializedObject, serializedObject.FindProperty("Systems"))
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

			systemList.displayAdd = installer.Systems.Length < systemTypes.Length;
			systemList.displayRemove = systemList.count > 0;
			systemList.DoLayoutList();

			End(false);
		}

		void ShowSystemHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, systemList.serializedProperty.displayName);
		}

		void ShowSystem(Rect rect, int index, bool isActive, bool isFocused)
		{
			if (index >= systemList.serializedProperty.arraySize)
				return;

			var systemProperty = systemList.serializedProperty.GetArrayElementAtIndex(index);
			var type = Type.GetType(systemProperty.GetValue<string>("TypeName"));
			var active = systemProperty.GetValue<bool>("Active");

			if (type == null)
			{
				systemList.serializedProperty.RemoveAt(index);
				return;
			}

			rect.y += 2f;
			var prefix = string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace + ".";
			EditorGUI.LabelField(rect, prefix + type.Name.Replace("System", ""));

			rect.x += rect.width - 16f;
			rect.width = 16f;

			if (Application.isPlaying && installer.SystemManager != null && installer.SystemManager.HasSystem(type))
			{
				var system = installer.SystemManager.GetSystem(type);
				system.Active = EditorGUI.Toggle(rect, system.Active);
			}
			else
			{
				active = EditorGUI.Toggle(rect, active);
				systemProperty.SetValue("Active", active);
			}
		}

		void OnAddSystemDropdown(Rect buttonRect, ReorderableList list)
		{
			var dropdown = new GenericMenu();

			for (int i = 0; i < systemTypes.Length; i++)
			{
				var type = systemTypes[i];

				if (list.serializedProperty.Find(property => property.GetValue<string>("TypeName") == type.AssemblyQualifiedName) == null)
					dropdown.AddItem(systemTypeNames[i].ToGUIContent(), false, OnSystemSelected, type);
			}

			dropdown.DropDown(buttonRect);
		}

		void OnSystemSelected(object data)
		{
			var type = (Type)data;

			var systemProperty = systemList.serializedProperty.Add(null);
			systemProperty.SetValue("TypeName", type.AssemblyQualifiedName);
			systemProperty.SetValue("Active", true);

			if (installer.SystemManager != null)
				installer.SystemManager.AddSystem(type);
		}

		void OnSystemRemoved(ReorderableList list)
		{
			var typeName = list.serializedProperty.GetArrayElementAtIndex(list.index).GetValue<string>("TypeName");
			var type = Type.GetType(typeName);
			list.serializedProperty.RemoveAt(list.index);

			if (installer.SystemManager != null)
				installer.SystemManager.RemoveSystem(type);
		}

		[InitializeOnLoadMethod, UnityEditor.Callbacks.DidReloadScripts]
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
			systemTypeNames = systemTypes.Convert(type =>
			{
				var prefix = string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace + "/";
				return prefix + type.Name.Replace("System", "");
			});
		}
	}
}