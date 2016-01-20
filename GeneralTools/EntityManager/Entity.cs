using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Entity
{
	public class Entity : IEntity
	{
		public event Action<IEntity, IComponent> OnComponentAdded = delegate { };
		public event Action<IEntity, IComponent> OnComponentRemoved = delegate { };

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
		[DoNotInitialize]
		IComponent[] singleComponents;
		[DoNotInitialize]
		ComponentGroup[] componentGroups;
		readonly List<IComponent> allComponents;
		readonly IList<IComponent> readonlyComponents;
		readonly List<int> componentIndices;
		readonly IList<int> readonlyComponentIndices;

		public Entity()
		{
			singleComponents = new IComponent[ComponentUtility.ComponentTypes.Length];
			componentGroups = new ComponentGroup[ComponentUtility.ComponentTypes.Length];
			allComponents = new List<IComponent>();
			readonlyComponents = allComponents.AsReadOnly();
			componentIndices = new List<int>();
			readonlyComponentIndices = componentIndices.AsReadOnly();
		}

		public void Initialize(IEntityManager entityManager, EntityGroups groups)
		{
			this.entityManager = entityManager;
			this.groups = groups;
		}

		public IList<int> GetComponentIndices()
		{
			return readonlyComponentIndices;
		}

		public T GetComponent<T>() where T : IComponent
		{
			int index = ComponentIndexHolder<T>.Index;

			if (index >= singleComponents.Length)
				return default(T);
			else
				return (T)singleComponents[index];
		}

		public IComponent GetComponent(Type type)
		{
			Assert.IsNotNull(type);

			int index = ComponentUtility.GetComponentIndex(type);

			if (index >= singleComponents.Length)
				return null;
			else
				return singleComponents[index];
		}

		public IList<T> GetComponents<T>() where T : IComponent
		{
			return GetComponentGroup<T>().Components;
		}

		public IList<IComponent> GetComponents(Type type)
		{
			Assert.IsNotNull(type);

			return GetComponentGroup(type).Components;
		}

		public bool TryGetComponent<T>(out T component) where T : IComponent
		{
			component = GetComponent<T>();

			return component != null;
		}

		public bool TryGetComponent(Type type, out IComponent component)
		{
			Assert.IsNotNull(type);

			component = GetComponent(type);

			return component != null;
		}

		public bool HasComponent<T>() where T : IComponent
		{
			return GetComponent<T>() != null;
		}

		public bool HasComponent(Type type)
		{
			Assert.IsNotNull(type);

			return GetComponent(type) != null;
		}

		public bool HasComponent(IComponent component)
		{
			Assert.IsNotNull(component);

			return GetComponentGroup(component.GetType()).Components.Contains(component);
		}

		public void AddComponent(IComponent component)
		{
			Assert.IsNotNull(component);

			AddComponent(component, true, true);
		}

		public void AddComponents(params IComponent[] components)
		{
			Assert.IsNotNull(components);

			AddComponents(components, true);
		}

		public void RemoveComponent(IComponent component)
		{
			Assert.IsNotNull(component);

			RemoveComponent(component, true, true);
		}

		public void RemoveComponents<T>() where T : IComponent
		{
			var components = GetComponents<T>();

			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i]);
		}

		public void RemoveComponents(Type type)
		{
			var components = GetComponents(type);

			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i]);
		}

		public void RemoveAllComponents()
		{
			RemoveAllComponents(true);
		}

		bool AddComponent(IComponent component, bool raiseEvent, bool updateEntity)
		{
			if (HasComponent(component))
				return false;

			allComponents.Add(component);
			var subComponentTypes = ComponentUtility.GetSubComponentTypes(component.GetType());
			bool isNew = false;

			for (int i = 0; i < subComponentTypes.Length; i++)
			{
				int index = ComponentUtility.GetComponentIndex(subComponentTypes[i]);
				AddComponentToGroup(component, index);
				isNew |= AddSingleComponent(component, index);
			}

			if (raiseEvent)
				OnComponentAdded(this, component);

			if (isNew && updateEntity)
				entityManager.UpdateEntity(this);

			return isNew;
		}

		void AddComponentToGroup(IComponent component, int index)
		{
			var componentGroup = componentGroups.Length <= index ? null : componentGroups[index];

			if (componentGroup != null)
				componentGroup.TryAdd(component);
		}

		bool AddSingleComponent(IComponent component, int index)
		{
			bool isNew = false;
			if (singleComponents.Length <= index)
				Array.Resize(ref singleComponents, ComponentUtility.ComponentTypes.Length);

			if (singleComponents[index] == null)
			{
				singleComponents[index] = component;
				AddComponentIndex(index);
				isNew = true;
			}

			return isNew;
		}

		void AddComponents(IList<IComponent> components, bool raiseEvent)
		{
			bool isNew = false;

			for (int i = 0; i < components.Count; i++)
				isNew |= AddComponent(components[i], raiseEvent, false);

			if (isNew)
				entityManager.UpdateEntity(this);
		}

		void RemoveComponent(IComponent component, bool raiseEvent, bool updateEntity)
		{
			if (allComponents.Remove(component))
			{
				var subComponentTypes = ComponentUtility.GetSubComponentTypes(component.GetType());

				for (int i = 0; i < subComponentTypes.Length; i++)
				{
					var type = subComponentTypes[i];
					int index = ComponentUtility.GetComponentIndex(subComponentTypes[i]);
					bool isEmpty = RemoveComponentFromGroup(component, index);
					RemoveSingleComponent(component, type, index, isEmpty);
				}

				if (raiseEvent)
					OnComponentRemoved(this, component);

				if (updateEntity)
					entityManager.UpdateEntity(this);
			}
		}

		bool RemoveComponentFromGroup(IComponent component, int index)
		{
			var componentGroup = componentGroups.Length <= index ? null : componentGroups[index];
			bool isEmpty = false;

			if (componentGroup != null)
			{
				componentGroup.Remove(component);
				isEmpty = componentGroup.Components.Count == 0;
			}

			return isEmpty;
		}

		void RemoveSingleComponent(IComponent component, Type type, int index, bool isGroupEmpty)
		{
			var singleComponent = singleComponents[index];

			if (singleComponent == component)
			{
				singleComponent = isGroupEmpty ? null : FindAssignableComponent(type);
				singleComponents[index] = singleComponent;

				if (singleComponent == null)
					RemoveComponentIndex(index);
			}
		}

		IComponent FindAssignableComponent(Type type)
		{
			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];

				if (type.IsAssignableFrom(component.GetType()))
					return component;
			}

			return null;
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

			if (raiseEvent)
			{
				for (int i = 0; i < allComponents.Count; i++)
					OnComponentRemoved(this, allComponents[i]);
			}

			singleComponents.Clear();
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
			return (IComponentGroup<T>)GetComponentGroup(typeof(T), ComponentIndexHolder<T>.Index);
		}

		IComponentGroup GetComponentGroup(Type type)
		{
			return GetComponentGroup(type, ComponentUtility.GetComponentIndex(type));
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
			var componentGroupType = ComponentUtility.GetComponentGroupType(type);
			var componentGroup = (ComponentGroup)TypePoolManager.Create(componentGroupType);

			bool success = false;

			for (int i = 0; i < allComponents.Count; i++)
				success |= componentGroup.TryAdd(allComponents[i]);

			if (success)
				AddComponentIndex(ComponentUtility.GetComponentIndex(type));

			return componentGroup;
		}

		void IPoolable.OnCreate() { }

		void IPoolable.OnRecycle()
		{
			RemoveAllComponents(false);
			TypePoolManager.RecycleElements(componentGroups);
		}
	}
}
