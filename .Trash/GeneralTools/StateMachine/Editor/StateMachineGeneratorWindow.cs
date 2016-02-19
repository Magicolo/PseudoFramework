using System;
using System.IO;
using Pseudo.Internal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Pseudo;

namespace Pseudo.Internal.Editor
{
	public class StateMachineGeneratorWindow : CustomWindowBase<StateMachineGeneratorWindow>
	{
		public string Path = "Assets";
		public bool Folder = true;
		public string Machine = "";
		public string Layer = "";
		public string Inherit = "";
		public string SubLayer = "";
		public int CallbackMask = 3;

		List<string> states = new List<string> { "Idle", "" };
		List<string> lockedStates = new List<string>();
		string assetsPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
		float height;
		bool initialized;

		static StateMachineGeneratorWindow instance;

		[MenuItem("Pseudo/State Machine Generator")]
		public static StateMachineGeneratorWindow Create()
		{
			instance = CreateWindow("State Machine Generator", new Vector2(275, 250));
			instance.SetSelectedPath();

			return instance;
		}

		void OnGUI()
		{
			height = 0;

			ShowPath();
			ShowLayer();
			ShowStates();
			ShowGeneratesButton();

			minSize = new Vector2(minSize.x, states.Count * 16 + 230 + height);
			maxSize = new Vector2(maxSize.x, minSize.y);
			initialized = true;
		}

		void OnDestroy()
		{
			Save();
		}

		void ShowPath()
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			GUIStyle style = new GUIStyle("boldLabel");
			EditorGUILayout.LabelField("Path".ToGUIContent(), style, GUILayout.Width(45));
			Path = CustomEditorBase.FolderPathButton(Path, assetsPath);

			GUILayout.Space(5);

			EditorGUILayout.EndHorizontal();

			Folder = EditorGUILayout.Toggle("Create Folder", Folder);

			string folderPath = string.Format("{0}{1}{2}", assetsPath, System.IO.Path.AltDirectorySeparatorChar, Path);
			if (!Directory.Exists(folderPath))
			{
				EditorGUILayout.HelpBox("Selected directory does not exist.", MessageType.Warning);
				height += 44;
			}
			else if (!Folder && Directory.GetFiles(folderPath).Length > 0)
			{
				EditorGUILayout.HelpBox("Selected directory is not empty.", MessageType.Warning);
				height += 44;
			}

			CustomEditorBase.Separator();
		}

		void ShowLayer()
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Layer", new GUIStyle("boldLabel"), GUILayout.Width(100));
			Layer = EditorGUILayout.TextField(Layer);

			EditorGUILayout.EndHorizontal();

			ShowMachine();
			ShowInherit();
			ShowSubLayer();
			ShowCallbacks();

