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
		static readonly EntityMatchGroup[] matchGroups;
		static readonly List<PEntity> allEntities = new List<PEntity>(64);

		static EntityManager()
		{
			var matchValues = (EntityMatch.Matches[])Enum.GetValues(typeof(EntityMatch.Matches));
			matchGroups = new EntityMatchGroup[matchValues.Length];

			for (int i = 0; i < matchValues.Length; i++)
				matchGroups[i] = new EntityMatchGroup(matchValues[i]);
		}

		public static List<PEntity> GetEntities(EntityMatch.Groups group, EntityMatch.Matches match = EntityMatch.Matches.All)
		{
			return GetEntityGroup(group, match).GetEntities();
		}

		public static List<PEntity> GetEntities(EntityMatch match)
		{
			return GetEntityGroup(match).GetEntities();
		}

		public static EntityGroup GetEntityGroup(EntityMatch.Groups group, EntityMatch.Matches match = EntityMatch.Matches.All)
		{
			return GetEntityMatchGroup(match).GetEntityGroup(group);
		}

		public static EntityGroup GetEntityGroup(EntityMatch match)
		{
			return GetEntityGroup(match.Group, match.Match);
		}

		public static List<PEntity> GetAllEntities()
		{
			return allEntities;
		}

		public static void UpdateEntity(PEntity entity)
		{
			for (int i = 0; i < matchGroups.Length; i++)
				matchGroups[i].UpdateEntity(entity);
		}

		public static void RegisterEntity(PEntity entity)
		{
			allEntities.Add(entity);

			for (int i = 0; i < matchGroups.Length; i++)
				matchGroups[i].RegisterEntity(entity);
		}

		public static void UnregisterEntity(PEntity entity)
		{
			allEntities.Remove(entity);

			for (int i = 0; i < matchGroups.Length; i++)
				matchGroups[i].UnregisterEntity(entity);
		}

		static EntityMatchGroup GetEntityMatchGroup(EntityMatch.Matches match)
		{
			return matchGroups[(int)match];
		}
	}
}