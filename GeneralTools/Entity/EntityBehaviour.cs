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
	public class EntityBehaviour : PMonoBehaviour, IPoolSettersInitializable
	{
		public IEntity Entity
		{
			get { return entity; }
		}
		public EntityGroups Groups
		{
			get { return groups; }
		}

		[SerializeField]
		EntityGroups groups = EntityGroups.Nothing;
		[InitializeContent]
		IComponent[] components;
		IEntity entity;
		IEntityManager entityManager;

		void Awake()
		{
			components = GetComponents<IComponent>();
		}

		[PostInject]
		public void Initialize(IEntityManager entityManager)
		{
			this.entityManager = entityManager;
			entity = entityManager.CreateEntity(groups);
			entity.AddComponents(components ?? (components = GetComponents<IComponent>()));
		}

		public void Recycle()
		{
			entityManager.RecycleEntity(this);
		}

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			components = GetComponents<IComponent>();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
	}
}