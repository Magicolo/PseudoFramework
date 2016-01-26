using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.EntityOld
{
	public class ComponentGroup
	{
		readonly Type type;
		readonly List<IComponentOld> components = new List<IComponentOld>();
		readonly IList genericComponents;

		public ComponentGroup(Type type)
		{
			this.type = type;

			genericComponents = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
		}

		public List<IComponentOld> GetComponents()
		{
			return components;
		}

		public List<T> GetComponents<T>()
		{
			return (List<T>)genericComponents;
		}

		public void TryAddComponent(IComponentOld component)
		{
			if (type.IsAssignableFrom(component.GetType()))
				AddComponent(component);
		}

		public void RemoveComponent(IComponentOld component)
		{
			if (components.Remove(component))
				genericComponents.Remove(component);
		}

		void AddComponent(IComponentOld component)
		{
			if (!components.Contains(component))
			{
				components.Add(component);
				genericComponents.Add(component);
			}
		}
	}
}