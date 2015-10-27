using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	public static class PStateMachineUtility
	{
		public static Type[] CallbackTypes = {
			typeof(PStateMachineUpdateCaller), typeof(PStateMachineFixedUpdateCaller), typeof(PStateMachineLateUpdateCaller),
			typeof(PStateMachineCollisionEnterCaller), typeof(PStateMachineCollisionStayCaller), typeof(PStateMachineCollisionExitCaller),
			typeof(PStateMachineCollisionEnter2DCaller), typeof(PStateMachineCollisionStay2DCaller), typeof(PStateMachineCollisionExit2DCaller),
			typeof(PStateMachineTriggerEnterCaller), typeof(PStateMachineTriggerStayCaller), typeof(PStateMachineTriggerExitCaller),
			typeof(PStateMachineTriggerEnter2DCaller), typeof(PStateMachineTriggerStay2DCaller), typeof(PStateMachineTriggerExit2DCaller)
		};

		public static string[] CallbackNames = {
			"OnUpdate", "OnFixedUpdate", "OnLateUpdate",
			"CollisionEnter", "CollisionStay", "CollisionExit",
			"CollisionEnter2D", "CollisionStay2D", "CollisionExit2D",
			"TriggerEnter", "TriggerStay", "TriggerExit",
			"TriggerEnter2D", "TriggerStay2D", "TriggerExit2D"
		};

		public static string[] FullCallbackNames = {
			"OnEnter", "OnExit",
			"OnUpdate", "OnFixedUpdate", "OnLateUpdate",
			"CollisionEnter", "CollisionStay", "CollisionExit",
			"CollisionEnter2D", "CollisionStay2D", "CollisionExit2D",
			"TriggerEnter", "TriggerStay", "TriggerExit",
			"TriggerEnter2D", "TriggerStay2D", "TriggerExit2D"
		};

		public static string[] CallbackOverrideMethods = {
			"OnEnter()", "OnExit()",
			"OnUpdate()", "OnFixedUpdate()", "OnLateUpdate()",
			"CollisionEnter(Collision collision)", "CollisionStay(Collision collision)", "CollisionExit(Collision collision)",
			"CollisionEnter2D(Collision2D collision)", "CollisionStay2D(Collision2D collision)", "CollisionExit2D(Collision2D collision)",
			"TriggerEnter(Collider collision)", "TriggerStay(Collider collision)", "TriggerExit(Collider collision)",
			"TriggerEnter2D(Collider2D collision)", "TriggerStay2D(Collider2D collision)", "TriggerExit2D(Collider2D collision)"
		};

		public static string[] CallbackBaseMethods = {
			"OnEnter()", "OnExit()",
			"OnUpdate()", "OnFixedUpdate()", "OnLateUpdate()",
			"CollisionEnter(collision)", "CollisionStay(collision)", "CollisionExit(collision)",
			"CollisionEnter2D(collision)", "CollisionStay2D(collision)", "CollisionExit2D(collision)",
			"TriggerEnter(collision)", "TriggerStay(collision)", "TriggerExit(collision)",
			"TriggerEnter2D(collision)", "TriggerStay2D(collision)", "TriggerExit2D(collision)"
		};

		static List<Type> machineTypes;
		public static List<Type> MachineTypes
		{
			get
			{
				if (machineTypes == null)
					BuildDicts();

				return machineTypes;
			}
		}

		static List<Type> layerTypes;
		public static List<Type> LayerTypes
		{
			get
			{
				if (layerTypes == null)
					BuildDicts();

				return layerTypes;
			}
		}

		static List<Type> stateTypes;
		public static List<Type> StateTypes
		{
			get
			{
				if (stateTypes == null)
					BuildDicts();

				return stateTypes;
			}
		}

		static Dictionary<string, Type> machineFormattedTypeDict;
		public static Dictionary<string, Type> MachineFormattedTypeDict
		{
			get
			{
				if (machineFormattedTypeDict == null)
					BuildDicts();

				return machineFormattedTypeDict;
			}
		}

		static Dictionary<Type, List<Type>> layerTypeStateTypeDict;
		public static Dictionary<Type, List<Type>> LayerTypeStateTypeDict
		{
			get
			{
				if (layerTypeStateTypeDict == null)
					BuildDicts();

				return layerTypeStateTypeDict;
			}
		}

		static Dictionary<string, List<string>> layerFormattedStateFormattedDict;
		public static Dictionary<string, List<string>> LayerFormattedStateFormattedDict
		{
			get
			{
				if (layerTypeStateTypeDict == null)
					BuildDicts();

				return layerFormattedStateFormattedDict;
			}
		}

		static Dictionary<Type, string> layerTypeFormattedDict;
		public static Dictionary<Type, string> LayerTypeFormattedDict
		{
			get
			{
				if (layerTypeFormattedDict == null)
					BuildDicts();

				return layerTypeFormattedDict;
			}
		}

		static Dictionary<string, Type> layerFormattedTypeDict;
		public static Dictionary<string, Type> LayerFormattedTypeDict
		{
			get
			{
				if (layerFormattedTypeDict == null)
					BuildDicts();

				return layerFormattedTypeDict;
			}
		}

		static Dictionary<string, Type> stateNameTypeDict;
		public static Dictionary<string, Type> StateNameTypeDict
		{
			get
			{
				if (stateNameTypeDict == null)
					BuildDicts();

				return stateNameTypeDict;
			}
		}

		public static PStateLayer AddLayer(PStateMachine machine, PState state)
		{
			PStateLayer layer = null;

#if UNITY_EDITOR
			layer = AddLayer(machine, GetLayerTypeFromState(state), machine);
			AddState(machine, layer, state);
#endif

			return layer;
		}

		public static PStateLayer AddLayer(PStateMachine machine, Type layerType, UnityEngine.Object parent)
		{
			PStateLayer layer = null;

#if UNITY_EDITOR
			layer = Array.Find(machine.GetComponents(layerType), component => component.GetType() == layerType) as PStateLayer;
			layer = layer == null ? machine.gameObject.AddComponent(layerType) as PStateLayer : layer;
			layer = AddLayer(machine, layer, parent);
#endif

			return layer;
		}

		public static PStateLayer AddLayer(PStateMachine machine, PStateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			layer.hideFlags = HideFlags.HideInInspector;

			UnityEditor.SerializedObject parentSerialized = new UnityEditor.SerializedObject(parent);
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty layersProperty = parentSerialized.FindProperty("stateReferences");
			UnityEditor.SerializedProperty parentProperty = layerSerialized.FindProperty("parentReference");

			if (parentProperty.GetValue<UnityEngine.Object>() == null)
				parentProperty.SetValue(parent);

			layerSerialized.FindProperty("machineReference").SetValue(machine);
			layerSerialized.ApplyModifiedProperties();

			if (!layersProperty.Contains(layer))
				layersProperty.Add(layer);

			UpdateLayerStates(machine, layer);
			UpdateCallbacks(machine);
#endif

			return layer;
		}

		public static void RemoveLayer(PStateMachine machine, PStateLayer layer)
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			UnityEngine.Object[] states = statesProperty.GetValues<UnityEngine.Object>();

			for (int i = 0; i < states.Length; i++)
			{
				UnityEngine.Object state = states[i];
				PStateLayer sublayer = state as PStateLayer;

				if (sublayer != null)
					RemoveLayer(machine, sublayer);
				else
					state.Destroy();
			}

			layer.Destroy();
			UpdateCallbacks(machine);
#endif
		}

		public static PState AddState(PStateMachine machine, PStateLayer layer, Type stateType)
		{
			PState state = null;

#if UNITY_EDITOR
			state = Array.Find(machine.GetComponents(stateType), component => component.GetType() == stateType) as PState;
			state = state == null ? machine.gameObject.AddComponent(stateType) as PState : state;
			state = AddState(machine, layer, state);
#endif

			return state;
		}

		public static PState AddState(PStateMachine machine, PStateLayer layer, PState state)
		{
#if UNITY_EDITOR
			state.hideFlags = HideFlags.HideInInspector;

			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedObject stateSerialized = new UnityEditor.SerializedObject(state);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");

			stateSerialized.FindProperty("layerReference").SetValue(layer);
			stateSerialized.FindProperty("machineReference").SetValue(machine);
			stateSerialized.ApplyModifiedProperties();

			if (!statesProperty.Contains(state))
				statesProperty.Add(state);
#endif

			return state;
		}

		public static void AddMissingStates(PStateMachine machine, PStateLayer layer)
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			List<Type> stateTypes = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < stateTypes.Count; i++)
			{
				Type stateType = stateTypes[i];

				if (statesProperty != null && !Array.Exists(statesProperty.GetValues<UnityEngine.Object>(), state => state.GetType() == stateType))
					AddState(machine, layer, stateType);
			}
