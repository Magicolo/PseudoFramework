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
			get { return target != null; }
		}

		IEntityGroup targetables;
		IEntity target;
		float counter;
		readonly Action<IEntity> onTargetRemoved;

		public GroupTarget()
		{
			onTargetRemoved = OnTargetRemoved;
		}

		[Message(ComponentMessages.OnAdded)]
		void OnAdd()
		{
			targetables.OnEntityRemoved += onTargetRemoved;
			targetables = Entity.Manager.Entities.Filter(typeof(TransformComponent));
		}

		[Message(ComponentMessages.OnRemoved)]
		void OnRemoved()
		{
			targetables.OnEntityRemoved -= onTargetRemoved;
		}

		void Update()
		{
			counter += Time.DeltaTime;

			if (counter >= 1f / UpdateFrequency)
			{
				counter -= 1f / UpdateFrequency;
				UpdateTarget();
			}
		}

		void UpdateTarget()
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