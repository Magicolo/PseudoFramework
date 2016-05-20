using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class GetComponentHierarchyAction : HierarchyAction<Type, Component>
	{
		public override bool ApplyToSelf(Transform transform, ref Type input, out Component output)
		{
			return output = transform.GetComponent(input);
		}

		public override bool ApplyUpwards(Transform transform, ref Type input, out Component output)
		{
			return output = transform.GetComponentInParent(input);
		}

		public override bool ApplyDownwards(Transform transform, ref Type input, out Component output)
		{
			return output = transform.GetComponentInChildren(input);
		}
	}
}
