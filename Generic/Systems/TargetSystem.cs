using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class TargetSystem : SystemBase, IUpdateable
	{
		public override float UpdateDelay
		{
			get { return 0.5f; }
		}

		IEntityGroup targetableEntities;

		public override IEntityGroup GetEntities()
		{
			targetableEntities = EntityManager.Entities.Filter(typeof(TransformComponent));

			return EntityManager.Entities.Filter(new[]
			{
				typeof(TransformComponent),
				typeof(TargetComponentBase)
			});
		}

		public override void OnActivate()
		{
			base.OnActivate();

			targetableEntities.OnEntityRemoved += OnEntityTargetRemoved;
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			targetableEntities.OnEntityRemoved -= OnEntityTargetRemoved;
		}

		public void Update()
		{
			for (int i = 0; i < Entities.Count; i++)
				UpdateTargets(Entities[i]);
		}

		void UpdateTargets(IEntity entity)
		{
			var targets = entity.GetComponents<TargetComponentBase>();

			for (int i = 0; i < targets.Count; i++)
			{
				var target = targets[i];

				if (target is GroupTargetComponent)
					UpdateTarget((GroupTargetComponent)target);
			}
		}

		void UpdateTarget(GroupTargetComponent groupTarget)
		{
			var targets = targetableEntities.Filter(groupTarget.Group);

			switch (groupTarget.Prefer)
			{
				case GroupTargetComponent.TargetPreferences.Closest:
					groupTarget.EntityTarget = GetClosest(targets, groupTarget.CachedTransform.position);
					break;
				case GroupTargetComponent.TargetPreferences.Farthest:
					groupTarget.EntityTarget = GetFarthest(targets, groupTarget.CachedTransform.position);
					break;
				case GroupTargetComponent.TargetPreferences.First:
					groupTarget.EntityTarget = targets.First();
					break;
				case GroupTargetComponent.TargetPreferences.Last:
					groupTarget.EntityTarget = targets.Last();
					break;
			}
		}

		public override void OnEntityAdded(IEntity entity)
		{
			base.OnEntityAdded(entity);

			UpdateTargets(entity);
		}

		void OnEntityTargetRemoved(IEntity entity)
		{
			Update();
		}

		IEntity GetClosest(IEntityGroup entities, Vector3 position)
		{
			float closestDisance = float.MaxValue;
			IEntity closestEntity = null;

			for (int i = 0; i < entities.Count; i++)
			{
				var entity = entities[i];
				var transform = entity.GetComponent<TransformComponent>().Transform;
				float distance = Vector3.Distance(transform.position, position);

				if (distance < closestDisance)
				{
					closestDisance = distance;
					closestEntity = entity;
				}
			}

			return closestEntity;
		}

		IEntity GetFarthest(IEntityGroup entities, Vector3 position)
		{
			float farthestDistance = 0f;
			IEntity farthestEntity = null;

			for (int i = 0; i < entities.Count; i++)
			{
				var entity = entities[i];
				var transform = entity.GetComponent<TransformComponent>().Transform;
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