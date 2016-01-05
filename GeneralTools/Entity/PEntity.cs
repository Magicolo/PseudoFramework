using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	// TODO Find an elegant way to have game specific messages and groups
	// TODO Find better name for IComponent
	// TODO Support Undo when modifying components

	[DisallowMultipleComponent]
	[AddComponentMenu("Pseudo/General/Entity")]
	public partial class PEntity : PMonoBehaviour, IEntity
	{
		public event Action<IEntity, IComponent> OnComponentAdded;
		public event Action<IEntity, IComponent> OnComponentRemoved;
		public bool Active
		{
			get { return active; }
			set
			{
				if (active != value)
				{
					enabled = value;
					active = value && CachedGameObject.activeInHierarchy;
				}
			}
		}
		public Transform Transform { get { return CachedTransform; } }
		public GameObject GameObject { get { return CachedGameObject; } }
		public ByteFlag Groups
		{
			get { return groups; }
			set
			{
				groups = value;
				EntityManager.UpdateEntity(this);
			}
		}

		[NonSerialized, DoNotInitialize]
		bool active;
		[SerializeField, PropertyField(typeof(EntityGroupsAttribute))]
		ByteFlag groups;
		[NonSerialized, InitializeContent]
		List<IComponent> allComponents = new List<IComponent>(8);

		public void AddComponent(IComponent component)
		{
			AddComponent(component, true);
		}

		public IComponent AddComponent(Type type)
		{
			var component = (IComponent)TypePoolManager.Create(type);
			AddComponent(component, true);

			return component;
		}

		public T AddComponent<T>() where T : IComponent
		{
			return (T)AddComponent(typeof(T));
		}

		public void RemoveComponent(IComponent component)
		{
			RemoveComponent(component, true);
		}

		public void RemoveComponents(Type type)
		{
			RemoveComponents(GetComponents(type), true);
		}

		public void RemoveComponents<T>()
		{
			RemoveComponents(typeof(T));
		}

		public void RemoveAllComponents()
		{
			RemoveAllComponents(true);
		}

		public IList<IComponent> GetAllComponents()
		{
			return allComponents;
		}

		new public IComponent GetComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().First();
		}

		new public T GetComponent<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>().First();
		}

		new public IList<IComponent> GetComponents(Type type)
		{
			return GetComponentGroup(type).GetComponents();
		}

		new public IList<T> GetComponents<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>();
		}

		public bool TryGetComponent(Type type, out IComponent component)
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

		public IComponent GetOrAddComponent(Type type)
		{
			IComponent component;

			if (!TryGetComponent(type, out component))
				component = AddComponent(type);

			return component;
		}

		public T GetOrAddComponent<T>() where T : IComponent
		{
			return (T)GetOrAddComponent(typeof(T));
		}

		public bool HasComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().Count > 0;
		}

		public bool HasComponent(IComponent component)
		{
			return allComponents.Contains(component);
		}

		public bool HasComponent<T>()
		{
			return HasComponent(typeof(T));
		}

		void OnEnable()
		{
			active = true;
		}

		void OnDisable()
		{
			active = false;
		}

		void OnDestroy()
		{
			EntityManager.UnregisterEntity(this);
		}

		public override void OnCreate()
		{
			base.OnCreate();

			EntityManager.RegisterEntity(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			EntityManager.UnregisterEntity(this);
			RemoveAllComponents(false);
		}

		protected virtual void RaiseOnComponentAddedEvent(IComponent component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(this, component);

			EntityManager.UpdateEntity(this);
		}

		protected virtual void RaiseOnComponentRemovedEvent(IComponent component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(this, component);

			EntityManager.UpdateEntity(this);
		}

		void AddComponent(IComponent component, bool raiseEvent)
		{
			allComponents.Add(component);
			RegisterComponent(component);

			// Raise event
			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		void AddComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				AddComponent(components[i], raiseEvent);
		}

		void RemoveComponent(IComponent component, bool raiseEvent)
		{
			if (allComponents.Remove(component))
			{
				UnregisterComponent(component);

				// Raise event
				if (raiseEvent)
					RaiseOnComponentRemovedEvent(component);

				TypePoolManager.Recycle(component);
			}
		}

		void RemoveComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = components.Count - 1; i >= 0; i--)
				RemoveComponent(components[i], raiseEvent);
		}

		void RemoveAllComponents(bool raiseEvent)
		{
			for (int i = 0; i < allComponents.Count; i++)
			{
				var component = allComponents[i];
				UnregisterComponent(component);

				// Raise event
				if (raiseEvent)
					RaiseOnComponentRemovedEvent(component);

				TypePoolManager.Recycle(component);
			}

			allComponents.Clear();
		}

		void RegisterAllComponents()
		{
			for (int i = 0; i < allComponents.Count; i++)
				RegisterComponent(allComponents[i]);
		}

		void RegisterComponent(IComponent component)
		{
			component.Entity = this;
			RegisterComponentToGroups(component);
			RegisterComponentToMessageGroups(component);
			RegisterComponentToUpdateCallbacks(component);
		}

		void UnregisterComponent(IComponent component)
		{
			component.Entity = null;
			UnregisterComponentFromGroups(component);
			UnregisterComponentFromMessageGroups(component);
			UnregisterComponentFromUpdateCallbacks(component);
		}
	}
}