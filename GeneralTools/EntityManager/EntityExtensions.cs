using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class EntityExtensions
	{
		public static bool HasTransform(this IEntity entity)
		{
			return entity.HasComponent<TransformComponent>();
		}

		public static Transform GetTransform(this IEntity entity)
		{
			return entity.GetComponent<TransformComponent>().Transform;
		}

		public static bool HasGameObject(this IEntity entity)
		{
			return entity.HasComponent<GameObjectComponent>();
		}

		public static GameObject GetGameObject(this IEntity entity)
		{
			return entity.GetComponent<GameObjectComponent>().GameObject;
		}

		public static bool HasTime(this IEntity entity)
		{
			return entity.HasComponent<TimeComponent>();
		}

		public static TimeChannel GetTime(this IEntity entity)
		{
			return entity.GetComponent<TimeComponent>();
		}

		public static IEntity First(this IEntityGroup group)
		{
			return group.Count > 0 ? group[0] : null;
		}

		public static IEntity Last(this IEntityGroup group)
		{
			return group.Count > 0 ? group[group.Count - 1] : null;
		}
	}
}