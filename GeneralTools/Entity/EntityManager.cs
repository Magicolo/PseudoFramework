using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public static class EntityManager
	{
		public static IList<PEntity> AllEntities
		{
			get { return masterGroup.Entities; }
		}

		static readonly EntityGroup masterGroup = new EntityGroup();

		public static IEntityGroup GetEntityGroup(ByteFlag<EntityGroups> groups, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(groups, match);
		}

		public static IEntityGroup GetEntityGroup(EntityMatch match)
		{
			return masterGroup.Filter(match);
		}

		public static IEntityGroup GetEntityGroup(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(componentTypes, match);
		}

		public static void ClearAllEntityGroups()
		{
			masterGroup.Clear();
		}

		public static void UpdateEntity(PEntity entity)
		{
			masterGroup.UpdateEntity(entity);
		}

		public static void RegisterEntity(PEntity entity)
		{
			EntityUtility.InitializeJanitor();
			masterGroup.RegisterEntity(entity);
		}

		public static void UnregisterEntity(PEntity entity)
		{
			masterGroup.UnregisterEntity(entity);
		}
	}
}