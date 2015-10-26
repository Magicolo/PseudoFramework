using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

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

		static List<Type> _machineTypes;
		public static List<Type> MachineTypes
		{
			get
			{
				if (_machineTypes == null)
					BuildDicts();

				return _machineTypes;
			}
		}

		static List<Type> _layerTypes;
		public static List<Type> LayerTypes
		{
			get
			{
				if (_layerTypes == null)
					BuildDicts();

				return _layerTypes;
			}
		}

		static List<Type> _stateTypes;
		public static List<Type> StateTypes
		{
			get
			{
				if (_stateTypes == null)
					BuildDicts();

				return _stateTypes;
			}
		}

		static Dictionary<string, Type> _machineFormattedTypeDict;
		public static Dictionary<string, Type> MachineFormattedTypeDict
		{
			get
			{
				if (_machineFormattedTypeDict == null)
					BuildDicts();

				return _machineFormattedTypeDict;
			}
		}

		static Dictionary<Type, List<Type>> _layerTypeStateTypeDict;
		public static Dictionary<Type, List<Type>> LayerTypeStateTypeDict
		{
			get
			{
				if (_layerTypeStateTypeDict == null)
					BuildDicts();

				return _layerTypeStateTypeDict;
			}
		}

		static Dictionary<string, List<string>> _layerFormattedStateFormattedDict;
		public static Dictionary<string, List<string>> LayerFormattedStateFormattedDict
		{
			get
			{
				if (_layerTypeStateTypeDict == null)
					BuildDicts();

				return _layerFormattedStateFormattedDict;
			}
		}

		static Dictionary<Type, string> _layerTypeFormattedDict;
		public static Dictionary<Type, string> LayerTypeFormattedDict
		{
			get
			{
				if (_layerTypeFormattedDict == null)
					BuildDicts();

				return _layerTypeFormattedDict;
			}
		}

		static Dictionary<string, Type> _layerFormattedTypeDict;
		public static Dictionary<string, Type> LayerFormattedTypeDict
		{
			get
			{
				if (_layerFormattedTypeDict == null)
					BuildDicts();

				return _layerFormattedTypeDict;
			}
		}

		static Dictionary<string, Type> _stateNameTypeDict;
		public static Dictionary<string, Type> StateNameTypeDict
		{
			get
			{
				if (_stateNameTypeDict == null)
					BuildDicts();

				return _stateNameTypeDict;
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
			layer = layer == null ? machine.AddComponent(layerType) as StateLayer : layer;
			layer = AddLayer(machine, layer, parent);
#endif

			return layer;
		}

		public static StateLayer AddLayer(StateMachine machine, StateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			layer.hideFlags = HideFlags.HideInInspector;

			UnityEditor.SerializedObject parentSerialized = new UnityEditor.SerializedObject(parent);
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty layersProperty = parentSerialized.FindProperty("_stateReferences");
			UnityEditor.SerializedProperty parentProperty = layerSerialized.FindProperty("_parentReference");

			if (parentProperty.GetValue<UnityEngine.Object>() == null)
				parentProperty.SetValue(parent);

			layerSerialized.FindProperty("_machineReference").SetValue(machine);
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
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("_stateReferences");
			UnityEngine.Object[] states = statesProperty.GetValues<UnityEngine.Object>();

			for (int i = 0; i < states.Length; i++)
			{
				UnityEngine.Object state = states[i];
				StateLayer sublayer = state as StateLayer;

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
			state = state == null ? machine.AddComponent(stateType) as State : state;
			state = AddState(machine, layer, state);
#endif

			return state;
		}

		public static State AddState(StateMachine machine, StateLayer layer, State state)
		{
#if UNITY_EDITOR
			state.hideFlags = HideFlags.HideInInspector;

			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedObject stateSerialized = new UnityEditor.SerializedObject(state);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("_stateReferences");

			stateSerialized.FindProperty("_layerReference").SetValue(layer);
			stateSerialized.FindProperty("_machineReference").SetValue(machine);
			stateSerialized.ApplyModifiedProperties();

			if (!statesProperty.Contains(state))
				statesProperty.Add(state);
#endif

			return state;
		}

		public static void AddMissingStates(StateMachine machine, StateLayer layer)
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("_stateReferences");
			List<Type> stateTypes = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < stateTypes.Count; i++)
			{
				Type stateType = stateTypes[i];

				if (statesProperty != null && !Array.Exists(statesProperty.GetValues<UnityEngine.Object>(), state => state.GetType() == stateType))
					AddState(machine, layer, stateType);
			}
#endif
		}

		public static void MoveLayerTo(StateLayer layer, UnityEngine.Object parent)
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedObject newParentSerialized = new UnityEditor.SerializedObject(parent);
			UnityEditor.SerializedProperty oldParentProperty = layerSerialized.FindProperty("_parentReference");
			UnityEditor.SerializedObject oldParentSerialized = new UnityEditor.SerializedObject(oldParentProperty.GetValue<UnityEngine.Object>());

			oldParentProperty.SetValue(parent);
			oldParentSerialized.FindProperty("_stateReferences").Remove(layer);
			newParentSerialized.FindProperty("_stateReferences").Add(layer);

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

			UnityEditor.SerializedObject stateSerialized = new UnityEditor.SerializedObject(state);
			UnityEngine.Object parentReference = stateSerialized.FindProperty("_layerReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object machineReference = stateSerialized.FindProperty("_machineReference").GetValue<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(stateToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(state);

			stateSerialized = new UnityEditor.SerializedObject(state);
			stateSerialized.FindProperty("_layerReference").SetValue(parentReference);
			stateSerialized.FindProperty("_machineReference").SetValue(machineReference);
#endif
		}

		public static void CopyLayer(StateLayer layer, StateLayer layerToCopy, bool copyStates, bool copySublayers)
		{
#if UNITY_EDITOR
			if (layerToCopy == null)
				return;

			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEngine.Object parentReference = layerSerialized.FindProperty("_parentReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object machineReference = layerSerialized.FindProperty("_machineReference").GetValue<UnityEngine.Object>();
			UnityEngine.Object[] stateReferences = layerSerialized.FindProperty("_stateReferences").GetValues<UnityEngine.Object>();
			UnityEngine.Object[] activeStateReferences = layerSerialized.FindProperty("_activeStateReferences").GetValues<UnityEngine.Object>();

			UnityEditorInternal.ComponentUtility.CopyComponent(layerToCopy);
			UnityEditorInternal.ComponentUtility.PasteComponentValues(layer);

			layerSerialized = new UnityEditor.SerializedObject(layer);
			layerSerialized.FindProperty("_parentReference").SetValue(parentReference);
			layerSerialized.FindProperty("_machineReference").SetValue(machineReference);
			layerSerialized.FindProperty("_stateReferences").SetValues(stateReferences);
			layerSerialized.FindProperty("_activeStateReferences").SetValues(activeStateReferences);

			for (int i = 0; i < stateReferences.Length; i++)
			{
				UnityEngine.Object stateReference = stateReferences[i];
				State state = stateReference as State;
				StateLayer sublayer = stateReference as StateLayer;

				if (copyStates && state != null)
				{
					State stateToCopy = layerToCopy.GetState(state.GetType()) as State;

					if (stateToCopy != null)
						CopyState(state, stateToCopy);
				}

				if (copySublayers && sublayer != null)
				{
					StateLayer sublayerToCopy = layerToCopy.GetState(sublayer.GetType()) as StateLayer;

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
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			isParent = layerSerialized.FindProperty("_parentReference").GetValue<UnityEngine.Object>() == parent;
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
			string formattedLayer = layerType.GetName().SplitWords(2).Concat("/");

			PropertyInfo machineProperty = layerType.GetProperty("Machine", ObjectExtensions.AllFlags);
			PropertyInfo layerProperty = layerType.GetProperty("Layer", ObjectExtensions.AllFlags);

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
			List<StateLayer> subLayers = new List<StateLayer>();

#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty subLayersProperty = layerSerialized.FindProperty("_stateReferences");

			for (int i = 0; i < subLayersProperty.arraySize; i++)
			{
				StateLayer subLayer = subLayersProperty.GetValue(i) as StateLayer;

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
			StateLayer[] layers = machine.GetComponents<StateLayer>();

			for (int i = 0; i < layers.Length; i++)
				UpdateLayerStates(machine, layers[i]);
		}

		public static void UpdateLayerStates(StateMachine machine, StateLayer layer)
		{
			List<Type> types = LayerTypeStateTypeDict[layer.GetType()];

			for (int i = 0; i < types.Count; i++)
				AddState(machine, layer, types[i]);
		}

		public static void UpdateCallbacks(StateMachine machine)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			int callerMask = 0;
			StateLayer[] layers = machine.GetComponents<StateLayer>();
			State[] states = machine.GetComponents<State>();

			for (int i = 0; i < layers.Length; i++)
			{
				StateLayer layer = layers[i];
				Type layerType = layer.GetType();

				while (layerType != typeof(StateLayer) || !typeof(StateLayer).IsAssignableFrom(layerType))
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
				State state = states[i];
				Type stateType = state.GetType();

				while (stateType != typeof(State) || !typeof(State).IsAssignableFrom(stateType))
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
					StateMachineCaller caller = machine.GetOrAddComponent(CallbackTypes[i]) as StateMachineCaller;

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
				StateMachine[] machines = Resources.FindObjectsOfTypeAll<StateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					StateMachine machine = machines[i];
					UpdateCallbacks(machine);
					UpdateLayerStates(machine);
				}
			}
		}

		public static void CleanUp(StateMachine machine, GameObject gameObject)
		{
			if (!Application.isPlaying && gameObject != null)
			{
				StateLayer[] layers = gameObject.GetComponents<StateLayer>();
				State[] states = gameObject.GetComponents<State>();
				StateMachineCaller[] callers = gameObject.GetComponents<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					StateLayer layer = layers[i];

					if (machine == null || layer.Machine == null || !object.ReferenceEquals(layer.Machine, machine) || layer.gameObject != gameObject)
						layer.Destroy();
				}

				for (int i = 0; i < states.Length; i++)
				{
					State state = states[i];

					if (machine == null || state.Machine == null || !object.ReferenceEquals(state.Machine, machine) || state.gameObject != gameObject || state.Layer == null)
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
				StateLayer[] layers = Resources.FindObjectsOfTypeAll<StateLayer>();
				State[] states = Resources.FindObjectsOfTypeAll<State>();
				StateMachineCaller[] callers = Resources.FindObjectsOfTypeAll<StateMachineCaller>();

				for (int i = 0; i < layers.Length; i++)
				{
					StateLayer layer = layers[i];

					if (layer.Machine == null)
					{
						Type layerType = layer.GetType();
						StateMachine machine = layer.GetOrAddComponent<StateMachine>();

						layer.Destroy();
						AddLayer(machine, layerType, machine);
					}
				}

				for (int i = 0; i < states.Length; i++)
				{
					State state = states[i];

					if (state.Machine == null || state.Layer == null)
					{
						Type stateType = state.GetType();
						StateMachine machine = state.GetOrAddComponent<StateMachine>();

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
				StateMachine[] machines = Resources.FindObjectsOfTypeAll<StateMachine>();

				for (int i = 0; i < machines.Length; i++)
				{
					StateMachine machine = machines[i];
					UpdateCallbacks(machine);
					UpdateLayerStates(machine);
				}
			}
		}

		public static void BuildDicts()
		{
			_machineTypes = new List<Type>();
			_layerTypes = new List<Type>();
			_stateTypes = new List<Type>();
			_machineFormattedTypeDict = new Dictionary<string, Type>();
			_layerTypeStateTypeDict = new Dictionary<Type, List<Type>>();
			_layerFormattedStateFormattedDict = new Dictionary<string, List<string>>();
			_layerTypeFormattedDict = new Dictionary<Type, string>();
			_layerFormattedTypeDict = new Dictionary<string, Type>();
			_stateNameTypeDict = new Dictionary<string, Type>();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly = assemblies[i];
				Type[] types = assembly.GetTypes();

				for (int j = 0; j < types.Length; j++)
				{
					Type type = types[j];

					if (type.IsSubclassOf(typeof(StateMachine)))
						_machineTypes.Add(type);
					else if (type.IsSubclassOf(typeof(StateLayer)))
						_layerTypes.Add(type);
					else if (type.IsSubclassOf(typeof(State)))
						_stateTypes.Add(type);
				}
			}

			for (int i = 0; i < _machineTypes.Count; i++)
			{
				Type machineType = _machineTypes[i];
				_machineFormattedTypeDict[FormatMachine(machineType)] = machineType;
			}

			for (int i = 0; i < _layerTypes.Count; i++)
			{
				Type layerType = _layerTypes[i];
				string layerTypeName = FormatLayer(layerType);

				_layerTypeStateTypeDict[layerType] = new List<Type>();
				_layerFormattedStateFormattedDict[layerTypeName] = new List<string>();
				_layerTypeFormattedDict[layerType] = layerTypeName;
				_layerFormattedTypeDict[layerTypeName] = layerType;
			}

			for (int i = 0; i < _stateTypes.Count; i++)
			{
				Type stateType = _stateTypes[i];
				PropertyInfo layerProperty = stateType.GetProperty("Layer", ObjectExtensions.AllFlags);

				if (layerProperty != null && typeof(IStateLayer).IsAssignableFrom(layerProperty.PropertyType))
				{
					string layerTypeName = FormatLayer(layerProperty.PropertyType);
					string layerTypePrefix = GetLayerPrefix(layerProperty.PropertyType);

					_layerTypeStateTypeDict[layerProperty.PropertyType].Add(stateType);
					_layerFormattedStateFormattedDict[layerTypeName].Add(FormatState(stateType, layerTypePrefix));
				}

				_stateNameTypeDict[stateType.GetName()] = stateType;
			}
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts()
		{
			_layerTypeStateTypeDict = null;
			_layerFormattedStateFormattedDict = null;
			_layerTypeFormattedDict = null;
			_layerFormattedTypeDict = null;
			_stateNameTypeDict = null;

			CleanUpAll();
			UpdateAll();
			SetIconAll();
		}
#endif
	}
}
