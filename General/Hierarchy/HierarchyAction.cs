using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class HierarchyAction<TIn, TOut>
	{
		public virtual TOut ApplyToHierarchy(Transform transform, ref TIn input, HierarchyScopes scope)
		{
			var output = default(TOut);
			var parent = transform.parent;
			var root = transform.root;

			if (scope.Contains(HierarchyScopes.Hierarchy))
				ApplyDownwards(root, ref input, out output);
			else
			{
				if (scope.Contains(HierarchyScopes.Self) && ApplyToSelf(transform, ref input, out output))
					return output;

				if (scope.Contains(HierarchyScopes.Descendants))
				{
					for (int i = 0; i < transform.childCount; i++)
					{
						if (ApplyDownwards(transform.GetChild(i), ref input, out output))
							return output;
					}
				}
				else if (scope.Contains(HierarchyScopes.Children))
				{
					for (int i = 0; i < transform.childCount; i++)
					{
						if (ApplyToSelf(transform.GetChild(i), ref input, out output))
							return output;
					}
				}

				if (scope.Contains(HierarchyScopes.Ancestors))
				{
					if (parent != null && ApplyUpwards(parent, ref input, out output))
						return output;
				}
				else if (scope.Contains(HierarchyScopes.Parent))
				{
					if (parent != null && ApplyToSelf(parent, ref input, out output))
						return output;
				}

				if (scope.Contains(HierarchyScopes.Root) && ApplyToSelf(root, ref input, out output))
					return output;

				if (scope.Contains(HierarchyScopes.Siblings))
				{
					if (parent != null)
					{
						for (int i = 0; i < parent.childCount; i++)
						{
							var child = parent.GetChild(i);

							if (child != transform && ApplyToSelf(child, ref input, out output))
								return output;
						}
					}
				}
			}

			return output;
		}

		public abstract bool ApplyToSelf(Transform transform, ref TIn input, out TOut output);
		public abstract bool ApplyUpwards(Transform transform, ref TIn input, out TOut output);
		public abstract bool ApplyDownwards(Transform transform, ref TIn input, out TOut output);
	}
}
