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

		void Awake()
		{
			components = GetComponents<IComponent>();
		}

		[PostInject]
		public void Initialize(IEntityManager entityManager)
		{
			components = components ?? GetComponents<IComponent>();
			entity = entityManager.CreateEntity(groups);
			entity.AddComponents(components);
		}

		public override void OnCreate()
		{
			base.OnCreate();

			for (int i = 0; i < components.Length; i++)
			{
				var poolable = components[i] as IPoolable;

				if (poolable != null)
					poolable.OnCreate();
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			for (int i = 0; i < components.Length; i++)
			{
				var poolable = components[i] as IPoolable;

				if (poolable != null)
					poolable.OnRecycle();
			}
		}

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			components = GetComponents<IComponent>();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
	}
}