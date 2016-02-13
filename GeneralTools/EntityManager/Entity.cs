using Pseudo.Internal.Communication;
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
		public IEntity Parent
		{
			get { return parent; }
		}
		public IList<IEntity> Children
		{
			get { return readonlyChildren; }
		}
		public IEntityManager Manager
		{
			get { return entityManager; }
		}
		public IList<IComponent> Components
		{
			get { return readonlyComponents; }
		}

		bool active;
		EntityGroups groups;
		IEntity parent;
		IEntityManager entityManager = null;
		MessageManager messageManager = null;
		[DoNotInitialize]
		IComponent[] singleComponents;
		[DoNotInitialize]
		ComponentGroup[] componentGroups;
		readonly List<IEntity> children;
		readonly IList<IEntity> readonlyChildren;
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

			return allComponents.Contains(component);
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
			RemoveAllComponents(true, true);
		}

		public void SendMessage(EntityMessage message)
		{
			SendMessage(message.Message.Value, (object)null, message.Propagation);
		}

		public void SendMessage<TArg>(EntityMessage message, TArg argument)
		{
			SendMessage(message.Message.Value, argument, message.Propagation);
		}

		public void SendMessage<TId>(TId identifier)
		{
			SendMessage(identifier, (object)null, MessagePropagation.Local);
		}

		public void SendMessage<TId>(TId identifier, MessagePropagation propagation)
		{
			SendMessage(identifier, (object)null, propagation);
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument)
		{
			SendMessage(identifier, argument, MessagePropagation.Local);
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument, MessagePropagation propagation)
		{
			if (!Active)
				return;

			switch (propagation)
			{
				case MessagePropagation.Local:
					for (int i = allComponents.Count - 1; i >= 0; i--)
					{
						var component = allComponents[i];

						if (component.Active)
							messageManager.Send(component, identifier, argument);
					}
					break;
				case MessagePropagation.Global:
					entityManager.Entities.BroadcastMessage(identifier, argument, MessagePropagation.Local);
					break;
				case MessagePropagation.UpwardsInclusive:
					SendMessage(identifier, argument, MessagePropagation.Local);
					SendMessage(identifier, argument, MessagePropagation.UpwardsExclusive);
					break;
				case MessagePropagation.UpwardsExclusive:
					if (parent != null)
						parent.SendMessage(identifier, argument, MessagePropagation.UpwardsInclusive);
					break;
				case MessagePropagation.DownwardsInclusive:
					SendMessage(identifier, argument, MessagePropagation.Local);
					SendMessage(identifier, argument, MessagePropagation.DownwardsExclusive);
					break;
				case MessagePropagation.DownwardsExclusive:
					for (int i = 0; i < children.Count; i++)
						children[i].SendMessage(identifier, argument, MessagePropagation.DownwardsInclusive);
					break;
			}
		}

		public void SetParent(IEntity entity)
		{
			if (parent == entity)
				return;

			if (parent != null)
				parent.RemoveChild(this);

			parent = entity;

			if (parent != null)
				parent.AddChild(this);
		}

		public bool HasChild(IEntity entity)
		{
			return children.Contains(entity);
		}

		public void AddChild(IEntity entity)
		{
			Assert.IsNotNull(entity);

			if (!HasChild(entity))
			{
				children.Add(entity);
				entity.SetParent(this);
			}
		}

		public void RemoveChild(IEntity entity)
		{
			Assert.IsNotNull(entity);

			if (HasChild(entity))
			{
				children.Remove(entity);
				entity.SetParent(null);
			}
		}

		public void RemoveAllChildren()
		{
			for (int i = children.Count - 1; i >= 0; i--)
				RemoveChild(children[i]);
		}

		void SetActive(bool active)
		{
			if (this.active != active)
			{
				this.active = active;

				if (this.active)
					SendMessage(ComponentMessages.OnEntityActivated);
				else
					SendMessage(ComponentMessages.OnEntityDeactivated);
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

			messageManager.Send(component, ComponentMessages.OnAdded);

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
				messageManager.Send(component, ComponentMessages.OnRemoved);

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

				messageManager.Send(component, ComponentMessages.OnRemoved);
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
			RemoveAllComponents(false, false);
			RemoveAllChildren();
			TypePoolManager.RecycleElements(componentGroups);
		}
	}
}
