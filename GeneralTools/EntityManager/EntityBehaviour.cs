using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Pseudo/General/Entity")]
	public class EntityBehaviour : PMonoBehaviour, IPoolInitializable, IPoolSettersInitializable
	{
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
		[DoNotInitialize]
		IComponent[] components;
		[InitializeContent]
		IComponent[] componentBehaviours;

		IEntityManager entityManager;
		IEntity entity;
		[DoNotInitialize]
		bool initialized;

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

		[PostInject]
		public void Initialize(IEntityManager entityManager, DiContainer container)
		{
			this.entityManager = entityManager;

			Initialize();

			for (int i = 0; i < children.Length; i++)
				children[i].Initialize(entityManager, container);

			CreateEntity();
			InitializeInjection(container);
		}

		public override void OnCreate()
		{
			base.OnCreate();

			Initialize();

			for (int i = 0; i < children.Length; i++)
				children[i].OnCreate();

			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var poolable = componentBehaviours[i] as IPoolable;

				if (poolable != null)
					poolable.OnCreate();
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			for (int i = 0; i < children.Length; i++)
				children[i].OnRecycle();

			for (int i = 0; i < componentBehaviours.Length; i++)
			{
				var poolable = componentBehaviours[i] as IPoolable;

				if (poolable != null)
					poolable.OnRecycle();
			}

			RecycleEntity();
		}

		void CreateEntity()
		{
			entity = entityManager.CreateEntity(groups, enabled);
			entity.AddComponents(components);
			entity.AddComponents(componentBehaviours);

			for (int i = 0; i < children.Length; i++)
				entity.AddChild(children[i].Entity);
		}

		void RecycleEntity()
		{
			entityManager.RecycleEntity(entity);
			entity = null;
		}

		void Initialize()
		{
			InitializeChildren();
			InitializeComponents();
		}

		void InitializeChildren()
		{
			if (children != null)
				return;

			var childList = new List<EntityBehaviour>();
			FindChildren(CachedTransform, childList);
			children = childList.ToArray();
		}

		void InitializeComponents()
		{
			if (components != null && componentBehaviours != null)
				return;

			components = new IComponent[]
			{
				new TransformComponent { Transform = CachedTransform },
				new GameObjectComponent { GameObject = CachedGameObject }
			};

			componentBehaviours = GetComponents<IComponent>();
		}

		void InitializeInjection(DiContainer container)
		{
			if (!initialized && container != null)
			{
				for (int i = 0; i < componentBehaviours.Length; i++)
					container.Inject(componentBehaviours[i]);

				initialized = true;
			}
		}

		void FindChildren(Transform parent, List<EntityBehaviour> entities)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				var child = parent.GetChild(i);
				var entity = child.GetComponent<EntityBehaviour>();

				if (entity == null)
					FindChildren(child, entities);
				else
					entities.Add(entity);
			}
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			Initialize();
		}

		void IPoolInitializable.OnPostPoolInitialize() { }

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			Initialize();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
	}
}