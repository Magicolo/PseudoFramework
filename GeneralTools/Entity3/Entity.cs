using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo.Internal.Entity3
{
	[Serializable]
	public class Entity : IEntity
	{
		public event Action<IEntity, IComponent> OnComponentAdded;
		public event Action<IEntity, IComponent> OnComponentRemoved;

		public ByteFlag Groups
		{
			get { return groups; }
			set
			{
				groups = value;
				entityManager.UpdateEntity(this);
			}
		}
		public IList<IComponent> AllComponents
		{
			get { return readonlyComponents; }
		}

		ByteFlag groups;
		IEntityManager entityManager;
		readonly List<IComponent> allComponents;
		readonly IList<IComponent> readonlyComponents;
		readonly List<int> componentIndices;
		readonly IList<int> readonlyComponentIndices;
		ComponentGroup[] componentGroups;

		public Entity(IEntityManager entityManager)
		{
			this.entityManager = entityManager;

			allComponents = new List<IComponent>();
			readonlyComponents = allComponents.AsReadOnly();
			componentIndices = new List<int>();
			readonlyComponentIndices = componentIndices.AsReadOnly();
			componentGroups = new ComponentGroup[ComponentUtility.ComponentTypes.Length];
		}

		public Entity(IEntityManager entityManager, ByteFlag groups) : this(entityManager)
		{
			this.groups = groups;
		}

		public void AddComponent(IComponent component)
		{
			AddComponent(component, true);
		}

		public IList<int> GetComponentIndices()
		{
			return readonlyComponentIndices;
		}

		public T GetComponent<T>() where T : IComponent
		{
			return GetComponentGroup<T>().Components.First();
		}

		public IComponent GetComponent(Type type)
		{
			return GetComponentGroup(type).Components.First();
		}

		public IList<T> GetComponents<T>() where T : IComponent
		{
			return GetComponentGroup<T>().Components;
		}

		public IList<IComponent> GetComponents(Type type)
		{
			return GetComponentGroup(type).Components;
		}

		public bool TryGetComponent<T>(out T component) where T : IComponent
		{
			component = GetComponent<T>();
			return component != null;
		}

		public bool TryGetComponent(Type type, out IComponent component)
		{
			component = GetComponent(type);
			return component != null;
		}

		public bool HasComponent<T>() where T : IComponent
		{
			return GetComponent<T>() != null;
		}

		public bool HasComponent(Type type)
		{
			return GetComponent(type) != null;
		}

		public bool HasComponent(IComponent component)
		{
			return GetComponentGroup(component.GetType()).Components.Contains(component);
		}

		public void RemoveComponent(IComponent component)
		{
			RemoveComponent(component, true);
		}

		public void RemoveComponents<T>() where T : IComponent
		{
			var components = GetComponents<T>();

			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i--]);
		}

		public void RemoveComponents(Type type)
		{
			var components = GetComponents(type);

			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i--]);
		}

		public void RemoveAllComponents()
		{
			RemoveAllComponents(true);
		}

		void AddComponent(IComponent component, bool raiseEvent)
		{
			if (HasComponent(component))
				return;

			allComponents.Add(component);

			for (int i = 0; i < componentGroups.Length; i++)
			{
				var componentGroup = componentGroups[i];

				if (componentGroup != null)
					componentGroup.TryAdd(component);
			}

			entityManager.UpdateEntity(this);

			if (raiseEvent && OnComponentAdded != null)
				OnComponentAdded(this, component);
		}

		void RemoveComponent(IComponent component, bool raiseEvent)
		{
			if (allComponents.Remove(component))
			{
				for (int i = 0; i < componentGroups.Length; i++)
				{
					var componentGroup = componentGroups[i];

					if (componentGroup != null && componentGroup.Components.Count > 0)
					{
						componentGroup.Remove(component);

						if (componentGroup.Components.Count == 0)
							componentIndices.Remove(i);
					}
				}
			}

			entityManager.UpdateEntity(this);

			if (raiseEvent && OnComponentRemoved != null)
				OnComponentRemoved(this, component);
		}

		void RemoveAllComponents(bool raiseEvent)
		{
			for (int i = 0; i < componentGroups.Length; i++)
			{
				var componentGroup = componentGroups[i];

				if (componentGroup != null)
					componentGroup.RemoveAll();
			}

			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];
				entityManager.UpdateEntity(this);

				if (raiseEvent && OnComponentRemoved != null)
					OnComponentRemoved(this, component);
			}

			allComponents.Clear();
			componentIndices.Clear();
			entityManager.UpdateEntity(this);
		}

		IComponentGroup<T> GetComponentGroup<T>() where T : IComponent
		{
			return (IComponentGroup<T>)GetComponentGroup(ComponentIndexHolder<T>.Index);
		}

		IComponentGroup GetComponentGroup(Type type)
		{
			return GetComponentGroup(type, ComponentUtility.GetComponentIndex(type));
		}

		IComponentGroup GetComponentGroup(int typeIndex)
		{
			ComponentGroup componentGroup;

			if (componentGroups.Length <= typeIndex)
			{
				Array.Resize(ref componentGroups, ComponentUtility.ComponentTypes.Length);
				componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex), typeIndex);
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex), typeIndex);
					componentGroups[typeIndex] = componentGroup;
				}
			}

			return componentGroup;
		}

		IComponentGroup GetComponentGroup(Type type, int typeIndex)
		{
			ComponentGroup componentGroup;

			if (componentGroups.Length <= typeIndex)
			{
				Array.Resize(ref componentGroups, ComponentUtility.ComponentTypes.Length);
				componentGroup = CreateComponentGroup(type, typeIndex);
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(type, typeIndex);
					componentGroups[typeIndex] = componentGroup;
				}
			}

			return componentGroup;
		}

		ComponentGroup CreateComponentGroup(Type type, int typeIndex)
		{
			componentIndices.Add(typeIndex);
			componentIndices.Sort();

			var componentGroupType = typeof(ComponentGroup<>).MakeGenericType(type);
			var componentGroup = (ComponentGroup)Activator.CreateInstance(componentGroupType);

			for (int i = 0; i < allComponents.Count; i++)
				componentGroup.TryAdd(allComponents[i]);

			return componentGroup;
		}
	}
}
