using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class ComponentHolder
	{
		[InitializeContent]
		List<Component> components = new List<Component>(4);

		public bool TryGetComponent(out Component component)
		{
			if (components.Count > 0)
			{
				component = components[0];
				return true;
			}
			else
			{
				component = null;
				return false;
			}
		}

		public Component GetComponent()
		{
			return components.Count > 0 ? components[0] : null;
		}

		public List<Component> GetComponents()
		{
			return components;
		}

		public void AddComponent(Component component)
		{
			components.Add(component);
		}

		public void RemoveComponent(Component component)
		{
			components.Remove(component);
		}

		public void RemoveComponents()
		{
			components.Clear();
		}
	}
}