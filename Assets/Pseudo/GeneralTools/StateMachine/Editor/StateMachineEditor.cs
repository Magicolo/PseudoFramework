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
	[CustomEditor(typeof(StateMachine), true), CanEditMultipleObjects]
	public class StateMachineEditor : CustomEditorBase
	{
		StateMachine _machine;
		GameObject _machineObject;
		StateLayer[] _existingLayers;
		State[] _existingStates;
		StateMachineCaller[] _existingCallers;

		Type _selectedLayerType;

		public override void OnEnable()
		{
			base.OnEnable();

			_machine = (StateMachine)target;
			_machineObject = _machine.CachedGameObject;
			_machine.SetScriptIcon(HelperFunctions.LoadAssetInFolder<Texture2D>("statemachine.png", "StateMachine"));

			HideMachineComponents();

			if (_machine.GetComponents<StateMachine>().Length > 1)
			{
				Debug.LogError("There can be only one StateMachine per GameObject.");
				_machine.Destroy();
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();

			StateMachineUtility.CleanUpAll();
			StateMachineUtility.CleanUp(_machine, _machineObject);
		}

		public override void OnInspectorGUI()
		{
			Begin();

			HideMachineComponents();
			ShowAddLayer();
			ShowMachineFields();
			ShowLayers(serializedObject.FindProperty("_stateReferences"));
			ReorderComponents();
			CleanUp();

			End();
		}

		void HideMachineComponents()
		{
			_existingLayers = _machineObject.GetComponents<StateLayer>();
			_existingStates = _machineObject.GetComponents<State>();
			_existingCallers = _machine.GetComponents<StateMachineCaller>();

			Array.ForEach(_existingLayers, layer => layer.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(_existingStates, state => state.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(_existingCallers, caller => caller.hideFlags = HideFlags.HideInInspector);
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

				foreach (KeyValuePair<Type, List<Type>> pair in StateMachineUtility.LayerTypeStateTypeDict)
				{
					PropertyInfo machineProperty = pair.Key.GetProperty("Machine", ObjectExtensions.AllFlags);
					PropertyInfo layerProperty = pair.Key.GetProperty("Layer", ObjectExtensions.AllFlags);
					bool machinePropertyValid = machineProperty.PropertyType == typeof(IStateMachine) || machineProperty.PropertyType.IsInstanceOfType(_machine);
					bool layerPropertyValid = layerProperty.PropertyType == typeof(IStateLayer) && Array.TrueForAll(_existingLayers, existingLayer => existingLayer.GetType() != pair.Key);

					if (machinePropertyValid && layerPropertyValid)
					{
						layerTypes.Add(pair.Key);
						layerTypesName.Add(StateMachineUtility.LayerTypeFormattedDict[pair.Key]);
					}
				}

				if (Selection.gameObjects.Length <= 1)
				{
					int layerTypeIndex = EditorGUILayout.Popup(layerTypes.IndexOf(_selectedLayerType) + 1, layerTypesName.ToArray(), style) - 1;
					_selectedLayerType = layerTypeIndex == -1 ? null : layerTypes[Mathf.Clamp(layerTypeIndex, 0, layerTypes.Count - 1)];

					if (_selectedLayerType != null)
					{
						StateMachineUtility.AddLayer(_machine, _selectedLayerType, _machine);
						_selectedLayerType = null;
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
				StateMachineGeneratorWindow window = StateMachineGeneratorWindow.Create();

				window.Machine = StateMachineUtility.FormatMachine(_machine);
				window.Layer = "";
				window.Inherit = "StateLayer";
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
			StateLayer layer = layerProperty.GetValue<StateLayer>();
			SerializedObject layerSerialized = new SerializedObject(layer);
			SerializedProperty statesProperty = layerSerialized.FindProperty("_stateReferences");
			SerializedProperty activeStatesProperty = layerSerialized.FindProperty("_activeStateReferences");

			StateMachineUtility.AddMissingStates(_machine, layer);

			BeginBox(GetBoxStyle(layer));

			Rect rect = EditorGUILayout.BeginHorizontal();

			ShowLayerOptions(layer, rect);

			if (DeleteFoldOut(layersProperty, index, GetLayerLabel(layer), GetLayerStyle(layer)))
			{
				StateMachineUtility.RemoveLayer(_machine, layer);
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
					currentLayerStatesName.Add(state is IStateLayer ? state.GetTypeName() : StateMachineUtility.FormatState(state.GetType(), layer));
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

		void ShowLayerOptions(StateLayer layer, Rect rect)
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
				bool machineIsParent = StateMachineUtility.IsParent(layer, _machine);

				foreach (KeyValuePair<Type, List<Type>> pair in StateMachineUtility.LayerTypeStateTypeDict)
				{
					PropertyInfo layerProperty = pair.Key.GetProperty("Layer", ObjectExtensions.AllFlags);
					PropertyInfo machineProperty = pair.Key.GetProperty("Machine", ObjectExtensions.AllFlags);

					if ((machineProperty == null || machineProperty.PropertyType.IsInstanceOfType(_machine)) && (layerProperty == null || layerProperty.PropertyType.IsInstanceOfType(layer)) && Array.TrueForAll(_existingLayers, existingLayer => existingLayer.GetType() != pair.Key))
					{
						layerTypes.Add(pair.Key);
						options.Add("Add/" + StateMachineUtility.LayerTypeFormattedDict[pair.Key]);
					}
				}

				if (!machineIsParent)
				{
					validParents.Add(_machine);
					options.Add("Move To/Machine");
				}

				for (int i = 0; i < _existingLayers.Length; i++)
				{
					StateLayer existingLayer = _existingLayers[i];

					if (layer != existingLayer && !StateMachineUtility.IsParent(layer, existingLayer) && !StateMachineUtility.GetSubLayersRecursive(layer).Contains(existingLayer))
					{
						validParents.Add(existingLayer);
						options.Add("Move To/" + StateMachineUtility.FormatLayer(existingLayer.GetType()));
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
						_selectedLayerType = index == -1 ? null : layerTypes[Mathf.Clamp(index, 0, options.Count - 1)];

						if (_selectedLayerType != null)
						{
							StateMachineUtility.AddLayer(_machine, _selectedLayerType, layer);
							_selectedLayerType = null;
						}
					}
					else if (option.StartsWith("Move To"))
					{
						UnityEngine.Object parent = validParents[index - layerTypes.Count];
						StateMachineUtility.MoveLayerTo(layer, parent);
					}
					else if (option.StartsWith("Copy"))
					{
						EditorPrefs.SetString("Clipboard Layer Type", layer.GetType().Name);
						EditorPrefs.SetInt("Clipboard Layer ID", layer.GetInstanceID());
					}
					else if (option.StartsWith("Paste"))
					{
						StateLayer layerToCopy = EditorUtility.InstanceIDToObject(EditorPrefs.GetInt("Clipboard Layer ID")) as StateLayer;

						if (option == "Paste/Layer")
						{
							StateMachineUtility.CopyLayer(layer, layerToCopy, false, false);
						}
						else if (option == "Paste/Layer and States")
						{
							StateMachineUtility.CopyLayer(layer, layerToCopy, true, false);
						}
						else if (option == "Paste/Layer and Sublayers")
						{
							StateMachineUtility.CopyLayer(layer, layerToCopy, false, true);
						}
						else if (option == "Paste/All")
						{
							StateMachineUtility.CopyLayer(layer, layerToCopy, true, true);
						}

						EditorPrefs.SetString("Clipboard Layer Type", "");
						EditorPrefs.SetInt("Clipboard Layer ID", 0);
					}
					else if (option.StartsWith("Generate"))
					{
						StateMachineGeneratorWindow generator = StateMachineGeneratorWindow.Create();

						string layerTypeName = layer.GetTypeName();
						string layerScriptPath = HelperFunctions.GetAssetPath(layerTypeName + ".cs");

						generator.Path = string.IsNullOrEmpty(layerScriptPath) ? generator.Path : Path.GetDirectoryName(layerScriptPath);
						generator.Machine = StateMachineUtility.FormatMachine(_machine);
						generator.Layer = layerTypeName;
						generator.Inherit = StateMachineUtility.FormatLayer(layer.GetType().BaseType);
						generator.SubLayer = layer.Layer == null ? "" : StateMachineUtility.FormatLayer(layer.Layer.GetType());
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

		void ShowStates(SerializedProperty statesProperty, StateLayer layer)
		{
			for (int i = 0; i < statesProperty.arraySize; i++)
			{
				SerializedProperty stateProperty = statesProperty.GetArrayElementAtIndex(i);
				State state = stateProperty.GetValue<UnityEngine.Object>() as State;

				if (state == null)
				{
					ShowLayer(statesProperty, i);
					continue;
				}

				BeginBox(GetBoxStyle(state));

				Foldout(stateProperty, StateMachineUtility.FormatState(state.GetType(), layer).ToGUIContent(), GetStateStyle(state));
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
				StateMachine selectedMachine = targets[i] as StateMachine;

				if (selectedMachine != null)
					StateMachineUtility.CleanUp(selectedMachine, selectedMachine.CachedGameObject);
			}
		}

		void CleanUpLayers(SerializedProperty layersProperty)
		{
			if (!Application.isPlaying && _machine != null)
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
			if (!Application.isPlaying && _machine != null)
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
			if (PrefabUtility.GetPrefabType(_machine) == PrefabType.Prefab)
				return;

			int firstStateOrLayerIndex = 0;

			Component[] components = _machine.GetComponents<Component>();
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

		GUIContent GetLayerLabel(StateLayer layer)
		{
			string label = layer.GetType().Name;

			if (Application.isPlaying && PrefabUtility.GetPrefabType(_machine) != PrefabType.Prefab)
			{
				IState[] activeStates = layer.GetActiveStates();
				string[] activeStateNames = new string[activeStates.Length];

				for (int i = 0; i < activeStateNames.Length; i++)
					activeStateNames[i] = activeStates[i] is IStateLayer ? activeStates[i].GetTypeName() : StateMachineUtility.FormatState(activeStates[i].GetType(), layer);

				label += " (" + activeStateNames.Concat(", ") + ")";
			}

			return label.ToGUIContent();
		}

		GUIStyle GetLayerStyle(StateLayer layer)
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

		GUIStyle GetStateStyle(State state)
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

			if (Application.isPlaying && PrefabUtility.GetPrefabType(_machine) != PrefabType.Prefab)
				style = state.IsActive ? CustomEditorStyles.GreenBox : CustomEditorStyles.RedBox;

			return style;
		}
	}
}
