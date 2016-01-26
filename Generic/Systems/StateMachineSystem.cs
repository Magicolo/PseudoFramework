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
		public static readonly StateMachineEvents OnStateSwitch = new StateMachineEvents(1);
		public static readonly StateMachineEvents OnStateEnter = new StateMachineEvents(2);
		public static readonly StateMachineEvents OnStateExit = new StateMachineEvents(3);

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
			EventManager.Subscribe(StateMachineEvents.OnStateSwitch, (Action<IEntity, int>)OnStateSwitch);
			entities.OnEntityAdded += OnEntityAdded;
			entities.OnEntityRemoved += OnEntityRemove;

			for (int i = 0; i < entities.Count; i++)
				OnEntityAdded(entities[i]);
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			EventManager.UnsubscribeAll((Action<Events, IEntity>)OnEvent);
			EventManager.Unsubscribe(StateMachineEvents.OnStateSwitch, (Action<IEntity, int>)OnStateSwitch);
			entities.OnEntityAdded -= OnEntityAdded;
			entities.OnEntityRemoved -= OnEntityRemove;

			for (int i = 0; i < entities.Count; i++)
				OnEntityRemove(entities[i]);
		}

		void OnEntityAdded(IEntity entity)
		{
			var stateMachine = entity.GetComponent<StateMachineComponent>();

			for (int i = 0; i < stateMachine.States.Length; i++)
			{
				var stateEntity = stateMachine.States[i];
				var state = stateEntity.GetComponent<StateComponent>();

				state.StateMachine = entity;
				stateEntity.CachedGameObject.SetActive(false);
			}

			EventManager.TriggerImmediate(StateMachineEvents.OnStateSwitch, entity, stateMachine.InitialStateIndex);
		}

		void OnEntityRemove(IEntity entity)
		{
			EventManager.TriggerImmediate(StateMachineEvents.OnStateSwitch, entity, -1);
		}

		void OnStateSwitch(IEntity entity, int stateIndex)
		{
			if (!entities.Contains(entity))
				return;

			var stateMachine = entity.GetComponent<StateMachineComponent>();

			if (stateMachine.CurrentState != null)
			{
				EventManager.Trigger(StateMachineEvents.OnStateExit, entity, stateMachine.CurrentState.Entity);
				stateMachine.CurrentState.CachedGameObject.SetActive(false);
				stateMachine.CurrentState = null;
			}

			if (stateIndex >= 0)
			{
				var stateEntity = stateMachine.States[stateIndex];

				stateEntity.CachedGameObject.SetActive(true);
				stateMachine.CurrentState = stateEntity;
				EventManager.Trigger(StateMachineEvents.OnStateEnter, entity, stateMachine.CurrentState.Entity);
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