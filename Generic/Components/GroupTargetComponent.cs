using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class GroupTargetComponent : TargetComponentBase
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

		public override Vector3 Target
		{
			get
			{
				if (HasTarget)
					return entityTarget.GetComponent<TransformComponent>().Transform.position;
				else
					return default(Vector3);
			}
		}
		public override bool HasTarget
		{
			get { return entityTarget != null; }
		}
		public IEntity EntityTarget
		{
			get { return entityTarget; }
			set { entityTarget = value; }
		}

		IEntity entityTarget;
	}
}