using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo
{
	public partial class PEntity
	{
		readonly Dictionary<Type, ComponentGroup> componentGroups = new Dictionary<Type, ComponentGroup>();

		ComponentGroup GetComponentGroup(Type type)
		{
			ComponentGroup group;

			if (!componentGroups.TryGetValue(type, out group))
			{
				group = CreateComponentGroup(type);
				componentGroups[type] = group;
			}

			return group;
		}

		ComponentGroup CreateComponentGroup(Type type)
		{
			var group = new ComponentGroup(type);

			for (int i = 0; i < allComponents.Count; i++)
				group.TryAddComponent(allComponents[i]);

			return group;
		}

		void RegisterComponentToGroups(IComponentOld component)
		{
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.TryAddComponent(component);

				enumerator.Dispose();
			}
		}

		void UnregisterComponentFromGroups(IComponentOld component)
		{
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.RemoveComponent(component);

				enumerator.Dispose();
			}
		}
	}
}