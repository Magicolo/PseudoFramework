using Pseudo.Communication;
using Pseudo.Communication.Internal;
using Pseudo.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pseudo.EntityFramework.Internal
{
	public partial class Entity : IEntity
	{
		public bool Active
		{
			get { return active; }
			set { SetActive(value); }
		}
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

		bool active;
		EntityGroups groups;
		IEntityManager entityManager = null;
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
			children = new List<IEntity>();
			readonlyChildren = children.AsReadOnly();
			singleComponents = new IComponent[ComponentUtility.ComponentTypes.Length];
			componentGroups = new ComponentGroup[ComponentUtility.ComponentTypes.Length];
			allComponents = new List<IComponent>();
			readonlyComponents = allComponents.AsReadOnly();
			componentIndices = new List<int>();
			readonlyComponentIndices = componentIndices.AsReadOnly();
		}

		public void Initialize(IEntityManager entityManager, MessageManager messageManager, EntityGroups groups, bool active)
		{
			this.entityManager = entityManager;
			this.messageManager = messageManager;
			this.groups = groups;
			this.active = active;
		}

		public IList<int> GetComponentIndices()
		{
			return readonlyComponentIndices;
		}

		public T GetComponent<T>() where T : class, IComponent
		{
			int index = ComponentIndexHolder<T>.Index;

			if (index >= singleComponents.Length)
				return default(T);
			else
				return (T)singleComponents[index];
		}

		public T GetComponent<T>(HierarchyScope scope) where T : class, IComponent
		{
			T component;

			if ((scope & HierarchyScope.Global) != 0)
				return Root.GetComponent<T>(HierarchyScope.Local | HierarchyScope.Children);

			if ((scope & HierarchyScope.Root) != 0 && Root.TryGetComponent(out component))
				return component;

			if ((scope & HierarchyScope.Local) != 0 && TryGetComponent(out component))
				return component;

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.TryGetComponent(out component, HierarchyScope.Local))
						return component;
				}
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
			{
				if (parent.TryGetComponent(out component, HierarchyScope.Local))
					return component;
			}

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
			{
				if (parent.TryGetComponent(out component, HierarchyScope.Local | HierarchyScope.Parents))
					return component;
			}

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGetComponent(out component, HierarchyScope.Local | HierarchyScope.Children))
						return component;
				}
			}

			return null;
		}

		public IComponent GetComponent(Type componentType)
		{
			Assert.IsNotNull(componentType);

			int index = ComponentUtility.GetComponentIndex(componentType);

			if (index >= singleComponents.Length)
				return null;
			else
				return singleComponents[index];
		}

		public IComponent GetComponent(Type componentType, HierarchyScope scope)
		{
			Assert.IsNotNull(componentType);

			IComponent component;

			if ((scope & HierarchyScope.Global) != 0)
				return Root.GetComponent(componentType, HierarchyScope.Local | HierarchyScope.Children);

			if ((scope & HierarchyScope.Root) != 0 && Root.TryGetComponent(componentType, out component))
				return component;

			if ((scope & HierarchyScope.Local) != 0 && TryGetComponent(componentType, out component))
				return component;

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.TryGetComponent(componentType, out component, HierarchyScope.Local))
						return component;
				}
			}

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGetComponent(componentType, out component, HierarchyScope.Local | HierarchyScope.Children))
						return component;
				}
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
			{
				if (parent.TryGetComponent(componentType, out component, HierarchyScope.Local))
					return component;
			}

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
			{
				if (parent.TryGetComponent(componentType, out component, HierarchyScope.Local | HierarchyScope.Parents))
					return component;
			}

			return null;
		}

		public IList<T> GetComponents<T>() where T : class, IComponent
		{
			return GetComponentGroup<T>().Components;
		}

		public IList<T> GetComponents<T>(HierarchyScope scope) where T : class, IComponent
		{
			if (scope == HierarchyScope.Local)
				return GetComponents<T>();

			var components = new List<T>();
			GetComponents(components, scope);

			return components;
		}

		public void GetComponents<T>(List<T> components, HierarchyScope scope) where T : class, IComponent
		{
			Assert.IsNotNull(components);

			if ((scope & HierarchyScope.Global) != 0)
				Root.GetComponents(components, HierarchyScope.Local | HierarchyScope.Children);

			if ((scope & HierarchyScope.Root) != 0)
				components.AddRange(Root.GetComponents<T>());

			if ((scope & HierarchyScope.Local) != 0)
				components.AddRange(GetComponents<T>());

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.GetComponents(components, HierarchyScope.Local);
				}
			}

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetComponents(components, HierarchyScope.Local | HierarchyScope.Children);
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
				parent.GetComponents(components, HierarchyScope.Local);

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
				parent.GetComponents(components, HierarchyScope.Local | HierarchyScope.Parents);
		}

		public IList<IComponent> GetComponents(Type componentType)
		{
			Assert.IsNotNull(componentType);

			return GetComponentGroup(componentType).Components;
		}

		public IList<IComponent> GetComponents(Type componentType, HierarchyScope scope)
		{
			if (scope == HierarchyScope.Local)
				return GetComponents(componentType);

			var components = new List<IComponent>();
			GetComponents(components, componentType, scope);

			return components;
		}

		public void GetComponents(List<IComponent> components, Type componentType, HierarchyScope scope)
		{
			Assert.IsNotNull(components);
			Assert.IsNotNull(componentType);

			if ((scope & HierarchyScope.Global) != 0)
				Root.GetComponents(components, componentType, HierarchyScope.Local | HierarchyScope.Children);

			if ((scope & HierarchyScope.Root) != 0)
				components.AddRange(Root.GetComponents(componentType));

			if ((scope & HierarchyScope.Local) != 0)
				components.AddRange(GetComponents(componentType));

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.GetComponents(components, componentType, HierarchyScope.Local);
				}
			}

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetComponents(components, componentType, HierarchyScope.Local | HierarchyScope.Children);
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
				parent.GetComponents(components, componentType, HierarchyScope.Local);

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
				parent.GetComponents(components, componentType, HierarchyScope.Local | HierarchyScope.Parents);
		}

		public bool TryGetComponent<T>(out T component) where T : class, IComponent
		{
			component = GetComponent<T>();

			return component != null;
		}

		public bool TryGetComponent<T>(out T component, HierarchyScope scope) where T : class, IComponent
		{
			component = GetComponent<T>(scope);

			return component != null;
		}

		public bool TryGetComponent(Type componentType, out IComponent component)
		{
			Assert.IsNotNull(componentType);

			component = GetComponent(componentType);

			return component != null;
		}

		public bool TryGetComponent(Type componentType, out IComponent component, HierarchyScope scope)
		{
			component = GetComponent(componentType, scope);

			return component != null;
		}

		public bool HasComponent<T>() where T : class, IComponent
		{
			return GetComponent<T>() != null;
		}

		public bool HasComponent<T>(HierarchyScope scope) where T : class, IComponent
		{
			return GetComponent<T>(scope) != null;
		}

		public bool HasComponent(Type componentType)
		{
			Assert.IsNotNull(componentType);

			return GetComponent(componentType) != null;
		}

		public bool HasComponent(Type componentType, HierarchyScope scope)
		{
			Assert.IsNotNull(componentType);

			return GetComponent(componentType, scope) != null;
		}

		public bool HasComponent(IComponent component)
		{
			Assert.IsNotNull(component);

			return allComponents.Contains(component);
		}

		public bool HasComponent(IComponent component, HierarchyScope scope)
		{
			Assert.IsNotNull(component);

			if ((scope & HierarchyScope.Global) != 0)
				return Root.HasComponent(component, HierarchyScope.Local | HierarchyScope.Children);

			if ((scope & HierarchyScope.Root) != 0 && Root.HasComponent(component))
				return true;

			if ((scope & HierarchyScope.Local) != 0 && HasComponent(component))
				return true;

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.HasComponent(component, HierarchyScope.Local))
						return true;
				}
			}

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].HasComponent(component, HierarchyScope.Local | HierarchyScope.Children))
						return true;
				}
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
			{
				if (parent.HasComponent(component, HierarchyScope.Local))
					return true;
			}

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
			{
				if (parent.HasComponent(component, HierarchyScope.Local | HierarchyScope.Parents))
					return true;
			}

			return false;
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

		public void RemoveComponents<T>() where T : class, IComponent
		{
			var components = GetComponents<T>();

			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i]);
		}

		public void RemoveComponents(Type componentType)
		{
			var components = GetComponents(componentType);

			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i]);
		}

		public void RemoveAllComponents()
		{
			RemoveAllComponents(true, true);
		}

		void SetActive(bool active)
		{
			if (this.active != active)
			{
				if (active)
				{
					active = false;

					for (int i = 0; i < allComponents.Count; i++)
						allComponents[i].OnEntityDeactivated();
				}
				else
				{
					active = true;

					for (int i = 0; i < allComponents.Count; i++)
						allComponents[i].OnEntityActivated();
				}
			}
		}

		bool AddComponent(IComponent component, bool raiseEvent, bool updateEntity)
		{
			if (HasComponent(component))
				return false;

			allComponents.Add(component);
			component.Entity = this;

			var subComponentTypes = ComponentUtility.GetSubComponentTypes(component.GetType());
			bool isNew = false;

			for (int i = 0; i < subComponentTypes.Length; i++)
			{
				int index = ComponentUtility.GetComponentIndex(subComponentTypes[i]);
				AddComponentToGroup(component, index);
				isNew |= AddSingleComponent(component, index);
			}

			if (isNew && updateEntity)
				entityManager.UpdateEntity(this);

			component.OnAdded();

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
				component.OnRemoved();

				var subComponentTypes = ComponentUtility.GetSubComponentTypes(component.GetType());

				for (int i = 0; i < subComponentTypes.Length; i++)
				{
					var type = subComponentTypes[i];
					int index = ComponentUtility.GetComponentIndex(subComponentTypes[i]);
					bool isEmpty = RemoveComponentFromGroup(component, index);
					RemoveSingleComponent(component, type, index, isEmpty);
				}

				component.Entity = null;

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

		void RemoveSingleComponent(IComponent component, Type componentType, int index, bool isGroupEmpty)
		{
			var singleComponent = singleComponents[index];

			if (singleComponent == component)
			{
				singleComponent = isGroupEmpty ? null : FindAssignableComponent(componentType);
				singleComponents[index] = singleComponent;

				if (singleComponent == null)
					RemoveComponentIndex(index);
			}
		}

		IComponent FindAssignableComponent(Type componentType)
		{
			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];

				if (componentType.IsAssignableFrom(component.GetType()))
					return component;
			}

			return null;
		}

		void RemoveComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i], raiseEvent, false);

			entityManager.UpdateEntity(this);
		}

		void RemoveAllComponents(bool raiseEvent, bool updateEntity)
		{
			for (int i = componentGroups.Length - 1; i >= 0; i--)
			{
				var componentGroup = componentGroups[i];

				if (componentGroup != null)
					componentGroup.RemoveAll();
			}

			for (int i = allComponents.Count - 1; i >= 0; i--)
			{
				var component = allComponents[i];

				component.OnRemoved();
				component.Entity = null;
			}

			singleComponents.Clear();
			allComponents.Clear();
			componentIndices.Clear();

			if (updateEntity)
				entityManager.UpdateEntity(this);
		}

		void AddComponentIndex(int index)
		{
			if (componentIndices.Count == 0 || componentIndices.Last() < index)
				componentIndices.Add(index);
			else
			{
				for (int i = 0; i < componentIndices.Count; i++)
				{
					int componentIndex = componentIndices[i];

					if (componentIndex == index)
						break;
					else if (componentIndex > index)
					{
						componentIndices.Insert(i, index);
						break;
					}
				}
			}
		}

		void RemoveComponentIndex(int index)
		{
			componentIndices.Remove(index);
		}

		IComponentGroup<T> GetComponentGroup<T>() where T : class, IComponent
		{
			return (IComponentGroup<T>)GetComponentGroup(typeof(T), ComponentIndexHolder<T>.Index);
		}

		IComponentGroup GetComponentGroup(Type componentType)
		{
			return GetComponentGroup(componentType, ComponentUtility.GetComponentIndex(componentType));
		}

		IComponentGroup GetComponentGroup(Type componentType, int typeIndex)
		{
			ComponentGroup componentGroup;

			if (componentGroups.Length <= typeIndex)
			{
				Array.Resize(ref componentGroups, ComponentUtility.ComponentTypes.Length);
				componentGroup = CreateComponentGroup(componentType);
				componentGroups[typeIndex] = componentGroup;
			}
			else
			{
				componentGroup = componentGroups[typeIndex];

				if (componentGroup == null)
				{
					componentGroup = CreateComponentGroup(componentType);
					componentGroups[typeIndex] = componentGroup;
				}
			}

			return componentGroup;
		}

		ComponentGroup CreateComponentGroup(Type componentType)
		{
			var componentGroupType = ComponentUtility.GetComponentGroupType(componentType);
			var componentGroup = (ComponentGroup)TypePoolManager.Create(componentGroupType);

			bool success = false;

			for (int i = 0; i < allComponents.Count; i++)
				success |= componentGroup.TryAdd(allComponents[i]);

			if (success)
				AddComponentIndex(ComponentUtility.GetComponentIndex(componentType));

			return componentGroup;
		}

		void IPoolable.OnCreate() { }

		void IPoolable.OnRecycle()
		{
			RemoveAllComponents(false, false);
			RemoveAllChildren();
			TypePoolManager.RecycleElements(componentGroups);
		}
	}
}
