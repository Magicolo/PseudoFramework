using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;

namespace Pseudo.Internal.Entity
{
	[AddComponentMenu("Pseudo/Entity3")]
	public class EntityBehaviour : PMonoBehaviour
	{
		public IEntity Entity
		{
			get { return entity; }
		}

		[Inject]
		IEntityManager entityManager = null;

		[SerializeField, EntityGroups]
		EntityGroups groups = EntityGroups.Nothing;
		IEntity entity;

		[PostInject]
		void Initialize()
		{
			entity = entityManager.CreateEntity(groups);
			entity.AddComponents(GetComponents<IComponent>());
		}
	}
}