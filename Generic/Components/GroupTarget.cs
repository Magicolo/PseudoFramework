using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class GroupTarget : TargetBase
	{
		public enum TargetPreferences
		{
			Closest,
			Farthest,
			First,
			Last
		}

		public EntityGroups Group;
		public TargetPreferences Prefer;
		public bool AutoUpdate = true;
		[Range(0.001f, 100)]
		public float UpdateFrequency = 2f;
		public TimeComponent Time;

		public override Vector3 Target
		{
			get
			{
				if (HasTarget)
					return target.GetTransform().position;
				else
					return default(Vector3);
			}
		}
		public override bool HasTarget
		{
			get { return target != null && target.HasTransform(); }
		}

		IEntityGroup targetables;
		IEntity target;
		float counter;
		[Inject]
		readonly IEntityManager entityManager = null;
		readonly Action<IEntity> onTargetRemoved;

		public GroupTarget()
		{
			onTargetRemoved = OnTargetRemoved;
		}

		public override void OnActivated()
		{
			base.OnActivated();

			targetables.OnEntityRemoved += onTargetRemoved;
		}

		public override void OnDeactivated()
		{
			base.OnDeactivated();

			targetables.OnEntityRemoved -= onTargetRemoved;
		}

		[Message(ComponentMessages.OnAdded)]
		void OnAdd()
		{
			targetables = entityManager.Entities.Filter(typeof(TransformComponent));
		}

		void Update()
		{
			if (!AutoUpdate) return;

			counter += Time.DeltaTime;

			if (counter >= 1f / UpdateFrequency)
			{
				counter -= 1f / UpdateFrequency;
				UpdateTarget();
			}
		}

		public void UpdateTarget()
		{
			var targets = targetables.Filter(Group);

			switch (Prefer)
			{
				case TargetPreferences.Closest:
					target = targets.GetClosest(Entity.GetTransform().position);
					break;
				case TargetPreferences.Farthest:
					target = targets.GetFarthest(Entity.GetTransform().position);
					break;
				case TargetPreferences.First:
					target = targets.First();
					break;
				case TargetPreferences.Last:
					target = targets.Last();
					break;
			}
		}

		void OnTargetRemoved(IEntity entity)
		{
			if (target == entity)
			{
				target = null;
				UpdateTarget();
			}
		}
	}
}