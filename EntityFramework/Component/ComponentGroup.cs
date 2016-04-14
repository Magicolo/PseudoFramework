using Pseudo.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Pseudo.EntityFramework.Internal
{
	public abstract class ComponentGroupBase : IComponentGroup, IPoolable
	{
		public IList<IComponent> Components
		{
			get { return components; }
		}

		protected readonly List<IComponent> components;

		protected ComponentGroupBase()
		{
			components = new List<IComponent>();
		}

		public abstract bool TryAdd(IComponent component);

		public abstract void Remove(IComponent component);

		public abstract void RemoveAll();

		void IPoolable.OnCreate() { }

		void IPoolable.OnRecycle()
		{
			RemoveAll();
		}
	}

	public class ComponentGroup<T> : ComponentGroupBase, IComponentGroup<T> where T : IComponent
	{
		IList<T> IComponentGroup<T>.Components
		{
			get { return genericComponents; }
		}

		readonly List<T> genericComponents = new List<T>();

		public ComponentGroup()
		{
			genericComponents = new List<T>();
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
			Assert.IsTrue(component is T);

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
