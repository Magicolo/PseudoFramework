using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract class StateLayer : PMonoBehaviour, IStateLayer
	{

		public IStateLayer Layer { get { return _parentReference as IStateLayer; } }
		public IStateMachine Machine { get { return _machineReference; } }

		bool _isActive;
		public bool IsActive { get { return _isActive; } }

		[SerializeField]
		Object _parentReference = null;
		[SerializeField]
		StateMachine _machineReference = null;
		[SerializeField]
		Object[] _stateReferences = new Object[0];
		[SerializeField]
		List<Object> _activeStateReferences = new List<Object> { null };

		IState[] _states = new IState[0];
		IState[] _activeStates = new IState[0];

		Dictionary<string, IState> _nameStateDict;
		Dictionary<string, IState> NameStateDict
		{
			get
			{
				if (_nameStateDict == null || !Application.isPlaying)
					BuildStateDict();

				return _nameStateDict;
			}
		}

		public virtual void OnEnter()
		{
			_isActive = true;

			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].OnEnter();
		}

		public virtual void OnExit()
		{
			_isActive = false;

			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].OnExit();
		}

		public virtual void OnAwake()
		{
			BuildActiveStates();
			BuildStateDict();

			for (int i = 0; i < _states.Length; i++)
				_states[i].OnAwake();
		}

		public virtual void OnStart()
		{
			for (int i = 0; i < _states.Length; i++)
				_states[i].OnStart();
		}

		public virtual void OnUpdate()
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].OnUpdate();
		}

		public virtual void OnFixedUpdate()
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].OnFixedUpdate();
		}

		public virtual void OnLateUpdate()
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].OnLateUpdate();
		}

		public virtual void CollisionEnter(Collision collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionEnter(collision);
		}

		public virtual void CollisionStay(Collision collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionStay(collision);
		}

		public virtual void CollisionExit(Collision collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionExit(collision);
		}

		public virtual void CollisionEnter2D(Collision2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionEnter2D(collision);
		}

		public virtual void CollisionStay2D(Collision2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionStay2D(collision);
		}

		public virtual void CollisionExit2D(Collision2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].CollisionExit2D(collision);
		}

		public virtual void TriggerEnter(Collider collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerEnter(collision);
		}

		public virtual void TriggerStay(Collider collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerStay(collision);
		}

		public virtual void TriggerExit(Collider collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerExit(collision);
		}

		public virtual void TriggerEnter2D(Collider2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerEnter2D(collision);
		}

		public virtual void TriggerStay2D(Collider2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerStay2D(collision);
		}

		public virtual void TriggerExit2D(Collider2D collision)
		{
			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i].TriggerExit2D(collision);
		}

		public T SwitchState<T>(int index = 0) where T : IState
		{
			IState state = SwitchState(GetState(typeof(T).Name), index);

			return state is T ? (T)state : default(T);
		}

		public IState SwitchState(System.Type stateType, int index = 0)
		{
			return SwitchState(GetState(stateType), index);
		}

		public IState SwitchState(string stateName, int index = 0)
		{
			return SwitchState(GetState(stateName), index);
		}

		public IState[] SwitchStates<T>(params int[] indices) where T : IState
		{
			return SwitchStates(typeof(T).Name, indices);
		}

		public IState[] SwitchStates(System.Type stateType, params int[] indices)
		{
			return SwitchStates(stateType.Name, indices);
		}

		public IState[] SwitchStates(string stateName, params int[] indices)
		{
			IState[] switchedStates = new IState[indices.Length];

			if (indices == null || indices.Length == 0)
			{
				indices = new int[_activeStates.Length];

				for (int i = 0; i < _activeStates.Length; i++)
					indices[i] = i;
			}

			for (int i = 0; i < indices.Length; i++)
			{
				int index = indices[i];
				switchedStates[i] = SwitchState(stateName, index);
			}

			return switchedStates;
		}

		public bool StateIsActive<T>(int index = 0) where T : IState
		{
			return StateIsActive(typeof(T).Name, index);
		}

		public bool StateIsActive(System.Type stateType, int index = 0)
		{
			return StateIsActive(stateType.Name, index);
		}

		public bool StateIsActive(string stateName, int index = 0)
		{
			return GetActiveState(index) == GetState(stateName);
		}

		public T GetActiveState<T>(int index = 0) where T : IState
		{
			IState activeState = GetActiveState(index);

			return activeState is T ? (T)activeState : default(T);
		}

		public IState GetActiveState(int index = 0)
		{
			IState activeState = null;

			try
			{
				activeState = _activeStates[index] ?? EmptyState.Instance;
			}
			catch
			{
				Debug.LogError(string.Format("State was not found at index {0}.", index));
			}

			return activeState;
		}

		public IState[] GetActiveStates()
		{
			return _activeStates;
		}

		public T GetState<T>() where T : IState
		{
			IState state = GetState(typeof(T).Name);

			return state is T ? (T)state : default(T);
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
				state = NameStateDict[stateName];
			}
			catch
			{
				Debug.LogError(string.Format("State named {0} was not found.", stateName));
			}

			return state;
		}

		public IState[] GetStates()
		{
			return _states;
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

		IState SwitchState(IState state, int index = 0)
		{
			IState activeState = GetActiveState(index);
			state = state ?? EmptyState.Instance;
			_activeStates[index] = state;
			_activeStateReferences[index] = state as Object;

			if (IsActive)
			{
				activeState.OnExit();
				state.OnEnter();
			}

			return state;
		}

		void BuildActiveStates()
		{
			_activeStates = new IState[_activeStateReferences.Count];

			for (int i = 0; i < _activeStates.Length; i++)
				_activeStates[i] = _activeStateReferences[i] as IState ?? EmptyState.Instance;
		}

		void BuildStateDict()
		{
			_nameStateDict = new Dictionary<string, IState>();
			_states = new IState[_stateReferences.Length];

			_nameStateDict[EmptyState.Instance.GetType().Name] = EmptyState.Instance;
			_nameStateDict[StateMachineUtility.FormatState(EmptyState.Instance.GetType(), "")] = EmptyState.Instance;

			for (int i = 0; i < _stateReferences.Length; i++)
			{
				IState state = (IState)_stateReferences[i];

				if (state != null)
				{
					_nameStateDict[state.GetType().Name] = state;
					_nameStateDict[state is StateLayer ? state.GetType().Name : StateMachineUtility.FormatState(state.GetType(), state.Layer.GetType())] = state;
					_states[i] = state;
				}
			}
		}
	}
}

