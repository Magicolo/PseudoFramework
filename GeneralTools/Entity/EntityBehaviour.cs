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
	[AddComponentMenu("Pseudo/Entity3")]
	public class EntityBehaviour : PMonoBehaviour, IPoolSettersInitializable
	{
		public IEntity Entity { get; private set; }
		public EntityGroups Groups
		{
			get { return groups; }
		}

		[SerializeField]
		EntityGroups groups = EntityGroups.Nothing;
		[InitializeContent]
		IComponent[] components;
		IEntityManager entityManager = null;

		[PostInject]
		public void Initialize(IEntityManager entityManager, IEntity entity)
		{
			this.entityManager = entityManager;
			Entity = entity;
			Entity.AddComponents(components ?? (components = GetComponents<IComponent>()));
		}

		public void Recycle()
		{
			entityManager.RecycleEntity(this);
			Entity = null;
		}

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			components = GetComponents<IComponent>();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
	}
}