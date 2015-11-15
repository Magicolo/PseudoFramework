using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/State Machine")]
	public class StateMachine : PMonoBehaviour, IStateMachine
	{
		bool isActive;
		public bool IsActive { get { return isActive; } }

		[SerializeField]
		Object[] stateReferences = new Object[0];
		IState[] states = new IState[0];
		IStateLayer[] layers = new IStateLayer[0];
		IStateLayer[] activeLayers = new IStateLayer[0];

		Dictionary<string, IStateLayer> nameLayerDict;
		Dictionary<string, IStateLayer> NameLayerDict
		{
			get
			{
				if (nameLayerDict == null)
					BuildLayerDict();

				return nameLayerDict;
			}
		}

		Dictionary<string, IState> nameStateDict;
		Dictionary<string, IState> NameStateDict
		{
			get
			{
				if (nameStateDict == null)
					BuildStateDict();

				return nameStateDict;
			}
		}

		bool initialized;

		void OnEnable()
		{
			if (!initialized)
				Awake();

			isActive = true;

			OnEnter();
		}

		void OnDisable()
		{
			isActive = false;

			OnExit();
		}

		void Awake()
		{
			if (!initialized)
			{
				BuildLayerDict();

				OnAwake();

				initialized = true;
			}
		}

		void Start()
		{
			OnStart();
		}

		public virtual void OnEnter()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnEnter();
		}

		public virtual void OnExit()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnExit();
		}

		public virtual void OnAwake()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnAwake();
		}

		public virtual void OnStart()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnStart();
		}

		public virtual void OnUpdate()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnUpdate();
		}

		public virtual void OnFixedUpdate()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnFixedUpdate();
		}

		public virtual void OnLateUpdate()
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].OnLateUpdate();
		}

		public virtual void CollisionEnter(Collision collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionEnter(collision);
		}

		public virtual void CollisionStay(Collision collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionStay(collision);
		}

		public virtual void CollisionExit(Collision collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionExit(collision);
		}

		public virtual void CollisionEnter2D(Collision2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionEnter2D(collision);
		}

		public virtual void CollisionStay2D(Collision2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionStay2D(collision);
		}

		public virtual void CollisionExit2D(Collision2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].CollisionExit2D(collision);
		}

		public virtual void TriggerEnter(Collider collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerEnter(collision);
		}

		public virtual void TriggerStay(Collider collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerStay(collision);
		}

		public virtual void TriggerExit(Collider collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerExit(collision);
		}

		public virtual void TriggerEnter2D(Collider2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerEnter2D(collision);
		}

		public virtual void TriggerStay2D(Collider2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerStay2D(collision);
		}

		public virtual void TriggerExit2D(Collider2D collision)
		{
			for (int i = 0; i < activeLayers.Length; i++)
				activeLayers[i].TriggerExit2D(collision);
		}

		public T GetState<T>() where T : IState
		{
			return (T)GetState(typeof(T).Name);
		}

		public IState GetState(System.Type stateType)
		{
			return GetState(stateType.Name);
		}

		public IState GetState(string stateName)
		{
			IState state = null;

			try
			{
				state = NameLayerDict[stateName];
			}
			catch
			{
				Debug.LogError(string.Format("State named {0} was not found.", stateName));
			}

			return state;
		}

		public IState[] GetStates()
		{
			return states;
		}

		public bool ContainsState<T>() where T : IState
		{
			return ContainsState(typeof(T).Name);
		}

		public bool ContainsState(System.Type stateType)
		{
			return ContainsState(stateType.Name);
		}

		public bool ContainsState(string stateName)
		{
			return NameStateDict.ContainsKey(stateName);
		}

		public T GetLayer<T>() where T : IStateLayer
		{
			return (T)GetLayer(typeof(T).Name);
		}

		public IStateLayer GetLayer(System.Type layerType)
		{
			return GetLayer(layerType.Name);
		}

		public IStateLayer GetLayer(string layerName)
		{
			IStateLayer layer = null;

			try
			{
				layer = NameLayerDict[layerName];
			}
			catch
			{
				Debug.LogError(string.Format("Layer named {0} was not found.", layerName));
			}

			return layer;
		}

		public IStateLayer[] GetLayers()
		{
			return layers;
		}

		public bool ContainsLayer<T>() where T : IStateLayer
		{
			return ContainsLayer(typeof(T).Name);
		}

		public bool ContainsLayer(System.Type layerType)
		{
			return ContainsLayer(layerType.Name);
		}

		public bool ContainsLayer(string layerName)
		{
			return NameLayerDict.ContainsKey(layerName);
		}

		void BuildStateDict()
		{
			nameStateDict = new Dictionary<string, IState>();
			states = GetComponents<State>();

			for (int i = 0; i < states.Length; i++)
			{
				IState state = states[i];
				nameStateDict[state.GetType().Name] = state;
				nameStateDict[StateMachineUtility.FormatState(state.GetType(), state.Layer.GetType())] = state;
			}
		}

		void BuildLayerDict()
		{
			nameStateDict = new Dictionary<string, IState>();
			nameLayerDict = new Dictionary<string, IStateLayer>();
			layers = GetComponents<StateLayer>();
			activeLayers = new IStateLayer[stateReferences.Length];

			for (int i = 0; i < stateReferences.Length; i++)
			{
				IStateLayer layer = (IStateLayer)stateReferences[i];

				if (layer != null)
					activeLayers[i] = layer;
			}

			for (int i = 0; i < layers.Length; i++)
			{
				IStateLayer layer = layers[i];
				nameStateDict[layer.GetType().Name] = layer;
				nameStateDict[StateMachineUtility.FormatLayer(layer.GetType())] = layer;
			}
		}

		protected virtual void Reset()
		{
			StateMachineUtility.CleanUp(null, CachedGameObject);
		}
	}
}
