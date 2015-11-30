using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Pool;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Pseudo/General/Entity")]
	public class PEntity : PMonoBehaviour, IPoolInitializable
	{
		public event Action<Component> OnComponentAdded;
		public event Action<Component> OnComponentRemoved;

		[SerializeField, PropertyField(typeof(FlagAttribute), typeof(EntityMatch.Groups))]
		ulong group;
		public EntityMatch.Groups Group
		{
			get { return (EntityMatch.Groups)group; }
			set
			{
				group = (ulong)value;
				EntityManager.UpdateEntity(this);
			}
		}

		readonly Dictionary<Type, ComponentGroup> componentGroups = new Dictionary<Type, ComponentGroup>();

		[InitializeContent, NonSerialized]
		List<Component> allComponents = new List<Component>();

		[DoNotInitialize, NonSerialized]
		bool initialized;

		public Component AddComponent(Type type)
		{
			return AddComponent(CachedGameObject, type);
		}

		public Component AddComponent(GameObject child, Type type)
		{
			Component component = child.AddComponent(type);
			AddComponent(component, true);

			return component;
		}

		public T AddComponent<T>() where T : Component
		{
			return AddComponent<T>(CachedGameObject);
		}

		public T AddComponent<T>(GameObject child) where T : Component
		{
			return (T)AddComponent(child, typeof(T));
		}

		public bool RemoveComponents(Type type)
		{
			return RemoveComponents(GetComponents(type), true);
		}

		public bool RemoveComponents<T>()
		{
			return RemoveComponents(typeof(T));
		}

		public List<Component> GetAllComponents()
		{
			InitializeComponents();

			return allComponents;
		}

		new public List<T> GetComponents<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>();
		}

		new public List<Component> GetComponents(Type type)
		{
			return GetComponentGroup(type).GetComponents();
		}

		new public Component GetComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().FirstOrDefault();
		}

		new public T GetComponent<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>().FirstOrDefault();
		}

		public bool TryGetComponent(Type type, out Component component)
		{
			var components = GetComponents(type);

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

		public bool TryGetComponent<T>(out T component)
		{
			var components = GetComponents<T>();

			if (components.Count > 0)
			{
				component = components[0];
				return true;
			}
			else
			{
				component = default(T);
				return false;
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
			return GetComponentGroup(component.GetType()).GetComponents().Contains(component);
		}

		public bool HasComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().Count > 0;
		}

		public bool HasComponent<T>() where T : Component
		{
			return HasComponent(typeof(T));
		}

		public ComponentGroup GetComponentGroup(Type type)
		{
			InitializeComponents();
			ComponentGroup group;

			if (!componentGroups.TryGetValue(type, out group))
			{
				group = CreateComponentGroup(type);
				componentGroups[type] = group;
			}

			return group;
		}

		public ComponentGroup GetComponentGroup<T>()
		{
			return GetComponentGroup(typeof(T));
		}

		public override void OnCreate()
		{
			base.OnCreate();

			InitializeComponents();
			EntityManager.RegisterEntity(this);

			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];

				if (component is IPoolable)
					((IPoolable)component).OnCreate();
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			EntityManager.UnregisterEntity(this);

			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];

				if (component is IPoolable)
					((IPoolable)component).OnRecycle();
			}
		}

		protected virtual void OnDestroy()
		{
			EntityManager.UnregisterEntity(this);
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

		ComponentGroup CreateComponentGroup(Type type)
		{
			var group = new ComponentGroup(type);

			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];

				if (type.IsAssignableFrom(component.GetType()))
					group.AddComponent(component);
			}

			return group;
		}

		void AddComponent(Component component, bool raiseEvent)
		{
			if (component is PEntity)
				return;

			if (component is PComponent)
				((PComponent)component).Entity = this;

			allComponents.Add(component);

			var type = component.GetType();
			var enumerator = componentGroups.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var group = enumerator.Current;

				if (group.Key.IsAssignableFrom(type))
					group.Value.AddComponent(component);
			}

			enumerator.Dispose();

			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		bool RemoveComponent(Component component, bool raiseEvent)
		{
			bool success = allComponents.Remove(component);

			if (success)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.RemoveComponent(component);

				enumerator.Dispose();

				if (raiseEvent)
					RaiseOnComponentRemovedEvent(component);

				component.Destroy();
			}
			else
				Debug.LogError(string.Format("Component {0} is not owned by entity {1}.", component, this));

			return success;
		}

		bool RemoveComponents(List<Component> components, bool raiseEvent)
		{
			bool success = false;

			for (int i = 0; i < components.Count; i++)
				success |= RemoveComponent(components[i], raiseEvent);

			return success;
		}

		void InitializeComponents()
		{
			if (initialized)
				return;

			var components = GetComponentsInChildren<Component>(true);

			for (int i = 0; i < components.Length; i++)
				AddComponent(components[i], false);

			initialized = true;
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			InitializeComponents();
		}

		void IPoolInitializable.OnPostPoolInitialize(List<IPoolSetter> setters) { }
	}
}