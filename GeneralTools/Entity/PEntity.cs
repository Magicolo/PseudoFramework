using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public class PEntity : PMonoBehaviour, IPoolInitializable
	{
		public event Action<Component> OnComponentAdded;
		public event Action<Component> OnComponentRemoved;

		[InitializeContent]
		ComponentHolder[] allComponents = new ComponentHolder[EntityUtility.GetTotalComponentCount()];
		[DoNotInitialize]
		bool initialized;

		protected virtual void Awake()
		{
			InitializeComponents();
		}

		public Component AddComponent(Type type)
		{
			return AddComponent(GameObject, type);
		}

		public Component AddComponent(GameObject child, Type type)
		{
			Component component = child.AddComponent(type);
			AddComponent(component, true);

			return component;
		}

		public T AddComponent<T>() where T : Component
		{
			return AddComponent<T>(GameObject);
		}

		public T AddComponent<T>(GameObject child) where T : Component
		{
			return (T)AddComponent(child, typeof(T));
		}

		public bool RemoveComponent(Component component)
		{
			List<Component> components;
			return TryGetComponents(component.GetType(), out components) && RemoveComponent(component, components, true);
		}

		public bool RemoveComponents(Type type)
		{
			List<Component> components;
			if (TryGetComponents(type, out components))
			{
				for (int i = 0; i < components.Count; i++)
				{
					Component component = components[i];
					RaiseOnComponentRemovedEvent(component);
					component.Destroy();
				}

				components.Clear();

				return true;
			}

			return false;
		}

		public bool RemoveComponents<T>()
		{
			return RemoveComponents(typeof(T));
		}

		public bool TryGetComponent(Type type, out Component component)
		{
			List<Component> components;
			bool success = TryGetComponents(type, out components);
			component = success ? components[0] : null;

			return success;
		}

		public bool TryGetComponent<T>(out T component) where T : Component
		{
			Component tempComponent;
			bool success = TryGetComponent(typeof(T), out tempComponent);
			component = (T)tempComponent;

			return success;
		}

		public bool TryGetComponents(Type type, out List<Component> components)
		{
			var componentHolder = allComponents[EntityUtility.GetComponentIndex(type)];

			if (componentHolder == null)
			{
				components = null;
				return false;
			}
			else
			{
				components = componentHolder.GetComponents();
				return components.Count > 0;
			}
		}

		public Component GetOrAddComponent(Type type)
		{
			Component component;

			if (!TryGetComponent(type, out component))
				component = AddComponent(type);

			return component;
		}

		public T GetOrAddComponent<T>() where T : Component
		{
			return (T)GetOrAddComponent(typeof(T));
		}

		public bool HasComponent(Component component)
		{
			List<Component> components;
			return TryGetComponents(component.GetType(), out components) && components.Contains(component);
		}

		public bool HasComponent(Type type)
		{
			List<Component> components;
			return TryGetComponents(type, out components);
		}

		public bool HasComponent<T>() where T : Component
		{
			return HasComponent(typeof(T));
		}

		public override void OnCreate()
		{
			base.OnCreate();

			for (int i = 0; i < allComponents.Length; i++)
			{
				var componentHolder = allComponents[i];

				if (componentHolder == null)
					continue;

				var components = componentHolder.GetComponents();

				for (int j = 0; j < components.Count; j++)
				{
					var component = components[j];

					if (component is IPoolable)
						((IPoolable)component).OnCreate();
				}
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			for (int i = 0; i < allComponents.Length; i++)
			{
				var component = allComponents[i];

				if (component is IPoolable)
					((IPoolable)component).OnRecycle();
			}
		}

		protected virtual void RaiseOnComponentAddedEvent(Component component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(component);
		}

		protected virtual void RaiseOnComponentRemovedEvent(Component component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(component);
		}

		void AddComponent(Component component, bool raiseEvent)
		{
			if (component is PEntity)
				return;

			if (component is IComponent)
				((IComponent)component).Entity = this;

			var index = EntityUtility.GetComponentIndex(component.GetType());
			var componentHolder = allComponents[index];

			if (componentHolder == null)
			{
				componentHolder = new ComponentHolder();
				allComponents[index] = componentHolder;
			}

			componentHolder.AddComponent(component);

			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		bool RemoveComponent(Component component, List<Component> components, bool raiseEvent, bool removeFromList = true)
		{
			bool success = false;

			if (removeFromList)
				success = components.Remove(component);

			if (success || !removeFromList)
			{
				if (raiseEvent)
					RaiseOnComponentRemovedEvent(component);

				component.Destroy();
			}
			else
				Debug.LogError(string.Format("Component {0} is not owned by entity {1}.", component, this));

			return success;
		}

		void RemoveComponents(List<Component> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i], components, raiseEvent, false);

			components.Clear();
		}

		void InitializeComponents()
		{
			if (initialized)
				return;

			var components = GetComponents<Component>();

			for (int i = 0; i < components.Length; i++)
				AddComponent(components[i], false);

			components = GetComponentsInChildren<Component>(true);

			for (int i = 0; i < components.Length; i++)
				AddComponent(components[i], false);

			initialized = true;
		}

		void IPoolInitializable.OnBeforePoolInitialize()
		{
			InitializeComponents();
		}

		void IPoolInitializable.OnAfterPoolInitialize(List<IPoolSetter> setters) { }
	}
}