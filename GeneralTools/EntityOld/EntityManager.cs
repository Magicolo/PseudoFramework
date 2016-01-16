using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo
{
	public class EntityManagerOld : Singleton<EntityManagerOld>
	{
		public static IList<IEntityOld> AllEntities
		{
			get { return masterGroup.Entities; }
		}

		static readonly EntityGroup masterGroup = new EntityGroup();
		static readonly List<IEntityUpdateable> updateables = new List<IEntityUpdateable>();

		public static IEntityGroupOld GetEntityGroup(ByteFlag groups, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return masterGroup.Filter(groups, match);
		}

		public static IEntityGroupOld GetEntityGroup(EntityMatchOld match)
		{
			return masterGroup.Filter(match);
		}

		public static IEntityGroupOld GetEntityGroup(Type componentType, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return masterGroup.Filter(componentType, match);
		}

		public static IEntityGroupOld GetEntityGroup(Type[] componentTypes, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return masterGroup.Filter(componentTypes, match);
		}

		public static void BroadcastMessage(EntityMessages message)
		{
			masterGroup.BroadcastMessage(message);
		}

		public static void BroadcastMessage<T>(EntityMessages message, T argument)
		{
			masterGroup.BroadcastMessage(message, argument);
		}

		public static void BroadcastMessage(EntityMessages message, object argument)
		{
			masterGroup.BroadcastMessage(message, argument);
		}

		public static void ClearAllEntityGroups()
		{
			masterGroup.Clear();
		}

		public static void UpdateEntity(IEntityOld entity)
		{
			masterGroup.UpdateEntity(entity, true);
		}

		public static void RegisterEntity(IEntityOld entity)
		{
			InitializeManager();
			masterGroup.UpdateEntity(entity, true);

			var updateable = entity as IEntityUpdateable;

			if (updateable != null)
				updateables.Add(updateable);
		}

		public static void UnregisterEntity(IEntityOld entity)
		{
			masterGroup.UpdateEntity(entity, false);

			if (entity is IEntityUpdateable)
				updateables.Remove((IEntityUpdateable)entity);
		}

		static void InitializeManager()
		{
			if (ApplicationUtility.IsPlaying && Instance == null)
				new GameObject("EntityManager").AddComponent<EntityManagerOld>();
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