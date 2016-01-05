using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class EntityManager : Singleton<EntityManager>
	{
		public static IList<IEntity> AllEntities
		{
			get { return masterGroup.Entities; }
		}

		static readonly EntityGroup masterGroup = new EntityGroup();
		static readonly List<IEntityUpdateable> updateables = new List<IEntityUpdateable>();

		public static IEntityGroup GetEntityGroup(ByteFlag groups, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(groups, match);
		}

		public static IEntityGroup GetEntityGroup(EntityMatch match)
		{
			return masterGroup.Filter(match);
		}

		public static IEntityGroup GetEntityGroup(Type componentType, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(componentType, match);
		}

		public static IEntityGroup GetEntityGroup(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(componentTypes, match);
		}

		public static void ClearAllEntityGroups()
		{
			masterGroup.Clear();
		}

		public static void UpdateEntity(IEntity entity)
		{
			masterGroup.UpdateEntity(entity, true);
		}

		public static void RegisterEntity(IEntity entity)
		{
			InitializeManager();
			masterGroup.UpdateEntity(entity, true);

			if (entity is IEntityUpdateable)
				updateables.Add((IEntityUpdateable)entity);
		}

		public static void UnregisterEntity(IEntity entity)
		{
			masterGroup.UpdateEntity(entity, false);

			if (entity is IEntityUpdateable)
				updateables.Remove((IEntityUpdateable)entity);
		}

		static void InitializeManager()
		{
			if (ApplicationUtility.IsPlaying && Instance == null)
				new GameObject("EntityManager").AddComponent<EntityManager>();
		}

		void Update()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active)
					updateable.ComponentUpdate();
			}
		}

		void LateUpdate()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active)
					updateable.ComponentLateUpdate();
			}
		}

		void FixedUpdate()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active)
					updateable.ComponentFixedUpdate();
			}
		}

		void OnDestroy()
		{
			ClearAllEntityGroups();
			EntityUtility.ClearAll();
			GC.Collect();
		}
	}
}