using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class ComponentGroup
	{
		public event Action<Component> OnComponentAdded;
		public event Action<Component> OnComponentRemoved;

		readonly List<Component> components = new List<Component>(2);
		readonly IList genericComponents;

		public ComponentGroup(Type type)
		{
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

		public void AddComponent(Component component)
		{
			if (!components.Contains(component))
			{
				components.Add(component);
				genericComponents.Add(component);
				RaiseOnComponentAdded(component);
			}
		}

		public void RemoveComponent(Component component)
		{
			if (components.Remove(component))
			{
				genericComponents.Remove(component);
				RaiseOnComponentRemoved(component);
			}
		}

		protected virtual void RaiseOnComponentAdded(Component component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(component);
		}

		protected virtual void RaiseOnComponentRemoved(Component component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(component);
		}
	}
}