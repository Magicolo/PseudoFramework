using System;
using System.IO;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(PStateMachine), true), CanEditMultipleObjects]
	public class PStateMachineEditor : CustomEditorBase
	{
		PStateMachine machine;
		GameObject machineObject;
		PStateLayer[] existingLayers;
		PState[] existingStates;
		StateMachineCaller[] existingCallers;

		Type selectedLayerType;

		public override void OnEnable()
		{
			base.OnEnable();

			machine = (PStateMachine)target;
			machineObject = machine.CachedGameObject;
			machine.SetScriptIcon(HelperFunctions.LoadAssetInFolder<Texture2D>("statemachine.png", "StateMachine"));

			HideMachineComponents();

			if (machine.GetComponents<PStateMachine>().Length > 1)
			{
				Debug.LogError("There can be only one StateMachine per GameObject.");
				machine.Destroy();
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();

			PStateMachineUtility.CleanUpAll();
			PStateMachineUtility.CleanUp(machine, machineObject);
		}

		public override void OnInspectorGUI()
		{
			Begin();

			HideMachineComponents();
			ShowAddLayer();
			ShowMachineFields();
			ShowLayers(serializedObject.FindProperty("stateReferences"));
			ReorderComponents();
			CleanUp();

			End();
		}

		void HideMachineComponents()
		{
			existingLayers = machineObject.GetComponents<PStateLayer>();
			existingStates = machineObject.GetComponents<PState>();
			existingCallers = machine.GetComponents<StateMachineCaller>();

			Array.ForEach(existingLayers, layer => layer.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(existingStates, state => state.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(existingCallers, caller => caller.hideFlags = HideFlags.HideInInspector);
		}

		void ShowMachineFields()
		{
			SerializedProperty iterator = serializedObject.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);

			if (!iterator.NextVisible(false))
				return;

			while (true)
			{
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.PropertyField(iterator, true);

				if (EditorGUI.EndChangeCheck())
					iterator.SetValueToSelected();

				if (!iterator.NextVisible(false))
					break;
			}

			Separator();
		}

		void ShowAddLayer()
		{
			GUIStyle style = new GUIStyle("popup");
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;

			EditorGUILayout.BeginHorizontal();

			if (!Application.isPlaying)
			{
				List<Type> layerTypes = new List<Type>();
				List<string> layerTypesName = new List<string> { "Add Layer" };

				foreach (KeyValuePair<Type, List<Type>> pair in PStateMachineUtility.LayerTypeStateTypeDict)
				{
					PropertyInfo machineProperty = pair.Key.GetProperty("Machine", ReflectionExtensions.AllFlags);
					PropertyInfo layerProperty = pair.Key.GetProperty("Layer", ReflectionExtensions.AllFlags);
					bool machinePropertyValid = machineProperty.PropertyType == typeof(IStateMachine) || machineProperty.PropertyType.IsInstanceOfType(machine);
					bool layerPropertyValid = layerProperty.PropertyType == typeof(IStateLayer) && Array.TrueForAll(existingLayers, existingLayer => existingLayer.GetType() != pair.Key);

					if (machinePropertyValid && layerPropertyValid)
					{
						layerTypes.Add(pair.Key);
						layerTypesName.Add(PStateMachineUtility.LayerTypeFormattedDict[pair.Key]);
					}
				}

				if (Selection.gameObjects.Length <= 1)
				{
					int layerTypeIndex = EditorGUILayout.Popup(layerTypes.IndexOf(selectedLayerType) + 1, layerTypesName.ToArray(), style) - 1;
					selectedLayerType = layerTypeIndex == -1 ? null : layerTypes[Mathf.Clamp(layerTypeIndex, 0, layerTypes.Count - 1)];

					if (selectedLayerType != null)
					{
						PStateMachineUtility.AddLayer(machine, selectedLayerType, machine);
						selectedLayerType = null;
					}
				}
				else
					GUI.Box(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), "Multi-editing is not supported.", new GUIStyle(EditorStyles.helpBox));
			}
			else
			{

				EditorGUI.BeginDisabledGroup(Application.isPlaying);

				EditorGUILayout.Popup(0, new[] { "Add Layer" }, style);

				EditorGUI.EndDisabledGroup();
			}

			if (GUILayout.Button("Generate".ToGUIContent()))
			{
				PStateMachineGeneratorWindow window = PStateMachineGeneratorWindow.Create();

				window.Machine = PStateMachineUtility.FormatMachine(machine);
				window.Layer = "";
				window.Inherit = "PStateLayer";
				window.SubLayer = "";
			}

			EditorGUILayout.EndHorizontal();
			Separator();
		}

		void ShowLayers(SerializedProperty layersProperty)
		{
			CleanUpLayers(layersProperty);

			for (int i = 0; i < layersProperty.arraySize; i++)
				ShowLayer(layersProperty, i);

			if (layersProperty.arraySize > 0)
				Separator();
		}

		void ShowLayer(SerializedProperty layersProperty, int index)
		{
			SerializedProperty layerProperty = layersProperty.GetArrayElementAtIndex(index);
			PStateLayer layer = layerProperty.GetValue<PStateLayer>();
			SerializedObject layerSerialized = new SerializedObject(layer);
			SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			SerializedProperty activeStatesProperty = layerSerialized.FindProperty("activeStateReferences");

			PStateMachineUtility.AddMissingStates(machine, layer);

			BeginBox(GetBoxStyle(layer));

			Rect rect = EditorGUILayout.BeginHorizontal();

			ShowLayerOptions(layer, rect);

			if (DeleteFoldOut(layersProperty, index, GetLayerLabel(layer), GetLayerStyle(layer)))
			{
				PStateMachineUtility.RemoveLayer(machine, layer);
				return;
			}

			EditorGUILayout.EndHorizontal();

			CleanUpStates(statesProperty);

			if (layerProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				List<string> currentLayerStatesName = new List<string> { "Empty" };
				IState[] states = statesProperty.GetValues<IState>();

				for (int i = 0; i < states.Length; i++)
				{
					IState state = states[i];
					currentLayerStatesName.Add(state is IStateLayer ? state.GetTypeName() : PStateMachineUtility.FormatState(state.GetType(), layer));
				}

				for (int i = 0; i < activeStatesProperty.arraySize; i++)
				{
					SerializedProperty activeStateProperty = activeStatesProperty.GetArrayElementAtIndex(i);
					UnityEngine.Object activeState = activeStateProperty.objectReferenceValue;

					if (Selection.gameObjects.Length <= 1)
					{
						Rect dragArea = EditorGUILayout.BeginHorizontal();

						EditorGUI.BeginChangeCheck();

						int stateIndex = EditorGUILayout.Popup(string.Format("Active State ({0})", i), statesProperty.IndexOf(activeState) + 1, currentLayerStatesName.ToArray(), GUILayout.MinWidth(200)) - 1;
						activeState = stateIndex == -1 ? null : statesProperty.GetValue<UnityEngine.Object>(Mathf.Clamp(stateIndex, 0, statesProperty.arraySize - 1));
						activeStateProperty.SetValue(activeState);

						if (EditorGUI.EndChangeCheck() && Application.isPlaying)
							layer.SwitchState(activeState == null ? typeof(EmptyState) : activeState.GetType(), i);

						if (i == 0)
							SmallAddButton(activeStatesProperty);
						else if (DeleteButton(activeStatesProperty, i))
							break;

						EditorGUILayout.EndHorizontal();

						Reorderable(activeStatesProperty, i, true, EditorGUI.IndentedRect(dragArea));
					}
					else
						GUI.Box(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), "Multi-editing is not supported.", new GUIStyle(EditorStyles.helpBox));
				}

				Separator();
				ShowLayerFields(layerSerialized);

				bool stateSeparator = statesProperty.arraySize > 0;
				layerSerialized.ApplyModifiedProperties();

				ShowStates(statesProperty, layer);

				if (stateSeparator)
					Separator();

				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowLayerOptions(PStateLayer layer, Rect rect)
		{
			bool isScrolling = (Screen.width - rect.xMax) > 5;

			GUIStyle style = new GUIStyle("MiniToolbarPopup");
			style.fontStyle = FontStyle.Bold;

			rect.x = Screen.width - (isScrolling ? 60 : 45) - EditorGUI.indentLevel * 15;
			rect.y += 1;
			rect.width = 24 + EditorGUI.indentLevel * 15;

			if (!Application.isPlaying && (Event.current.type == EventType.ExecuteCommand || rect.Contains(Event.current.mousePosition)))
			{
				List<UnityEngine.Object> validParents = new List<UnityEngine.Object>();
				List<Type> layerTypes = new List<Type>();
				List<string> options = new List<string> { "..." };
				bool machineIsParent = PStateMachineUtility.IsParent(layer, machine);

				foreach (KeyValuePair<Type, List<Type>> pair in PStateMachineUtility.LayerTypeStateTypeDict)
				{
					PropertyInfo layerProperty = pair.Key.GetProperty("Layer", ReflectionExtensions.AllFlags);
					PropertyInfo machineProperty = pair.Key.GetProperty("Machine", ReflectionExtensions.AllFlags);

					if ((machineProperty == null || machineProperty.PropertyType.IsInstanceOfType(machine)) && (layerProperty == null || layerProperty.PropertyType.IsInstanceOfType(layer)) && Array.TrueForAll(existingLayers, existingLayer => existingLayer.GetType() != pair.Key))
					{
						layerTypes.Add(pair.Key);
						options.Add("Add/" + PStateMachineUtility.LayerTypeFormattedDict[pair.Key]);
					}
				}

				if (!machineIsParent)
				{
					validParents.Add(machine);
					options.Add("Move To/Machine");
				}

				for (int i = 0; i < existingLayers.Length; i++)
				{
					PStateLayer existingLayer = existingLayers[i];

					if (layer != existingLayer && !PStateMachineUtility.IsParent(layer, existingLayer) && !PStateMachineUtility.GetSubLayersRecursive(layer).Contains(existingLayer))
					{
						validParents.Add(existingLayer);
						options.Add("Move To/" + PStateMachineUtility.FormatLayer(existingLayer.GetType()));
					}
				}

				options.Add("Copy");

				if (EditorPrefs.GetString("Clipboard Layer Type") == layer.GetType().Name)
				{
					options.Add("Paste/Layer");
					options.Add("Paste/Layer and States");
					options.Add("Paste/Layer and Sublayers");
					options.Add("Paste/All");
				}

				options.Add("Generate");

				if (Selection.gameObjects.Length <= 1)
				{
					//					int index = EditorGUI.Popup(rect, layerTypes.IndexOf(selectedLayerType) + 1, options.ToArray(), style) - 1;
					int index = EditorGUI.Popup(rect, 0, options.ToArray(), style) - 1;
					string option = index == -1 ? "" : options[index + 1];

					if (index < layerTypes.Count)
					{
						selectedLayerType = index == -1 ? null : layerTypes[Mathf.Clamp(index, 0, options.Count - 1)];

						if (selectedLayerType != null)
						{
							PStateMachineUtility.AddLayer(machine, selectedLayerType, layer);
							selectedLayerType = null;
						}
					}
					else if (option.StartsWith("Move To"))
					{
						UnityEngine.Object parent = validParents[index - layerTypes.Count];
						PStateMachineUtility.MoveLayerTo(layer, parent);
					}
					else if (option.StartsWith("Copy"))
					{
						EditorPrefs.SetString("Clipboard Layer Type", layer.GetType().Name);
						EditorPrefs.SetInt("Clipboard Layer ID", layer.GetInstanceID());
					}
					else if (option.StartsWith("Paste"))
					{
						PStateLayer layerToCopy = EditorUtility.InstanceIDToObject(EditorPrefs.GetInt("Clipboard Layer ID")) as PStateLayer;

						if (option == "Paste/Layer")
						{
							PStateMachineUtility.CopyLayer(layer, layerToCopy, false, false);
						}
						else if (option == "Paste/Layer and States")
						{
							PStateMachineUtility.CopyLayer(layer, layerToCopy, true, false);
						}
						else if (option == "Paste/Layer and Sublayers")
						{
							PStateMachineUtility.CopyLayer(layer, layerToCopy, false, true);
						}
						else if (option == "Paste/All")
						{
							PStateMachineUtility.CopyLayer(layer, layerToCopy, true, true);
						}

						EditorPrefs.SetString("Clipboard Layer Type", "");
						EditorPrefs.SetInt("Clipboard Layer ID", 0);
					}
					else if (option.StartsWith("Generate"))
					{
						PStateMachineGeneratorWindow generator = PStateMachineGeneratorWindow.Create();

						string layerTypeName = layer.GetTypeName();
						string layerScriptPath = HelperFunctions.GetAssetPath(layerTypeName + ".cs");

						generator.Path = string.IsNullOrEmpty(layerScriptPath) ? generator.Path : Path.GetDirectoryName(layerScriptPath);
						generator.Machine = PStateMachineUtility.FormatMachine(machine);
						generator.Layer = layerTypeName;
						generator.Inherit = PStateMachineUtility.FormatLayer(layer.GetType().BaseType);
						generator.SubLayer = layer.Layer == null ? "" : PStateMachineUtility.FormatLayer(layer.Layer.GetType());
					}
				}
			}
			else
			{
				EditorGUI.BeginDisabledGroup(Application.isPlaying);

				EditorGUI.Popup(rect, 0, new[] { "..." }, style);

				EditorGUI.EndDisabledGroup();
			}
		}

		void ShowLayerFields(SerializedObject layerSerialized)
		{
			SerializedProperty iterator = layerSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			iterator.NextVisible(false);

			if (!iterator.NextVisible(false))
				return;

			while (true)
			{
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.PropertyField(iterator, true);

				if (EditorGUI.EndChangeCheck())
					iterator.SetValueToSelected();

				if (!iterator.NextVisible(false))
					break;
			}

			Separator();
		}

		void ShowStates(SerializedProperty statesProperty, PStateLayer layer)
		{
			for (int i = 0; i < statesProperty.arraySize; i++)
			{
				SerializedProperty stateProperty = statesProperty.GetArrayElementAtIndex(i);
				PState state = stateProperty.GetValue<UnityEngine.Object>() as PState;

				if (state == null)
				{
					ShowLayer(statesProperty, i);
					continue;
				}

				BeginBox(GetBoxStyle(state));

				Foldout(stateProperty, PStateMachineUtility.FormatState(state.GetType(), layer).ToGUIContent(), GetStateStyle(state));
				Reorderable(statesProperty, i, true);

				ShowState(stateProperty);

				EndBox();
			}
		}

		void ShowState(SerializedProperty stateProperty)
		{
			SerializedObject stateSerialized = new SerializedObject(stateProperty.objectReferenceValue);

			if (stateProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				ShowStateFields(stateSerialized);

				EditorGUI.indentLevel -= 1;
			}

			stateSerialized.ApplyModifiedProperties();
		}

		void ShowStateFields(SerializedObject stateSerialized)
		{
			SerializedProperty iterator = stateSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);

			if (!iterator.NextVisible(false))
				return;

			while (true)
			{
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.PropertyField(iterator, true);

				if (EditorGUI.EndChangeCheck())
					iterator.SetValueToSelected();

				if (!iterator.NextVisible(false))
					break;
			}

			Separator();
		}

		void CleanUp()
		{
			for (int i = 0; i < targets.Length; i++)
			{
				PStateMachine selectedMachine = targets[i] as PStateMachine;

				if (selectedMachine != null)
					PStateMachineUtility.CleanUp(selectedMachine, selectedMachine.CachedGameObject);
			}
		}

		void CleanUpLayers(SerializedProperty layersProperty)
		{
			if (!Application.isPlaying && machine != null)
			{
				for (int i = layersProperty.arraySize - 1; i >= 0; i--)
				{
					if (layersProperty.GetValue<UnityEngine.Object>(i) == null)
						DeleteFromArray(layersProperty, i);
				}
			}
		}

		void CleanUpStates(SerializedProperty statesProperty)
		{
			if (!Application.isPlaying && machine != null)
			{
				for (int i = statesProperty.arraySize - 1; i >= 0; i--)
				{
					if (statesProperty.GetValue<UnityEngine.Object>(i) == null)
						DeleteFromArray(statesProperty, i);
				}
			}
		}

		void ReorderComponents()
		{
			if (PrefabUtility.GetPrefabType(machine) == PrefabType.Prefab)
				return;

			int firstStateOrLayerIndex = 0;

			Component[] components = machine.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				Component component = components[i];

				if (component as IState != null || component as IStateLayer != null || component as StateMachineCaller != null)
					firstStateOrLayerIndex = firstStateOrLayerIndex == 0 ? i : firstStateOrLayerIndex;
				else if (firstStateOrLayerIndex > 0)
				{
					for (int j = 0; j < i - firstStateOrLayerIndex; j++)
						ComponentUtility.MoveComponentUp(component);
				}
			}
		}

		GUIContent GetLayerLabel(PStateLayer layer)
		{
			string label = layer.GetType().Name;

			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab)
			{
				IState[] activeStates = layer.GetActiveStates();
				string[] activeStateNames = new string[activeStates.Length];

				for (int i = 0; i < activeStateNames.Length; i++)
					activeStateNames[i] = activeStates[i] is IStateLayer ? activeStates[i].GetTypeName() : PStateMachineUtility.FormatState(activeStates[i].GetType(), layer);

				label += " (" + activeStateNames.Concat(", ") + ")";
			}

			return label.ToGUIContent();
		}

		GUIStyle GetLayerStyle(PStateLayer layer)
		{
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;

			Color textColor = style.normal.textColor * 1.4F;

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;

			return style;
		}

		GUIStyle GetStateStyle(PState state)
		{
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor * 1.4F;

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;

			return style;
		}

		GUIStyle GetBoxStyle(IState state)
		{
			GUIStyle style = new GUIStyle("box");

			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab)
				style = state.IsActive ? CustomEditorStyles.GreenBox : CustomEditorStyles.RedBox;

			return style;
		}
	}
}
