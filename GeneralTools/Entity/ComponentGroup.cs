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
		readonly List<IComponent> components = new List<IComponent>();
		readonly IList genericComponents;

		public ComponentGroup(Type type)
		{
			this.type = type;

			genericComponents = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
		}

		public List<IComponent> GetComponents()
		{
			return components;
		}

		public List<T> GetComponents<T>()
		{
			return (List<T>)genericComponents;
		}

		public void TryAddComponent(IComponent component)
		{
			if (type.IsAssignableFrom(component.GetType()))
				AddComponent(component);
		}

		public void RemoveComponent(IComponent component)
		{
			if (components.Remove(component))
				genericComponents.Remove(component);
		}

		void AddComponent(IComponent component)
		{
			if (!components.Contains(component))
			{
				components.Add(component);
				genericComponents.Add(component);
			}
		}
	}
}