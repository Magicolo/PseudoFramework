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

		public IList<IComponent> Components
		{
			get { return entity.Components; }
		}

		[SerializeField]
		EntityGroups groups = null;
		[NonSerialized, InitializeContent]
		EntityBehaviour[] children;
		[InitializeContent]
		IComponent[] components;
		[InitializeContent]
		IComponent[] componentBehaviours;

		IEntityManager entityManager;
		IEntity entity;

		void OnEnable()
		{
			Initialize();
			CreateEntity();
		}

		void OnDisable()
		{
			RecycleEntity();
		}

		[PostInject]
		public void Initialize(IEntityManager entityManager)
		{
			this.entityManager = entityManager;

			Initialize();

			for (int i = 0; i < children.Length; i++)
				children[i].Initialize(entityManager);

			CreateEntity();
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
			if (entityManager == null || entity != null || !CachedGameObject.activeInHierarchy || !enabled)
				return;

			entity = entityManager.CreateEntity(groups);
			entity.AddComponents(components);
			entity.AddComponents(componentBehaviours);
		}

		void RecycleEntity()
		{
			if (entityManager == null || entity == null)
				return;

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
			FindEntities(CachedTransform, childList);
			children = childList.ToArray();

			bool isRoot = CachedGameObject.GetComponentInParent<EntityBehaviour>(true) == null;

			if (isRoot)
				children = CachedGameObject.GetComponentsInChildren<EntityBehaviour>(true, true);
			else
				children = new EntityBehaviour[0];
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

		void FindEntities(Transform parent, List<EntityBehaviour> entities)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				var child = parent.GetChild(i);
				var entity = child.GetComponent<EntityBehaviour>();

				if (entity == null)
					FindEntities(child, entities);
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