#endif
		}

		public static void MoveLayerTo(PStateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedObject newParentSerialized = new UnityEditor.SerializedObject(parent);
			UnityEditor.SerializedProperty oldParentProperty = layerSerialized.FindProperty("parentReference");
			UnityEditor.SerializedObject oldParentSerialized = new UnityEditor.SerializedObject(oldParentProperty.GetValue<UnityEngine.Object>());

			oldParentProperty.SetValue(parent);
			oldParentSerialized.FindProperty("stateReferences").Remove(layer);
			newParentSerialized.FindProperty("stateReferences").Add(layer);

			layerSerialized.ApplyModifiedProperties();
			newParentSerialized.ApplyModifiedProperties();
			oldParentSerialized.ApplyModifiedProperties();
#endif
		}

		public static void CopyState(PState state, PState stateToCopy)
		{
#if UNITY_EDITOR
			if (stateToCopy == null)
				return;

			UnityEditor.SerializedObject stateSerialized = new UnityEditor.SerializedObject(state);
			UnityEngine.Object parentReference = stateSerialized.FindProperty("layerReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object machineReference = stateSerialized.FindProperty("machineReference").GetValue<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(stateToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(state);

			stateSerialized = new UnityEditor.SerializedObject(state);
			stateSerialized.FindProperty("layerReference").SetValue(parentReference);
			stateSerialized.FindProperty("machineReference").SetValue(machineReference);
#endif
		}

		public static void CopyLayer(PStateLayer layer, PStateLayer layerToCopy, bool copyStates, bool copySublayers)
		{
#if UNITY_EDITOR
			if (layerToCopy == null)
				return;

			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEngine.Object parentReference = layerSerialized.FindProperty("parentReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object machineReference = layerSerialized.FindProperty("machineReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object[] stateReferences = layerSerialized.FindProperty("stateReferences").GetValues<UnityEngine.Object>();
			UnityEngine.Object[] activeStateReferences = layerSerialized.FindProperty("activeStateReferences").GetValues<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(layerToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(layer);

			layerSerialized = new UnityEditor.SerializedObject(layer);
			layerSerialized.FindProperty("parentReference").SetValue(parentReference);
			layerSerialized.FindProperty("machineReference").SetValue(machineReference);
			layerSerialized.FindProperty("stateReferences").SetValues(stateReferences);
			layerSerialized.FindProperty("activeStateReferences").SetValues(activeStateReferences);

			for (int i = 0; i < stateReferences.Length; i++)
			{
				UnityEngine.Object stateReference = stateReferences[i];
				PState state = stateReference as PState;
				PStateLayer sublayer = stateReference as PStateLayer;

				if (copyStates && state != null)
				{
					PState stateToCopy = layerToCopy.GetState(state.GetType()) as PState;

					if (stateToCopy != null)
						CopyState(state, stateToCopy);
				}

				if (copySublayers && sublayer != null)
				{
					PStateLayer sublayerToCopy = layerToCopy.GetState(sublayer.GetType()) as PStateLayer;

					if (sublayerToCopy != null)
						CopyLayer(sublayer, sublayerToCopy, copyStates, copySublayers);
				}
			}
#endif
		}

		public static bool IsParent(PStateLayer layer, UnityEngine.Object parent)
		{
			bool isParent = false;

#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			isParent = layerSerialized.FindProperty("parentReference").GetValue<UnityEngine.Object>() == parent;
#endif

			return isParent;
		}

		public static Type GetLayerTypeFromState(PState state)
		{
			return GetLayerTypeFromState(state.GetType());
		}

		public static Type GetLayerTypeFromState(Type stateType)
		{
			Type layerType = null;

			foreach (KeyValuePair<Type, List<Type>> pair in LayerTypeStateTypeDict)
			{
				if (pair.Value.Contains(stateType))
				{
					layerType = pair.Key;
					break;
				}
			}

			return layerType;
		}

		public static string FormatMachine(Type machineType)
		{
			return machineType.GetName();
		}

		public static string FormatMachine(PStateMachine machine)
		{
			return FormatMachine(machine.GetType());
		}

		public static string FormatLayer(Type layerType)
		{
			string formattedLayer = layerType.GetName().SplitWords(2).Concat("/");

			PropertyInfo machineProperty = layerType.GetProperty("Machine", ReflectionExtensions.AllFlags);
			PropertyInfo layerProperty = layerType.GetProperty("Layer", ReflectionExtensions.AllFlags);

			if (machineProperty != null && typeof(IStateMachine).IsAssignableFrom(machineProperty.PropertyType))
				formattedLayer = string.Format("{0} [M: {1}]", formattedLayer, FormatMachine(machineProperty.PropertyType));

			if (layerProperty != null && typeof(IStateLayer).IsAssignableFrom(layerProperty.PropertyType))
				formattedLayer = string.Format("{0} [L: {1}]", formattedLayer, GetLayerPrefix(layerProperty.PropertyType));

			return formattedLayer;
		}

		public static string FormatState(Type stateType, string layerTypePrefix)
		{
			string formattedState = stateType.GetName();

			if (formattedState.StartsWith(layerTypePrefix))
				formattedState = formattedState.Substring(layerTypePrefix.Length);

			return formattedState;
		}

		public static string FormatState(Type stateType, PStateLayer layer)
		{
			return FormatState(stateType, GetLayerPrefix(layer));
		}

		public static string FormatState(Type stateType, Type layerType)
		{
			return FormatState(stateType, GetLayerPrefix(layerType));
		}

		public static string GetLayerPrefix(Type layerType)
		{
			return layerType.GetName();
		}

		public static string GetLayerPrefix(PStateLayer layer)
		{
			return GetLayerPrefix(layer.GetType());
		}

		public static PStateLayer[] GetSubLayersRecursive(PStateLayer layer)
		{
			List<PStateLayer> subLayers = new List<PStateLayer>();

#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty subLayersProperty = layerSerialized.FindProperty("stateReferences");

			for (int i = 0; i < subLayersProperty.arraySize; i++)
			{
				PStateLayer subLayer = subLayersProperty.GetValue(i) as PStateLayer;

				if (subLayer != null)
				{
					subLayers.Add(subLayer);
					subLayers.AddRange(GetSubLayersRecursive(subLayer));
				}
			}
#endif

			return subLayers.ToArray();
		}

		public static void UpdateLayerStates(PStateMachine machine)
		{
			PStateLayer[] layers = machine.GetComponents<PStateLayer>();

			for (int i = 0; i < layers.Length; i++)
				UpdateLayerStates(machine, layers[i]);
		}

		public static void UpdateLayerStates(PStateMachine machine, PStateLayer layer)
		{
			List<Type> types = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < types.Count; i++)
				AddState(machine, layer, types[i]);
		}

		public static void UpdateCallbacks(PStateMachine machine)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			int callerMask = 0;
			PStateLayer[] layers = machine.GetComponents<PStateLayer>();
			PState[] states = machine.GetComponents<PState>();

			for (int i = 0; i < layers.Length; i++)
			{
				PStateLayer layer = layers[i];
				Type layerType = layer.GetType();

				while (layerType != typeof(PStateLayer) || !typeof(PStateLayer).IsAssignableFrom(layerType))
				{
					MethodInfo[] methods = layerType.GetMethods(flags);

					for (int j = 0; j < methods.Length; j++)
					{
						MethodInfo method = methods[j];
						if (CallbackNames.Contains(method.Name))
							callerMask |= 1 << (Array.IndexOf(CallbackNames, method.Name) + 2);
					}

					layerType = layerType.BaseType;
				}
			}

			for (int i = 0; i < states.Length; i++)
			{
				PState state = states[i];
				Type stateType = state.GetType();

				while (stateType != typeof(PState) || !typeof(PState).IsAssignableFrom(stateType))
				{
					MethodInfo[] methods = stateType.GetMethods(flags);

					for (int j = 0; j < methods.Length; j++)
					{
						MethodInfo method = methods[j];
						if (CallbackNames.Contains(method.Name))
							callerMask |= 1 << (Array.IndexOf(CallbackNames, method.Name) + 2);
					}

					stateType = stateType.BaseType;
				}
			}

			for (int i = 0; i < CallbackTypes.Length; i++)
			{
				if ((callerMask & 1 << i + 2) != 0)
				{
					StateMachineCaller caller = machine.gameObject.GetOrAddComponent(CallbackTypes[i]) as StateMachineCaller;

					caller.hideFlags = HideFlags.HideInInspector;
					caller.machine = machine;
				}
				else
				{
					StateMachineCaller caller = machine.GetComponent(CallbackTypes[i]) as StateMachineCaller;

					if (caller != null)
						caller.Destroy();
				}
			}
		}

		public static void UpdateAll()
		{
			if (!Application.isPlaying)
			{
				PStateMachine[] machines = Resources.FindObjectsOfTypeAll<PStateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					PStateMachine machine = machines[i];
					UpdateCallbacks(machine);
					UpdateLayerStates(machine);
				}
			}
		}

		public static void CleanUp(PStateMachine machine, GameObject gameObject)
		{
			if (!Application.isPlaying && gameObject != null)
			{
				PStateLayer[] layers = gameObject.GetComponents<PStateLayer>();
				PState[] states = gameObject.GetComponents<PState>();
				StateMachineCaller[] callers = gameObject.GetComponents<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					PStateLayer layer = layers[i];

					if (machine == null || layer.Machine == null || !object.ReferenceEquals(layer.Machine, machine) || layer.CachedGameObject != gameObject)
						layer.Destroy();
				}

				for (int i = 0; i < states.Length; i++)
				{
					PState state = states[i];

					if (machine == null || state.Machine == null || !object.ReferenceEquals(state.Machine, machine) || state.CachedGameObject != gameObject || state.Layer == null)
						state.Destroy();
				}

				for (int i = 0; i < callers.Length; i++)
				{
					StateMachineCaller caller = callers[i];

					if (machine == null || caller.machine == null || caller.machine != machine || caller.gameObject != gameObject)
						caller.Destroy();
				}
			}
		}

		public static void CleanUpAll()
		{
			if (!Application.isPlaying)
			{
				PStateLayer[] layers = Resources.FindObjectsOfTypeAll<PStateLayer>();
				PState[] states = Resources.FindObjectsOfTypeAll<PState>();
				StateMachineCaller[] callers = Resources.FindObjectsOfTypeAll<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					PStateLayer layer = layers[i];

					if (layer.Machine == null)
					{
						Type layerType = layer.GetType();
						PStateMachine machine = layer.CachedGameObject.GetOrAddComponent<PStateMachine>();

						layer.Destroy();
						AddLayer(machine, layerType, machine);
					}
				}

				for (int i = 0; i < states.Length; i++)
				{
					PState state = states[i];

					if (state.Machine == null || state.Layer == null)
					{
						Type stateType = state.GetType();
						PStateMachine machine = state.CachedGameObject.GetOrAddComponent<PStateMachine>();

						state.Destroy();
						AddLayer(machine, GetLayerTypeFromState(stateType), machine);
					}
				}

				for (int i = 0; i < callers.Length; i++)
				{
					StateMachineCaller caller = callers[i];

					if (caller.machine == null)
						caller.Destroy();
				}
			}
		}

		public static void SetIconAll()
		{
			if (!Application.isPlaying)
			{
				PStateMachine[] machines = Resources.FindObjectsOfTypeAll<PStateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					PStateMachine machine = machines[i];
					UpdateCallbacks(machine);
					UpdateLayerStates(machine);
				}
			}
		}

		public static void BuildDicts()
		{
			machineTypes = new List<Type>();
			layerTypes = new List<Type>();
			stateTypes = new List<Type>();
			machineFormattedTypeDict = new Dictionary<string, Type>();
			layerTypeStateTypeDict = new Dictionary<Type, List<Type>>();
			layerFormattedStateFormattedDict = new Dictionary<string, List<string>>();
			layerTypeFormattedDict = new Dictionary<Type, string>();
			layerFormattedTypeDict = new Dictionary<string, Type>();
			stateNameTypeDict = new Dictionary<string, Type>();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly = assemblies[i];
				Type[] types = assembly.GetTypes();

				for (int j = 0; j < types.Length; j++)
				{
					Type type = types[j];

					if (type.IsSubclassOf(typeof(PStateMachine)))
						machineTypes.Add(type);
					else if (type.IsSubclassOf(typeof(PStateLayer)))
						layerTypes.Add(type);
					else if (type.IsSubclassOf(typeof(PState)))
						stateTypes.Add(type);
				}
			}

			for (int i = 0; i < machineTypes.Count; i++)
			{
				Type machineType = machineTypes[i];
				machineFormattedTypeDict[FormatMachine(machineType)] = machineType;
			}

			for (int i = 0; i < layerTypes.Count; i++)
			{
				Type layerType = layerTypes[i];
				string layerTypeName = FormatLayer(layerType);

				layerTypeStateTypeDict[layerType] = new List<Type>();
				layerFormattedStateFormattedDict[layerTypeName] = new List<string>();
				layerTypeFormattedDict[layerType] = layerTypeName;
				layerFormattedTypeDict[layerTypeName] = layerType;
			}

			for (int i = 0; i < stateTypes.Count; i++)
			{
				Type stateType = stateTypes[i];
				PropertyInfo layerProperty = stateType.GetProperty("Layer", ReflectionExtensions.AllFlags);

				if (layerProperty != null && typeof(IStateLayer).IsAssignableFrom(layerProperty.PropertyType))
				{
					string layerTypeName = FormatLayer(layerProperty.PropertyType);
					string layerTypePrefix = GetLayerPrefix(layerProperty.PropertyType);

					layerTypeStateTypeDict[layerProperty.PropertyType].Add(stateType);
					layerFormattedStateFormattedDict[layerTypeName].Add(FormatState(stateType, layerTypePrefix));
				}

				stateNameTypeDict[stateType.GetName()] = stateType;
			}
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts()
		{
			layerTypeStateTypeDict = null;
			layerFormattedStateFormattedDict = null;
			layerTypeFormattedDict = null;
			layerFormattedTypeDict = null;
			stateNameTypeDict = null;

			CleanUpAll();
			UpdateAll();
			SetIconAll();
		}
#endif
	}
}
