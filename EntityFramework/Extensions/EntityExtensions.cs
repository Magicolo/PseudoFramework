using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.EntityFramework.Internal;

namespace Pseudo.EntityFramework
{
	public static class EntityExtensions
	{
		public static bool HasTransform(this IEntity entity)
		{
			return entity.Has<TransformComponent>();
		}

		public static Transform GetTransform(this IEntity entity)
		{
			var component = entity.Get<TransformComponent>();

			return component == null ? null : component.Transform;
		}

		public static bool HasGameObject(this IEntity entity)
		{
			return entity.Has<GameObjectComponent>();
		}

		public static GameObject GetGameObject(this IEntity entity)
		{
			var component = entity.Get<GameObjectComponent>();

			return component == null ? null : component.GameObject;
		}

		public static bool HasBehaviour(this IEntity entity)
		{
			return entity.Has<BehaviourComponent>();
		}

		public static EntityBehaviour GetBehaviour(this IEntity entity)
		{
			var component = entity.Get<BehaviourComponent>();

			return component == null ? null : component.Behaviour;
		}

		//public static bool HasTime(this IEntity entity)
		//{
		//	return entity.Has<TimeComponent>();
		//}

		//public static TimeChannel GetTime(this IEntity entity)
		//{
		//	return entity.Get<TimeComponent>();
		//}

		public static IEntity GetEntity(this Component component)
		{
			return EntityUtility.GetEntity(component);
		}

		public static IEntity First(this IEntityGroup group)
		{
			return group.Count > 0 ? group[0] : null;
		}

		public static IEntity Last(this IEntityGroup group)
		{
			return group.Count > 0 ? group[group.Count - 1] : null;
		}

		public static IEntity GetClosest(this IEntityGroup group, Vector3 position)
		{
			float closestDisance = float.MaxValue;
			IEntity closestEntity = null;

			for (int i = 0; i < group.Count; i++)
			{
				var entity = group[i];
				var transform = entity.GetTransform();
				float distance = Vector3.Distance(transform.position, position);

				if (distance < closestDisance)
				{
					closestDisance = distance;
					closestEntity = entity;
				}
			}

			return closestEntity;
		}

		public static IEntity GetFarthest(this IEntityGroup group, Vector3 position)
		{
			float farthestDistance = 0f;
			IEntity farthestEntity = null;

			for (int i = 0; i < group.Count; i++)
			{
				var entity = group[i];
				var transform = entity.GetTransform();
				float distance = Vector3.Distance(transform.position, position);

				if (distance > farthestDistance)
				{
					farthestDistance = distance;
					farthestEntity = entity;
				}
			}

			return farthestEntity;
		}
	}
}