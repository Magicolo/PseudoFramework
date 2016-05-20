using Pseudo.Communication;
using Pseudo.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace Pseudo.EntityFramework.Internal
{
	public partial class Entity : IEntity, IPoolable
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
				entityManager.AddEntity(this);
			}
		}
		public int Count
		{
			get { return allComponents.Count; }
		}

		bool active;
		EntityGroups groups;
		IEntityManager entityManager;
		IComponent[] singleComponents;
		ComponentGroupBase[] componentGroups;
		readonly List<IComponent> allComponents;
		readonly List<int> componentIndices;

		public Entity()
		{
			children = new List<IEntity>();
			readonlyChildren = children.AsReadOnly();
			singleComponents = new IComponent[ComponentUtility.ComponentTypes.Length];
			componentGroups = new ComponentGroupBase[ComponentUtility.ComponentTypes.Length];
			allComponents = new List<IComponent>();
			componentIndices = new List<int>();
		}

		public void Initialize(IEntityManager entityManager, EntityGroups groups, bool active)
		{
			this.entityManager = entityManager;
			this.groups = groups;
			this.active = active;
		}

		public T Get<T>() where T : class, IComponent
		{
			int index = ComponentIndexHolder<T>.Index;

			if (index >= singleComponents.Length)
				return default(T);
			else
				return (T)singleComponents[index];
		}

		public T Get<T>(HierarchyScopes scope) where T : class, IComponent
		{
			T component;

			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root) && Root.TryGet(out component))
				return component;

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
				return Root.Get<T>(HierarchyScopes.Descendants);

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self) && TryGet(out component))
				return component;

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.TryGet(out component, HierarchyScopes.Self))
						return component;
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGet(out component, HierarchyScopes.Self))
						return component;
				}
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGet(out component, HierarchyScopes.Descendants))
						return component;
				}
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null)
			{
				if (parent.TryGet(out component, HierarchyScopes.Self))
					return component;
			}

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null)
			{
				if (parent.TryGet(out component, HierarchyScopes.Ancestors))
					return component;
			}

			return null;
		}

		public IComponent Get(Type componentType)
		{
			Assert.IsNotNull(componentType);

			int index = ComponentUtility.GetComponentIndex(componentType);

			if (index >= singleComponents.Length)
				return null;
			else
				return singleComponents[index];
		}

		public IComponent Get(Type componentType, HierarchyScopes scope)
		{
			Assert.IsNotNull(componentType);

			IComponent component;

			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root) && Root.TryGet(componentType, out component))
				return component;

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
				return Root.Get(componentType, HierarchyScopes.Descendants);

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self) && TryGet(componentType, out component))
				return component;

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.TryGet(componentType, out component, HierarchyScopes.Self))
						return component;
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGet(componentType, out component, HierarchyScopes.Self))
						return component;
				}
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].TryGet(componentType, out component, HierarchyScopes.Descendants))
						return component;
				}
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null)
			{
				if (parent.TryGet(componentType, out component, HierarchyScopes.Self))
					return component;
			}

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null)
			{
				if (parent.TryGet(componentType, out component, HierarchyScopes.Ancestors))
					return component;
			}

			return null;
		}

		public IList<T> GetAll<T>() where T : class, IComponent
		{
			return GetComponentGroup<T>().Components;
		}

		public IList<T> GetAll<T>(HierarchyScopes scope) where T : class, IComponent
		{
			if (scope == HierarchyScopes.Self)
				return GetAll<T>();

			var components = new List<T>();
			GetAll(components, scope);

			return components;
		}

		public void GetAll<T>(List<T> components) where T : class, IComponent
		{
			Assert.IsNotNull(components);

			components.AddRange(GetAll<T>());
		}

		public void GetAll<T>(List<T> components, HierarchyScopes scope) where T : class, IComponent
		{
			Assert.IsNotNull(components);

			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root))
				components.AddRange(Root.GetAll<T>());

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
			{
				Root.GetAll(components, HierarchyScopes.Descendants);
				return;
			}

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self))
				components.AddRange(GetAll<T>());

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.GetAll(components, HierarchyScopes.Self);
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetAll(components, HierarchyScopes.Self);
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetAll(components, HierarchyScopes.Descendants);
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null)
				parent.GetAll(components, HierarchyScopes.Self);

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null)
				parent.GetAll(components, HierarchyScopes.Ancestors);
		}

		public IList<IComponent> GetAll(Type componentType)
		{
			Assert.IsNotNull(componentType);

			return GetComponentGroup(componentType).Components;
		}

		public IList<IComponent> GetAll(Type componentType, HierarchyScopes scope)
		{
			if (scope == HierarchyScopes.Self)
				return GetAll(componentType);

			var components = new List<IComponent>();
			GetAll(components, componentType, scope);

			return components;
		}

		public void GetAll(List<IComponent> components, Type componentType)
		{
			Assert.IsNotNull(components);

			components.AddRange(GetAll(componentType));
		}

		public void GetAll(List<IComponent> components, Type componentType, HierarchyScopes scope)
		{
			Assert.IsNotNull(components);
			Assert.IsNotNull(componentType);

			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root))
				components.AddRange(Root.GetAll(componentType));

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
			{
				Root.GetAll(components, componentType, HierarchyScopes.Descendants);
				return;
			}

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self))
				components.AddRange(GetAll(componentType));

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.GetAll(components, componentType, HierarchyScopes.Self);
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetAll(components, componentType, HierarchyScopes.Self);
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].GetAll(components, componentType, HierarchyScopes.Descendants);
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null)
				parent.GetAll(components, componentType, HierarchyScopes.Self);

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null)
				parent.GetAll(components, componentType, HierarchyScopes.Ancestors);
		}

		public IComponent[] GetAll()
		{
			return allComponents.ToArray();
		}

		public bool TryGet<T>(out T component) where T : class, IComponent
		{
			return (component = Get<T>()) != null;
		}

		public bool TryGet<T>(out T component, HierarchyScopes scope) where T : class, IComponent
		{
			return (component = Get<T>(scope)) != null;
		}

		public bool TryGet(Type componentType, out IComponent component)
		{
			return (component = Get(componentType)) != null;
		}

		public bool TryGet(Type componentType, out IComponent component, HierarchyScopes scope)
		{
			return (component = Get(componentType, scope)) != null;
		}

		public bool Has<T>() where T : class, IComponent
		{
			return Get<T>() != null;
		}

		public bool Has<T>(HierarchyScopes scope) where T : class, IComponent
		{
			return Get<T>(scope) != null;
		}

		public bool Has(Type componentType)
		{
			Assert.IsNotNull(componentType);

			return Get(componentType) != null;
		}

		public bool Has(Type componentType, HierarchyScopes scope)
		{
			Assert.IsNotNull(componentType);

			return Get(componentType, scope) != null;
		}

		public bool Has(IComponent component)
		{
			Assert.IsNotNull(component);

			return allComponents.Contains(component);
		}

		public bool Has(IComponent component, HierarchyScopes scope)
		{
			Assert.IsNotNull(component);

			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root) && Root.Has(component))
				return true;

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
				return Root.Has(component, HierarchyScopes.Descendants);

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self) && Has(component))
				return true;

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this && child.Has(component, HierarchyScopes.Self))
						return true;
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].Has(component, HierarchyScopes.Self))
						return true;
				}
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i].Has(component, HierarchyScopes.Descendants))
						return true;
				}
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null && parent.Has(component, HierarchyScopes.Self))
				return true;

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null && parent.Has(component, HierarchyScopes.Ancestors))
				return true;

			return false;
		}

		public bool HasAll(params IComponent[] components)
		{
			Assert.IsNotNull(components);

			for (int i = 0; i < components.Length; i++)
			{
				if (!Has(components[i]))
					return false;
			}

			return true;
		}

		public void Add(IComponent component)
		{
			Assert.IsNotNull(component);

			AddComponent(component, true);
		}

		public void AddAll(params IComponent[] components)
		{
			Assert.IsNotNull(components);

			AddComponents(components, true);
		}

		public void Remove(IComponent component)
		{
			Assert.IsNotNull(component);

			RemoveComponent(component, true);
		}

		public void RemoveAll(params IComponent[] components)
		{
			Assert.IsNotNull(components);

			RemoveComponents(components);
		}

		public void RemoveAll<T>() where T : class, IComponent
		{
			var components = GetAll<T>();

			for (int i = components.Count - 1; i >= 0; i--)
				Remove(components[i]);
		}

		public void RemoveAll(Type componentType)
		{
			var components = GetAll(componentType);

			for (int i = components.Count - 1; i >= 0; i--)
				Remove(components[i]);
		}

		public void RemoveAll()
		{
			RemoveAllComponents(true);
		}

		/// <summary>
		/// Used internally for component matching.
		/// </summary>
		/// <returns>The sorted indices of the IComponent instances attached to the IEntity instance.</returns>
		public IList<int> GetIndices()
		{
			return componentIndices;
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

		bool AddComponent(IComponent component, bool updateEntity)
		{
			if (Has(component))
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
				entityManager.AddEntity(this);

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

		void AddComponents(IList<IComponent> components, bool updateEntity)
		{
			bool isNew = false;

			for (int i = 0; i < components.Count; i++)
				isNew |= AddComponent(components[i], false);

			if (isNew && updateEntity)
				entityManager.AddEntity(this);
		}

		void RemoveComponent(IComponent component, bool updateEntity)
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
					entityManager.AddEntity(this);
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

		void RemoveComponents(IList<IComponent> components)
		{
			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i], false);

			entityManager.AddEntity(this);
		}

		void RemoveAllComponents(bool updateEntity)
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
				entityManager.AddEntity(this);
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
			ComponentGroupBase componentGroup;

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

		ComponentGroupBase CreateComponentGroup(Type componentType)
		{
			var componentGroupType = ComponentUtility.GetComponentGroupType(componentType);
			//var componentGroup = (ComponentGroupBase)TypePoolManager.Create(componentGroupType);
			var componentGroup = (ComponentGroupBase)Activator.CreateInstance(componentGroupType);

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
			RemoveAllComponents(false);
			RemoveAllChildren();
			//TypePoolManager.RecycleElements(componentGroups);
		}
	}
}
