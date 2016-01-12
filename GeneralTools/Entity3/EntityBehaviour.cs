using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity3
{
	[AddComponentMenu("Pseudo/Entity3")]
	public class EntityBehaviour : PMonoBehaviour
	{
		public IEntity Entity
		{
			get { return entity; }
		}

		[SerializeField, EntityGroups]
		ByteFlag groups = ByteFlag.Nothing;
		IEntity entity;

		void Awake()
		{
			entity = EntityManager.Instance.CreateEntity(groups);
			entity.AddComponents(GetComponents<IComponent>());
		}
	}
}