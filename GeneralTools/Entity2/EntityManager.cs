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
		static readonly List<IComponent> updateables = new List<IComponent>();
		static readonly List<IComponent> fixedUpdateables = new List<IComponent>();

		public static IEntityGroup GetEntityGroup(EntityGroups group, EntityMatches match = EntityMatches.All)
		{
			return masterGroup.Filter(group, match);
		}

		public static IEntityGroup GetEntityGroup(ByteFlag<EntityGroups> groups, EntityMatches match = EntityMatches.All)
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

		public static void UpdateEntity(IEntity entity)
		{
			masterGroup.UpdateEntity(entity);
		}

		public static void RegisterEntity(IEntity entity)
		{
			Initialize();
			masterGroup.RegisterEntity(entity);
			//entity.OnComponentAdded += OnComponentAdded;
			//entity.OnComponentRemoved += OnComponentRemoved;

			var components = entity.GetAllComponents();

			for (int i = 0; i < components.Count; i++)
				OnComponentAdded(components[i]);
		}

		public static void UnregisterEntity(IEntity entity)
		{
			masterGroup.UnregisterEntity(entity);
			//entity.OnComponentAdded -= OnComponentAdded;
			//entity.OnComponentRemoved -= OnComponentRemoved;

			var components = entity.GetAllComponents();

			for (int i = 0; i < components.Count; i++)
				OnComponentRemoved(components[i]);
		}

		public static void ClearAllEntityGroups()
		{
			masterGroup.Clear();
		}

		static void Initialize()
		{
			if (Application.isPlaying && Instance == null)
				new GameObject("EntityManager").AddComponent<EntityManager>();
		}

		static void OnComponentAdded(IComponent component)
		{
			if (component is IUpdateable && !updateables.Contains(component))
				updateables.Add(component);

			if (component is IFixedUpdateable && !fixedUpdateables.Contains(component))
				fixedUpdateables.Add(component);
		}

		static void OnComponentRemoved(IComponent component)
		{
			if (component is IUpdateable)
				updateables.Remove(component);

			if (component is IFixedUpdateable)
				fixedUpdateables.Remove(component);
		}

		void Update()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active && updateable.Entity.Active)
					((IUpdateable)updateable).Update();
			}
		}

		void FixedUpdate()
		{
			for (int i = 0; i < fixedUpdateables.Count; i++)
			{
				var fixedUpdateable = fixedUpdateables[i];

				if (fixedUpdateable.Active && fixedUpdateable.Entity.Active)
					((IFixedUpdateable)fixedUpdateable).FixedUpdate();
			}
		}

		void OnDestroy()
		{
			updateables.Clear();
			fixedUpdateables.Clear();
			ClearAllEntityGroups();
			EntityUtility.ClearAll();
		}
	}
}