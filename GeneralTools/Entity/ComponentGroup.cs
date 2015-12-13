using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class ComponentGroup
	{
		readonly Type type;
		readonly List<Component> components = new List<Component>();
		readonly IList genericComponents;

		public ComponentGroup(Type type)
		{
			this.type = type;

			Type listType = typeof(List<>).MakeGenericType(type);
			genericComponents = (IList)Activator.CreateInstance(listType);
		}

		public List<Component> GetComponents()
		{
			return components;
		}

		public List<T> GetComponents<T>()
		{
			return (List<T>)genericComponents;
		}

		public void TryAddComponent(Component component)
		{
			if (type.IsAssignableFrom(component.GetType()))
				AddComponent(component);
		}

		public void RemoveComponent(Component component)
		{
			if (components.Remove(component))
				genericComponents.Remove(component);
		}

		void AddComponent(Component component)
		{
			if (!components.Contains(component))
			{
				components.Add(component);
				genericComponents.Add(component);
			}
		}
	}
}