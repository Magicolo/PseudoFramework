using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public static class EntityUtility
	{
		static readonly Dictionary<Component, EntityBehaviour> componentToEntity = new Dictionary<Component, EntityBehaviour>();

		public static IEntity GetEntity(Component component)
		{
			EntityBehaviour entity;

			if (!componentToEntity.TryGetValue(component, out entity))
			{
				entity = component.GetComponentInParent<EntityBehaviour>();
				componentToEntity[component] = entity;
			}

			return entity == null ? null : entity.Entity;
		}
	}
}
