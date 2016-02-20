using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Pseudo/General/Entity")]
	public class EntityBehaviour : PMonoBehaviour, IPoolInitializable, IPoolSettersInitializable
	{
		class EntityState
		{
			public bool Active;
			public bool Enabled;
		}

		class ComponentState
		{
			public ComponentBehaviour Component;
			public bool Enabled;
		}

		public IEntity Entity
		{
			get { return entity; }
		}
		public EntityGroups Groups
		{
			get { return entity.Groups; }
			set { entity.Groups = value; }
		}

		[SerializeField]
		EntityGroups groups = null;
		[NonSerialized, InitializeContent]
		EntityBehaviour[] children;
		[InitializeContent]
		IComponent[] components;
		[InitializeContent]
		ComponentBehaviour[] componentBehaviours;
		[DoNotInitialize]
		EntityState entityState;
		[DoNotInitialize]
		ComponentState[] componentStates;

		IEntityManager entityManager;
		IEntity entity;

		void OnEnable()
		{
			if (entity != null)
				entity.Active = true;
		}

		void OnDisable()
		{
			if (entity != null)
				entity.Active = false;
		}

		[Inject]
		public void Initialize(IEntityManager entityManager, IBinder binder)
		{
			this.entityManager = entityManager;

			GatherChildren();
			GatherComponents();

			InitializeChildren(entityManager, binder);
			InitializeComponents(binder);
			CreateEntity();
		}

		public override void OnCreate()
		{
			base.OnCreate();

			GatherChildren();
			GatherComponents();

			for (int i = 0; i < children.Length; i++)
				children[i].OnCreate();

			// Create components bottom to top
			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var component = componentBehaviours[i];
				var poolable = component as IPoolable;

				if (poolable != null)
					poolable.OnCreate();
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			// Recycle components top to bottom
			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var component = componentBehaviours[i];
				var poolable = component as IPoolable;

				if (poolable != null)
					poolable.OnRecycle();
			}

			for (int i = 0; i < children.Length; i++)
				children[i].OnRecycle();

			RecycleEntity();
			ResetStates();
		}

		void CreateEntity()
		{
			entity = entityManager.CreateEntity(groups, enabled);

			for (int i = 0; i < children.Length; i++)
				entity.AddChild(children[i].Entity);

			entity.AddComponents(components);
			entity.AddComponents(componentBehaviours);

			// Activate components
			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var component = componentBehaviours[i];

				if (component.Active && component.Entity != null)
					component.OnActivated();
			}
		}

		void RecycleEntity()
		{
			// Deactivate components
			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var component = componentBehaviours[i];

				if (component.Active && component.Entity != null)
					component.OnDeactivated();
			}

			entityManager.RecycleEntity(entity);
			entity = null;
		}

		void GatherChildren()
		{
			if (children != null)
				return;

			var childList = new List<EntityBehaviour>();
			PopulateChildren(CachedTransform, childList);
			children = childList.ToArray();
			entityState = new EntityState { Active = CachedGameObject.activeSelf, Enabled = enabled };
		}

		void GatherComponents()
		{
			if (components != null && componentBehaviours != null)
				return;

			components = new IComponent[]
			{
				new TransformComponent { Transform = CachedTransform },
				new GameObjectComponent { GameObject = CachedGameObject },
				new BehaviourComponent {Behaviour = this }
			};

			componentBehaviours = GetComponents<ComponentBehaviour>();
			componentStates = new ComponentState[componentBehaviours.Length];

			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var component = componentBehaviours[i];
				componentStates[i] = new ComponentState { Component = component, Enabled = component.enabled };
			}
		}

		void InitializeChildren(IEntityManager entityManager, IBinder binder)
		{
			for (int i = 0; i < children.Length; i++)
				children[i].Initialize(entityManager, binder);
		}

		void InitializeComponents(IBinder binder)
		{
			if (binder != null)
			{
				for (int i = 0; i < componentBehaviours.Length; i++)
					binder.Injector.Inject(componentBehaviours[i]);
			}
		}

		void ResetStates()
		{
			CachedGameObject.SetActive(entityState.Active);
			enabled = entityState.Enabled;

			for (int i = 0; i < componentStates.Length; i++)
			{
				var componentState = componentStates[i];
				componentState.Component.enabled = componentState.Enabled;
			}
		}

		void PopulateChildren(Transform parent, List<EntityBehaviour> entities)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				var child = parent.GetChild(i);
				var entity = child.GetComponent<EntityBehaviour>();

				if (entity == null)
					PopulateChildren(child, entities);
				else
					entities.Add(entity);
			}
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			GatherChildren();
			GatherComponents();
		}

		void IPoolInitializable.OnPostPoolInitialize() { }

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			GatherChildren();
			GatherComponents();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
	}
}