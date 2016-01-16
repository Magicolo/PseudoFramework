using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity
{
	public abstract class ComponentGroup : IComponentGroup
	{
		public IList<IComponent> Components
		{
			get { return readonlyComponents; }
		}

		protected readonly List<IComponent> components;
		readonly IList<IComponent> readonlyComponents;

		protected ComponentGroup()
		{
			components = new List<IComponent>();
			readonlyComponents = components.AsReadOnly();
		}

		public abstract bool TryAdd(IComponent component);

		public abstract void Remove(IComponent component);

		public abstract void RemoveAll();
	}

	public class ComponentGroup<T> : ComponentGroup, IComponentGroup<T> where T : IComponent
	{
		IList<T> IComponentGroup<T>.Components
		{
			get { return readonlyGenericComponents; }
		}

		readonly List<T> genericComponents = new List<T>();
		readonly IList<T> readonlyGenericComponents;

		public ComponentGroup()
		{
			genericComponents = new List<T>();
			readonlyGenericComponents = genericComponents.AsReadOnly();
		}

		public override bool TryAdd(IComponent component)
		{
			if (component is T)
			{
				components.Add(component);
				genericComponents.Add((T)component);

				return true;
			}

			return false;
		}

		public override void Remove(IComponent component)
		{
			if (components.Remove(component))
				genericComponents.Remove((T)component);
		}

		public override void RemoveAll()
		{
			components.Clear();
			genericComponents.Clear();
		}
	}
}
