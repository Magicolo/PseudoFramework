using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	public static class StateMachineUtility
	{
		public static Type[] CallbackTypes = {
			typeof(StateMachineUpdateCaller), typeof(StateMachineFixedUpdateCaller), typeof(StateMachineLateUpdateCaller),
			typeof(StateMachineCollisionEnterCaller), typeof(StateMachineCollisionStayCaller), typeof(StateMachineCollisionExitCaller),
			typeof(StateMachineCollisionEnter2DCaller), typeof(StateMachineCollisionStay2DCaller), typeof(StateMachineCollisionExit2DCaller),
			typeof(StateMachineTriggerEnterCaller), typeof(StateMachineTriggerStayCaller), typeof(StateMachineTriggerExitCaller),
			typeof(StateMachineTriggerEnter2DCaller), typeof(StateMachineTriggerStay2DCaller), typeof(StateMachineTriggerExit2DCaller)
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

		public static StateLayer AddLayer(StateMachine machine, State state)
		{
			StateLayer layer = null;

#if UNITY_EDITOR
			layer = AddLayer(machine, GetLayerTypeFromState(state), machine);
			AddState(machine, layer, state);
#endif

			return layer;
		}

		public static StateLayer AddLayer(StateMachine machine, Type layerType, UnityEngine.Object parent)
		{
			StateLayer layer = null;

#if UNITY_EDITOR
			layer = Array.Find(machine.GetComponents(layerType), component => component.GetType() == layerType) as StateLayer;
			layer = layer == null ? machine.gameObject.AddComponent(layerType) as StateLayer : layer;
			layer = AddLayer(machine, layer, parent);
#endif

			return layer;
		}

		public static StateLayer AddLayer(StateMachine machine, StateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			layer.hideFlags = HideFlags.HideInInspector;

			var parentSerialized = new UnityEditor.SerializedObject(parent);
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var layersProperty = parentSerialized.FindProperty("stateReferences");
			var parentProperty = layerSerialized.FindProperty("parentReference");

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

		public static void RemoveLayer(StateMachine machine, StateLayer layer)
		{
#if UNITY_EDITOR
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var statesProperty = layerSerialized.FindProperty("stateReferences");
			var states = statesProperty.GetValues<UnityEngine.Object>();

			for (int i = 0; i < states.Length; i++)
			{
				var state = states[i];
				var sublayer = state as StateLayer;

				if (sublayer != null)
					RemoveLayer(machine, sublayer);
				else
					state.Destroy();
			}

			layer.Destroy();
			UpdateCallbacks(machine);
#endif
		}

		public static State AddState(StateMachine machine, StateLayer layer, Type stateType)
		{
			State state = null;

#if UNITY_EDITOR
			state = Array.Find(machine.GetComponents(stateType), component => component.GetType() == stateType) as State;
			state = state == null ? machine.gameObject.AddComponent(stateType) as State : state;
			state = AddState(machine, layer, state);
#endif

			return state;
		}

		public static State AddState(StateMachine machine, StateLayer layer, State state)
		{
#if UNITY_EDITOR
			state.hideFlags = HideFlags.HideInInspector;

			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var stateSerialized = new UnityEditor.SerializedObject(state);
			var statesProperty = layerSerialized.FindProperty("stateReferences");

			stateSerialized.FindProperty("layerReference").SetValue(layer);
			stateSerialized.FindProperty("machineReference").SetValue(machine);
			stateSerialized.ApplyModifiedProperties();

			if (!statesProperty.Contains(state))
				statesProperty.Add(state);
#endif

			return state;
		}

		public static void AddMissingStates(StateMachine machine, StateLayer layer)
		{
#if UNITY_EDITOR
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var statesProperty = layerSerialized.FindProperty("stateReferences");
			var stateTypes = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < stateTypes.Count; i++)
			{
				var stateType = stateTypes[i];

				if (statesProperty != null && !Array.Exists(statesProperty.GetValues<UnityEngine.Object>(), state => state.GetType() == stateType))
					AddState(machine, layer, stateType);
			}
#endif
		}

		public static void MoveLayerTo(StateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var newParentSerialized = new UnityEditor.SerializedObject(parent);
			var oldParentProperty = layerSerialized.FindProperty("parentReference");
			var oldParentSerialized = new UnityEditor.SerializedObject(oldParentProperty.GetValue<UnityEngine.Object>());

			oldParentProperty.SetValue(parent);
			oldParentSerialized.FindProperty("stateReferences").Remove(layer);
			newParentSerialized.FindProperty("stateReferences").Add(layer);

			layerSerialized.ApplyModifiedProperties();
			newParentSerialized.ApplyModifiedProperties();
			oldParentSerialized.ApplyModifiedProperties();
#endif
		}

		public static void CopyState(State state, State stateToCopy)
		{
#if UNITY_EDITOR
			if (stateToCopy == null)
				return;

			var stateSerialized = new UnityEditor.SerializedObject(state);
			var parentReference = stateSerialized.FindProperty("layerReference").GetValue<UnityEngine.Object>();
			var machineReference = stateSerialized.FindProperty("machineReference").GetValue<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(stateToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(state);

			stateSerialized = new UnityEditor.SerializedObject(state);
			stateSerialized.FindProperty("layerReference").SetValue(parentReference);
			stateSerialized.FindProperty("machineReference").SetValue(machineReference);
#endif
		}

		public static void CopyLayer(StateLayer layer, StateLayer layerToCopy, bool copyStates, bool copySublayers)
		{
#if UNITY_EDITOR
			if (layerToCopy == null)
				return;

			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var parentReference = layerSerialized.FindProperty("parentReference").GetValue<UnityEngine.Object>();
			var machineReference = layerSerialized.FindProperty("machineReference").GetValue<UnityEngine.Object>();
			var stateReferences = layerSerialized.FindProperty("stateReferences").GetValues<UnityEngine.Object>();
			var activeStateReferences = layerSerialized.FindProperty("activeStateReferences").GetValues<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(layerToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(layer);

			layerSerialized = new UnityEditor.SerializedObject(layer);
			layerSerialized.FindProperty("parentReference").SetValue(parentReference);
			layerSerialized.FindProperty("machineReference").SetValue(machineReference);
			layerSerialized.FindProperty("stateReferences").SetValues(stateReferences);
			layerSerialized.FindProperty("activeStateReferences").SetValues(activeStateReferences);

			for (int i = 0; i < stateReferences.Length; i++)
			{
				var stateReference = stateReferences[i];
				var state = stateReference as State;
				var sublayer = stateReference as StateLayer;

				if (copyStates && state != null)
				{
					var stateToCopy = layerToCopy.GetState(state.GetType()) as State;

					if (stateToCopy != null)
						CopyState(state, stateToCopy);
				}

				if (copySublayers && sublayer != null)
				{
					var sublayerToCopy = layerToCopy.GetState(sublayer.GetType()) as StateLayer;

					if (sublayerToCopy != null)
						CopyLayer(sublayer, sublayerToCopy, copyStates, copySublayers);
				}
			}
#endif
		}

		public static bool IsParent(StateLayer layer, UnityEngine.Object parent)
		{
			bool isParent = false;

#if UNITY_EDITOR
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			isParent = layerSerialized.FindProperty("parentReference").GetValue<UnityEngine.Object>() == parent;
#endif

			return isParent;
		}

		public static Type GetLayerTypeFromState(State state)
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

		public static string FormatMachine(StateMachine machine)
		{
			return FormatMachine(machine.GetType());
		}

		public static string FormatLayer(Type layerType)
		{
			var formattedLayer = layerType.GetName().SplitWords(2).Concat("/");
			var machineProperty = layerType.GetProperty("Machine", ReflectionExtensions.AllFlags);
			var layerProperty = layerType.GetProperty("Layer", ReflectionExtensions.AllFlags);

			if (machineProperty != null && typeof(IStateMachine).IsAssignableFrom(machineProperty.PropertyType))
				formattedLayer = string.Format("{0} [M: {1}]", formattedLayer, FormatMachine(machineProperty.PropertyType));

			if (layerProperty != null && typeof(IStateLayer).IsAssignableFrom(layerProperty.PropertyType))
				formattedLayer = string.Format("{0} [L: {1}]", formattedLayer, GetLayerPrefix(layerProperty.PropertyType));

			return formattedLayer;
		}

		public static string FormatState(Type stateType, string layerTypePrefix)
		{
			var formattedState = stateType.GetName();

			if (formattedState.StartsWith(layerTypePrefix))
				formattedState = formattedState.Substring(layerTypePrefix.Length);

			return formattedState;
		}

		public static string FormatState(Type stateType, StateLayer layer)
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

		public static string GetLayerPrefix(StateLayer layer)
		{
			return GetLayerPrefix(layer.GetType());
		}

		public static StateLayer[] GetSubLayersRecursive(StateLayer layer)
		{
			var subLayers = new List<StateLayer>();

#if UNITY_EDITOR
			var layerSerialized = new UnityEditor.SerializedObject(layer);
			var subLayersProperty = layerSerialized.FindProperty("stateReferences");

			for (int i = 0; i < subLayersProperty.arraySize; i++)
			{
				var subLayer = subLayersProperty.GetValue(i) as StateLayer;

				if (subLayer != null)
				{
					subLayers.Add(subLayer);
					subLayers.AddRange(GetSubLayersRecursive(subLayer));
				}
			}
#endif

			return subLayers.ToArray();
		}

		public static void UpdateLayerStates(StateMachine machine)
		{
			var layers = machine.GetComponents<StateLayer>();

			for (int i = 0; i < layers.Length; i++)
				UpdateLayerStates(machine, layers[i]);
		}

		public static void UpdateLayerStates(StateMachine machine, StateLayer layer)
		{
			var types = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < types.Count; i++)
				AddState(machine, layer, types[i]);
		}

		public static void UpdateCallbacks(StateMachine machine)
		{
			var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			int callerMask = 0;
			var layers = machine.GetComponents<StateLayer>();
			var states = machine.GetComponents<State>();

			for (int i = 0; i < layers.Length; i++)
			{
				var layer = layers[i];
				var layerType = layer.GetType();

				while (layerType != typeof(StateLayer) || !typeof(StateLayer).IsAssignableFrom(layerType))
				{
					var methods = layerType.GetMethods(flags);

					for (int j = 0; j < methods.Length; j++)
					{
						var method = methods[j];
						if (CallbackNames.Contains(method.Name))
							callerMask |= 1 << (Array.IndexOf(CallbackNames, method.Name) + 2);
					}

					layerType = layerType.BaseType;
				}
			}

			for (int i = 0; i < states.Length; i++)
			{
				var state = states[i];
				var stateType = state.GetType();

				while (stateType != typeof(State) || !typeof(State).IsAssignableFrom(stateType))
				{
					var methods = stateType.GetMethods(flags);

					for (int j = 0; j < methods.Length; j++)
					{
						var method = methods[j];

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
					var caller = machine.gameObject.GetOrAddComponent(CallbackTypes[i]) as StateMachineCaller;

					caller.hideFlags = HideFlags.HideInInspector;
					caller.machine = machine;
				}
				else
				{
					var caller = machine.GetComponent(CallbackTypes[i]) as StateMachineCaller;

					if (caller != null)
						caller.Destroy();
				}
			}
		}

		public static void UpdateAll()
		{
			if (!Application.isPlaying)
			{
				var machines = Resources.FindObjectsOfTypeAll<StateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					var machine = machines[i];
					UpdateCallbacks(machine);
					UpdateLayerStates(machine);
				}
			}
		}

		public static void CleanUp(StateMachine machine, GameObject gameObject)
		{
			if (!Application.isPlaying && gameObject != null)
			{
				var layers = gameObject.GetComponents<StateLayer>();
				var states = gameObject.GetComponents<State>();
				var callers = gameObject.GetComponents<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					var layer = layers[i];

					if (machine == null || layer.Machine == null || !object.ReferenceEquals(layer.Machine, machine) || layer.CachedGameObject != gameObject)
						layer.Destroy();
				}

				for (int i = 0; i < states.Length; i++)
				{
					var state = states[i];

					if (machine == null || state.Machine == null || !object.ReferenceEquals(state.Machine, machine) || state.CachedGameObject != gameObject || state.Layer == null)
						state.Destroy();
				}

				for (int i = 0; i < callers.Length; i++)
				{
					var caller = callers[i];

					if (machine == null || caller.machine == null || caller.machine != machine || caller.gameObject != gameObject)
						caller.Destroy();
				}
			}
		}

		public static void CleanUpAll()
		{
			if (!Application.isPlaying)
			{
				var layers = Resources.FindObjectsOfTypeAll<StateLayer>();
				var states = Resources.FindObjectsOfTypeAll<State>();
				var callers = Resources.FindObjectsOfTypeAll<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					var layer = layers[i];

					if (layer.Machine == null)
					{
						var layerType = layer.GetType();
						var machine = layer.CachedGameObject.GetOrAddComponent<StateMachine>();

						layer.Destroy();
						AddLayer(machine, layerType, machine);
					}
				}

				for (int i = 0; i < states.Length; i++)
				{
					var state = states[i];

					if (state.Machine == null || state.Layer == null)
					{
						var stateType = state.GetType();
						var machine = state.CachedGameObject.GetOrAddComponent<StateMachine>();

						state.Destroy();
						AddLayer(machine, GetLayerTypeFromState(stateType), machine);
					}
				}

				for (int i = 0; i < callers.Length; i++)
				{
					var caller = callers[i];

					if (caller.machine == null)
						caller.Destroy();
				}
			}
		}

		public static void SetIconAll()
		{
			if (!Application.isPlaying)
			{
				var machines = Resources.FindObjectsOfTypeAll<StateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					var machine = machines[i];
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

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				var assembly = assemblies[i];
				var types = assembly.GetTypes();

				for (int j = 0; j < types.Length; j++)
				{
					var type = types[j];

					if (type.IsSubclassOf(typeof(StateMachine)))
						machineTypes.Add(type);
					else if (type.IsSubclassOf(typeof(StateLayer)))
						layerTypes.Add(type);
					else if (type.IsSubclassOf(typeof(State)))
						stateTypes.Add(type);
				}
			}

			for (int i = 0; i < machineTypes.Count; i++)
			{
				var machineType = machineTypes[i];
				machineFormattedTypeDict[FormatMachine(machineType)] = machineType;
			}

			for (int i = 0; i < layerTypes.Count; i++)
			{
				var layerType = layerTypes[i];
				var layerTypeName = FormatLayer(layerType);

				layerTypeStateTypeDict[layerType] = new List<Type>();
				layerFormattedStateFormattedDict[layerTypeName] = new List<string>();
				layerTypeFormattedDict[layerType] = layerTypeName;
				layerFormattedTypeDict[layerTypeName] = layerType;
			}

			for (int i = 0; i < stateTypes.Count; i++)
			{
				var stateType = stateTypes[i];
				var layerProperty = stateType.GetProperty("Layer", ReflectionExtensions.AllFlags);

				if (layerProperty != null && typeof(IStateLayer).IsAssignableFrom(layerProperty.PropertyType))
				{
					var layerTypeName = FormatLayer(layerProperty.PropertyType);
					var layerTypePrefix = GetLayerPrefix(layerProperty.PropertyType);

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
