using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class StateMachineEvents : PEnum<StateMachineEvents, int>
	{
		public static readonly StateMachineEvents OnSwitchState = new StateMachineEvents(1);
		public static readonly StateMachineEvents OnStateEntered = new StateMachineEvents(2);
		public static readonly StateMachineEvents OnStateExited = new StateMachineEvents(3);

		protected StateMachineEvents(int value) : base(value) { }
	}

	public class StateMachineSystem : SystemBase
	{
		IEntityGroup entities;

		public override void OnInitialize()
		{
			base.OnInitialize();

			entities = EntityManager.Entities.Filter(typeof(StateMachineComponent));
		}

		public override void OnActivate()
		{
			base.OnActivate();

			EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
			EventManager.Subscribe(StateMachineEvents.OnSwitchState, (Action<IEntity, int>)OnSwitchState);
			entities.OnEntityAdded += OnEntityAdded;
			entities.OnEntityRemoved += OnEntityRemove;

			for (int i = 0; i < entities.Count; i++)
				OnEntityAdded(entities[i]);
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			EventManager.UnsubscribeAll((Action<Events, IEntity>)OnEvent);
			EventManager.Unsubscribe(StateMachineEvents.OnSwitchState, (Action<IEntity, int>)OnSwitchState);
			entities.OnEntityAdded -= OnEntityAdded;
			entities.OnEntityRemoved -= OnEntityRemove;

			for (int i = 0; i < entities.Count; i++)
				OnEntityRemove(entities[i]);
		}

		void OnEntityAdded(IEntity entity)
		{
			var stateMachine = entity.GetComponent<StateMachineComponent>();

			EventManager.TriggerImmediate(StateMachineEvents.OnSwitchState, entity, stateMachine.InitialStateIndex);
		}

		void OnEntityRemove(IEntity entity)
		{
			EventManager.TriggerImmediate(StateMachineEvents.OnSwitchState, entity, -1);
		}

		void OnSwitchState(IEntity entity, int stateIndex)
		{
			if (!entities.Contains(entity))
				return;

			var stateMachine = entity.GetComponent<StateMachineComponent>();

			if (stateMachine.CurrentState != null)
			{
				EntityManager.RecycleEntity(stateMachine.CurrentState);
				EventManager.Trigger(StateMachineEvents.OnStateExited, entity);
			}

			if (stateIndex >= 0)
			{
				var stateEntity = EntityManager.CreateEntity(stateMachine.States[stateIndex]);
				var state = stateEntity.Entity.GetComponent<StateComponent>();

				state.StateMachine = entity;
				stateEntity.CachedTransform.parent = stateMachine.CachedTransform;
				stateMachine.CurrentState = stateEntity;
				EventManager.Trigger(StateMachineEvents.OnStateEntered, entity);
			}
		}

		void OnEvent(Events identifier, IEntity entity)
		{
			if (!entities.Contains(entity))
				return;

			var stateMachine = entity.GetComponent<StateMachineComponent>();

			if (stateMachine.CurrentState != null)
				EventManager.Trigger(identifier, stateMachine.CurrentState.Entity);
		}
	}
}