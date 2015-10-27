using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract class PState : PMonoBehaviour, IState
	{
		public virtual IStateLayer Layer { get { return layerReference; } }
		public virtual IStateMachine Machine { get { return machineReference; } }

		bool isActive;
		public bool IsActive { get { return isActive; } }

		[SerializeField]
		PStateLayer layerReference = null;
		[SerializeField]
		PStateMachine machineReference = null;

		public virtual void OnEnter()
		{
			isActive = true;
		}

		public virtual void OnExit()
		{
			isActive = false;
		}

		public virtual void OnAwake()
		{
		}

		public virtual void OnStart()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnLateUpdate()
		{
		}

		public virtual void CollisionEnter(Collision collision)
		{
		}

		public virtual void CollisionStay(Collision collision)
		{
		}

		public virtual void CollisionExit(Collision collision)
		{
		}

		public virtual void CollisionEnter2D(Collision2D collision)
		{
		}

		public virtual void CollisionStay2D(Collision2D collision)
		{
		}

		public virtual void CollisionExit2D(Collision2D collision)
		{
		}

		public virtual void TriggerEnter(Collider collision)
		{
		}

		public virtual void TriggerStay(Collider collision)
		{
		}

		public virtual void TriggerExit(Collider collision)
		{
		}

		public virtual void TriggerEnter2D(Collider2D collision)
		{
		}

		public virtual void TriggerStay2D(Collider2D collision)
		{
		}

		public virtual void TriggerExit2D(Collider2D collision)
		{
		}

		public T SwitchState<T>(int index = 0) where T : IState
		{
			return Layer.SwitchState<T>(index);
		}

		public IState SwitchState(System.Type stateType, int index = 0)
		{
			return Layer.SwitchState(stateType.Name, index);
		}

		public IState SwitchState(string stateName, int index = 0)
		{
			return Layer.SwitchState(stateName, index);
		}

		public IState[] SwitchStates<T>(params int[] indices) where T : IState
		{
			return Layer.SwitchStates<T>(indices);
		}

		public IState[] SwitchStates(System.Type stateType, params int[] indices)
		{
			return Layer.SwitchStates(stateType, indices);
		}

		public IState[] SwitchStates(string stateName, params int[] indices)
		{
			return Layer.SwitchStates(stateName, indices);
		}

		public bool StateIsActive<T>(int index = 0) where T : IState
		{
			return Layer.StateIsActive<T>(index);
		}

		public bool StateIsActive(System.Type stateType, int index = 0)
		{
			return Layer.StateIsActive(stateType, index);
		}

		public bool StateIsActive(string stateName, int index = 0)
		{
			return Layer.StateIsActive(stateName, index);
		}

		public T GetActiveState<T>(int index = 0) where T : IState
		{
			return Layer.GetActiveState<T>(index);
		}

		public IState GetActiveState(int index = 0)
		{
			return Layer.GetActiveState(index);
		}

		public IState[] GetActiveStates()
		{
			return Layer.GetActiveStates();
		}

		public T GetState<T>() where T : IState
		{
			return Layer.GetState<T>();
		}

		public IState GetState(System.Type stateType)
		{
			return Layer.GetState(stateType);
		}

		public IState GetState(string stateName)
		{
			return Layer.GetState(stateName);
		}

		public IState[] GetStates()
		{
			return Layer.GetStates();
		}

		public bool ContainsState<T>() where T : IState
		{
			return Layer.ContainsState<T>();
		}

		public bool ContainsState(System.Type stateType)
		{
			return Layer.ContainsState(stateType);
		}

		public bool ContainsState(string stateName)
		{
			return Layer.ContainsState(stateName);
		}
	}
}