			CustomEditorBase.Separator();
		}

		void ShowMachine()
		{
			List<string> options = new List<string> { "PStateMachine" };
			options.AddRange(StateMachineUtility.MachineFormattedTypeDict.Keys);

			EditorGUILayout.BeginHorizontal();
			EditorGUI.indentLevel += 1;

			EditorGUILayout.LabelField("Machine", GUILayout.Width(100));

			EditorGUI.indentLevel -= 1;

			Machine = CustomEditorBase.Popup(Machine, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));

			EditorGUILayout.EndHorizontal();
		}

		void ShowInherit()
		{
			List<string> options = new List<string> { "PStateLayer" };
			options.AddRange(StateMachineUtility.LayerFormattedTypeDict.Keys);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			EditorGUI.indentLevel += 1;

			EditorGUILayout.LabelField("Inherits From", GUILayout.Width(100));

			EditorGUI.indentLevel -= 1;

			Inherit = CustomEditorBase.Popup(Inherit, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));

			EditorGUILayout.EndHorizontal();

			if (!initialized || EditorGUI.EndChangeCheck())
			{
				lockedStates = new List<string>();

				if (StateMachineUtility.LayerFormattedStateFormattedDict.ContainsKey(Inherit))
				{
					lockedStates = StateMachineUtility.LayerFormattedStateFormattedDict[Inherit];

					for (int i = lockedStates.Count - 1; i >= 0; i--)
					{
						if (!states.Contains(lockedStates[i]))
							AddState(lockedStates[i]);

						states.Move(states.IndexOf(lockedStates[i]), 0);
					}
				}
			}
		}

		void ShowSubLayer()
		{
			List<string> options = new List<string> { " " };
			options.AddRange(StateMachineUtility.LayerFormattedTypeDict.Keys);

			EditorGUILayout.BeginHorizontal();
			EditorGUI.indentLevel += 1;

			EditorGUILayout.LabelField("Sublayer Of", GUILayout.Width(100));

			EditorGUI.indentLevel -= 1;

			SubLayer = CustomEditorBase.Popup(SubLayer, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));

			EditorGUILayout.EndHorizontal();
		}

		void ShowCallbacks()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUI.indentLevel += 1;

			EditorGUILayout.LabelField("Callbacks", GUILayout.Width(100));

			EditorGUI.indentLevel -= 1;

			CallbackMask = EditorGUILayout.MaskField(CallbackMask, StateMachineUtility.FullCallbackNames, GUILayout.MinWidth(150));

			EditorGUILayout.EndHorizontal();
		}

		void ShowStates()
		{
			EditorGUILayout.LabelField("States", new GUIStyle("boldLabel"), GUILayout.Width(100));

			EditorGUI.indentLevel += 1;

			for (int i = 0; i < states.Count; i++)
			{
				bool locked = lockedStates.Contains(states[i]);

				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup(locked);

				states[i] = EditorGUILayout.TextField(states[i]);

				EditorGUI.EndDisabledGroup();

				if (i == 0)
				{
					if (CustomEditorBase.SmallAddButton())
						AddState("");
				}
				else
				{
					EditorGUI.BeginDisabledGroup(locked);

					if (CustomEditorBase.DeleteButton())
					{
						RemoveState(i);
						break;
					}

					EditorGUI.EndDisabledGroup();
				}

				GUILayout.Space(6);

				EditorGUILayout.EndHorizontal();
			}

			if (EditorGUIUtility.editingTextField && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			{
				AddState("");
			}

			EditorGUI.indentLevel -= 1;
			CustomEditorBase.Separator();
		}

		void ShowGeneratesButton()
		{
			if (CustomEditorBase.LargeButton("Generate".ToGUIContent()))
				Generate();

			CustomEditorBase.Separator();
		}

		void AddState(string stateName)
		{
			states.Add(stateName);
		}

		void RemoveState(int index)
		{
			states.RemoveAt(index);

			if (states.Count == 0)
				states.Add("");
		}

		void Generate()
		{
			if (string.IsNullOrEmpty(Path))
			{
				Debug.LogError("Path can not be empty.");
				return;
			}

			if (string.IsNullOrEmpty(Layer))
			{
				Debug.LogError("Layer name can not be empty.");
				return;
			}

			if (Folder)
			{
				string folderPath = assetsPath + Path + System.IO.Path.AltDirectorySeparatorChar + Layer;

				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				Path += System.IO.Path.AltDirectorySeparatorChar + Layer;
				Folder = false;
			}

			GenerateLayer();
			GenerateStates();
			AssetDatabase.Refresh();
			Save();
		}

		void GenerateLayer()
		{
#if !UNITY_WEBPLAYER
			string layerFileName = Layer.Capitalized() + ".cs";
			string layerInherit = "PStateLayer";
			string layerSublayer = "";
			List<string> script = new List<string>();

			if (!string.IsNullOrEmpty(AssetDatabaseUtility.GetAssetPath(layerFileName)))
				return;

			if (StateMachineUtility.LayerFormattedTypeDict.ContainsKey(Inherit))
				layerInherit = StateMachineUtility.LayerFormattedTypeDict[Inherit].GetName();

			if (StateMachineUtility.LayerFormattedTypeDict.ContainsKey(SubLayer))
				layerSublayer = StateMachineUtility.LayerFormattedTypeDict[SubLayer].GetName();

			script.Add("using UnityEngine;");
			script.Add("using System.Collections;");
			script.Add("using System.Collections.Generic;");
			script.Add("using System.Linq;");
			script.Add("using Pseudo;");
			script.Add("");
			script.Add("public class " + Layer + " : " + layerInherit + " {");
			script.Add("	");
			AddMachine(script, Machine);
			AddLayer(script, layerSublayer);
			AddCallbacks(script, CallbackMask);
			script.Add("}");

			File.WriteAllLines(assetsPath + System.IO.Path.AltDirectorySeparatorChar + Path + System.IO.Path.AltDirectorySeparatorChar + layerFileName, script.ToArray());
#endif
		}

		void GenerateStates()
		{
#if !UNITY_WEBPLAYER
			for (int i = 0; i < states.Count; i++)
			{
				string state = states[i];
				string stateFileName = Layer.Capitalized() + state.Capitalized() + ".cs";
				string stateInherit = "PState";
				List<string> script = new List<string>();

				if (string.IsNullOrEmpty(state))
					continue;

				if (!lockedStates.Contains(state) && !string.IsNullOrEmpty(AssetDatabaseUtility.GetAssetPath(stateFileName)))
				{
					Debug.LogError(string.Format("A script named {0} already exists.", stateFileName));
					continue;
				}

				if (StateMachineUtility.LayerFormattedStateFormattedDict.ContainsKey(Inherit) && StateMachineUtility.LayerFormattedStateFormattedDict[Inherit].Contains(state))
					stateInherit = StateMachineUtility.LayerFormattedTypeDict[Inherit].GetName() + state;

				script.Add("using UnityEngine;");
				script.Add("using System;");
				script.Add("using System.Collections;");
				script.Add("using System.Collections.Generic;");
				script.Add("using System.Linq;");
				script.Add("using Pseudo;");
				script.Add("");
				script.Add("public class " + Layer + state + " : " + stateInherit);
				script.Add("{");
				AddMachine(script, Machine);
				AddLayer(script, Layer);
				script.Add("	");
				AddCallbacks(script, CallbackMask);
				script.Add("	");
				script.Add("}");

				File.WriteAllLines(assetsPath + Path + System.IO.Path.AltDirectorySeparatorChar + stateFileName, script.ToArray());
			}
#endif
		}

		void AddMachine(List<string> script, string machineName)
		{
			if (string.IsNullOrEmpty(machineName))
				return;

			script.Add("	new public " + machineName + " Machine { get { return (" + machineName + ")base.Machine; } }");
		}

		void AddLayer(List<string> script, string layerName)
		{
			if (string.IsNullOrEmpty(layerName))
				return;

			script.Add("	new public " + layerName + " Layer { get { return (" + layerName + ")base.Layer; } }");
		}

		void AddCallbacks(List<string> script, int callbacks)
		{
			for (int i = 0; i < StateMachineUtility.FullCallbackNames.Length; i++)
			{
				if ((callbacks & 1 << i) != 0)
				{
					script.Add("	public override void " + StateMachineUtility.CallbackOverrideMethods[i]);
					script.Add("	{");
					script.Add("		base." + StateMachineUtility.CallbackBaseMethods[i] + ";");
					script.Add("		");
					script.Add("	}");
					script.Add("	");
				}
			}

			if (script.Count > 0)
				script.PopLast();
		}

		void SetSelectedPath()
		{
			if (Selection.objects != null && Selection.objects.Length > 0 && Selection.objects[0] is DefaultAsset)
			{
				string selectedPath = AssetDatabase.GetAssetPath(Selection.objects[0]);

				if (Directory.Exists(selectedPath))
				{
					Path = selectedPath;
					Repaint();
				}
			}
		}

		public override void OnSelectionChange()
		{
			base.OnSelectionChange();

			SetSelectedPath();
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts()
		{
			if (instance != null)
				instance.initialized = false;
		}
	}
}
