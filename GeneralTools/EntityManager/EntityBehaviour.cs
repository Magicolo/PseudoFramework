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
	public class EntityBehaviour : PMonoBehaviour, ISceneInjectable, IPoolable, IPoolInitializable, IPoolFieldsInitializable
	{
		class EntityState
		{
			public bool Active;
			public bool Enabled;
			public bool[] ComponentStates;
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
		public bool IsRoot
		{
			get { return parent == null; }
		}

		/// <summary>
		/// Groups that the linked Entity will have.
		/// </summary>
		[SerializeField]
		EntityGroups groups = null;
		/// <summary>
		/// Needed to determine if EntityBehaviour is the root of its hierarchy.
		/// </summary>
		[NonSerialized, DoNotInitialize]
		EntityBehaviour parent;
		/// <summary>
		/// Needed to build the Entity hierachy.
		/// Initialize content because pool will start initializing from root.
		/// </summary>
		[NonSerialized, InitializeContent]
		EntityBehaviour[] children;
		/// <summary>
		/// Default components.
		/// No need to initialize because they are immutable.
		/// </summary>
		[DoNotInitialize]
		IComponent[] components;
		/// <summary>
		/// Cached components on the same GameObject.
		/// Initialize their content to reset their state when pooled.
		/// </summary>
		[InitializeContent]
		ComponentBehaviour[] componentBehaviours;
		/// <summary>
		/// Some information about the initial state of the EntityBehaviour and its components.
		/// Used to reset the EntityBehaviour and its components to their initial state.
		/// </summary>
		[DoNotInitialize]
		EntityState initialState;

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

		void OnDestroy()
		{
			Recycle(false);
		}

		public void Initialize(IEntityManager entityManager, IBinder binder)
		{
			Initialize(entityManager, binder, true);
		}

		void CreateEntity()
		{
			if (entity != null)
				RecycleEntity();

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
			if (entity == null)
				return;

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

		void Initialize(IEntityManager entityManager, IBinder binder, bool inject, bool fromRoot = false)
		{
			this.entityManager = entityManager;

			InitializeHierarchyIfNeeded();
			InitializeComponentsIfNeeded();

			if (inject)
			{
				for (int i = 0; i < componentBehaviours.Length; i++)
					binder.Injector.Inject(componentBehaviours[i]);
			}

			// Initialize children only if this call comes from a root EntityBehaviour to ensure proper initialization of the Entity hierarchy.
			if (fromRoot || IsRoot)
			{
				// Initialize bottom up
				for (int i = 0; i < children.Length; i++)
					children[i].Initialize(entityManager, binder, inject, true);

				CreateEntity();
			}
		}

		void InitializeHierarchyIfNeeded()
		{
			if (children != null)
				return;

			parent = CachedGameObject.GetComponentInParent<EntityBehaviour>(true);
			var childList = new List<EntityBehaviour>();
			PopulateChildren(CachedTransform, childList);
			children = childList.ToArray();
		}

		void InitializeComponentsIfNeeded()
		{
			if (components != null && componentBehaviours != null)
				return;

			components = new IComponent[]
			{
				new TransformComponent(CachedTransform),
				new GameObjectComponent(CachedGameObject),
				new BehaviourComponent(this)
			};

			componentBehaviours = GetComponents<ComponentBehaviour>();
			SaveInitialState();
		}

		void Recycle(bool resetState)
		{
			// Recycle from bottom to top
			for (int i = 0; i < children.Length; i++)
				children[i].Recycle(resetState);

			RecycleEntity();

			if (resetState)
				ResetToInitialState();
		}

		void SaveInitialState()
		{
			initialState = new EntityState
			{
				Active = CachedGameObject.activeSelf,
				Enabled = enabled,
				ComponentStates = componentBehaviours.Convert(c => c.enabled)
			};

		}

		void ResetToInitialState()
		{
			CachedGameObject.SetActive(initialState.Active);
			enabled = initialState.Enabled;

			for (int i = 0; i < initialState.ComponentStates.Length; i++)
				componentBehaviours[i].enabled = initialState.ComponentStates[i];
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

		void ISceneInjectable.OnPreSceneInject(IBinder binder) { }

		void ISceneInjectable.OnPostSceneInject(IBinder binder)
		{
			Initialize(binder.Resolver.Resolve<IEntityManager>(), binder, false);
		}

		void IPoolable.OnCreate() { }

		void IPoolable.OnRecycle()
		{
			Recycle(true);
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			InitializeHierarchyIfNeeded();
			InitializeComponentsIfNeeded();
		}

		void IPoolInitializable.OnPostPoolInitialize() { }

		void IPoolFieldsInitializable.OnPrePoolFieldsInitialize(IFieldInitializer initializer)
		{
			InitializeHierarchyIfNeeded();
			InitializeComponentsIfNeeded();
		}

		void IPoolFieldsInitializable.OnPostPoolFieldsInitialize(IFieldInitializer initializer) { }
	}
}