using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo.Internal.Entity
{
	public class Entity : IEntity
	{
		public event Action<IEntity, IComponent> OnComponentAdded;
		public event Action<IEntity, IComponent> OnComponentRemoved;

		public EntityGroups Groups
		{
			get { return groups; }
			set
			{
				groups = value;
				entityManager.UpdateEntity(this);
			}
		}
		public IList<IComponent> Components
		{
			get { return readonlyComponents; }
		}

		EntityGroups groups;
		IEntityManager entityManager;
		readonly List<IComponent> allComponents;
		readonly IList<IComponent> readonlyComponents;
		readonly List<int> componentIndices;
		readonly IList<int> readonlyComponentIndices;
		ComponentGroup[] componentGroups;

		public Entity(IEntityManager entityManager) : this(entityManager, EntityGroups.Nothing) { }

		public Entity(IEntityManager entityManager, EntityGroups groups)
		{
			this.entityManager = entityManager;
			this.groups = groups;

			allComponents = new List<IComponent>();
			readonlyComponents = allComponents.AsReadOnly();
			componentIndices = new List<int>();
			readonlyComponentIndices = componentIndices.AsReadOnly();
			componentGroups = new ComponentGroup[ComponentUtility.ComponentTypes.Length];
		}

		public IList<int> GetComponentIndices()
		{
			return readonlyComponentIndices;
		}

		public T GetComponent<T>() where T : IComponent
		{
			var components = GetComponentGroup<T>().Components;

			if (components.Count > 0)
				return components[0];
			else
				return default(T);
		}

		public IComponent GetComponent(Type type)
		{
			var components = GetComponentGroup(type).Components;

			if (components.Count > 0)
				return components[0];
			else
				return null;
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

		public void AddComponent(IComponent component)
		{
			AddComponent(component, true, true);
		}

		public void AddComponents(params IComponent[] components)
		{
			AddComponents(components, true);
		}

		public void RemoveComponent(IComponent component)
		{
			RemoveComponent(component, true, true);
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

		void AddComponent(IComponent component, bool raiseEvent, bool updateEntity)
		{
			if (HasComponent(component))
				return;

			allComponents.Add(component);

			for (int i = 0; i < componentGroups.Length; i++)
			{
				var componentGroup = componentGroups[i];

				if (componentGroup != null)
				{
					bool isEmpty = componentGroup.Components.Count == 0;

					if (componentGroup.TryAdd(component) && isEmpty)
						AddComponentIndex(i);
				}
			}

			// Ensure component groups exist for sub types for proper entity matching
			var subComponentTypes = ComponentUtility.GetSubComponentTypes(component.GetType());

			for (int i = 0; i < subComponentTypes.Length; i++)
				GetComponentGroup(subComponentTypes[i]);
			////

			if (raiseEvent && OnComponentAdded != null)
				OnComponentAdded(this, component);

			if (updateEntity)
				entityManager.UpdateEntity(this);
		}

		void AddComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				AddComponent(components[i], raiseEvent, false);

			entityManager.UpdateEntity(this);
		}

		void RemoveComponent(IComponent component, bool raiseEvent, bool updateEntity)
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
							RemoveComponentIndex(i);
					}
				}
			}

			if (raiseEvent && OnComponentRemoved != null)
				OnComponentRemoved(this, component);

			if (updateEntity)
				entityManager.UpdateEntity(this);
		}

		void RemoveComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i], raiseEvent, false);

			entityManager.UpdateEntity(this);
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

				if (raiseEvent && OnComponentRemoved != null)
					OnComponentRemoved(this, component);
			}

			allComponents.Clear();
			componentIndices.Clear();
			entityManager.UpdateEntity(this);
		}

		void AddComponentIndex(int index)
		{
			if (!componentIndices.Contains(index))
			{
				componentIndices.Add(index);
				componentIndices.Sort();
			}
		}

		void RemoveComponentIndex(int index)
		{
			componentIndices.Remove(index);
		}

		IComponentGroup<T> GetComponentGroup<T>() where T : IComponent
		{
			ComponentGroup componentGroup;
			int typeIndex = ComponentIndexHolder<T>.Index;

			if (componentGroups.Length <= typeIndex)
			{
				Array.Resize(ref componentGroups, ComponentUtility.ComponentTypes.Length);
				componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex));
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex));
					componentGroups[typeIndex] = componentGroup;
				}
			}

			return (IComponentGroup<T>)componentGroup;
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
				componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex));
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(ComponentUtility.GetComponentType(typeIndex));
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
				componentGroup = CreateComponentGroup(type);
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(type);
					componentGroups[typeIndex] = componentGroup;
				}
			}

			return componentGroup;
		}

		ComponentGroup CreateComponentGroup(Type type)
		{
			var componentGroupType = typeof(ComponentGroup<>).MakeGenericType(type);
			var componentGroup = (ComponentGroup)Activator.CreateInstance(componentGroupType);
			bool success = false;

			for (int i = 0; i < allComponents.Count; i++)
				success |= componentGroup.TryAdd(allComponents[i]);

			if (success)
				AddComponentIndex(ComponentUtility.GetComponentIndex(type));

			return componentGroup;
		}
	}
}
