using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[DisallowMultipleComponent]
	public partial class PEntity : PMonoBehaviour, IEntityOld
	{
		public event Action<IEntityOld, IComponentOld> OnComponentAdded;
		public event Action<IEntityOld, IComponentOld> OnComponentRemoved;
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
				EntityManagerOld.UpdateEntity(this);
			}
		}

		[NonSerialized, DoNotInitialize]
		bool active;
		[SerializeField, PropertyField(typeof(EntityGroupsAttribute))]
		ByteFlag groups;
		[NonSerialized, InitializeContent]
		List<IComponentOld> allComponents = new List<IComponentOld>(8);

		public void AddComponent(IComponentOld component)
		{
			AddComponent(component, true);
		}

		public IComponentOld AddComponent(Type type)
		{
			var component = (IComponentOld)TypePoolManager.Create(type);
			AddComponent(component, true);

			return component;
		}

		public T AddComponent<T>() where T : IComponentOld
		{
			return (T)AddComponent(typeof(T));
		}

		public void RemoveComponent(IComponentOld component)
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

		public IList<IComponentOld> GetAllComponents()
		{
			return allComponents;
		}

		new public IComponentOld GetComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().First();
		}

		new public T GetComponent<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>().First();
		}

		new public IList<IComponentOld> GetComponents(Type type)
		{
			return GetComponentGroup(type).GetComponents();
		}

		new public IList<T> GetComponents<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>();
		}

		public bool TryGetComponent(Type type, out IComponentOld component)
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

		public IComponentOld GetOrAddComponent(Type type)
		{
			IComponentOld component;

			if (!TryGetComponent(type, out component))
				component = AddComponent(type);

			return component;
		}

		public T GetOrAddComponent<T>() where T : IComponentOld
		{
			return (T)GetOrAddComponent(typeof(T));
		}

		public bool HasComponent(Type type)
		{
			return GetComponentGroup(type).GetComponents().Count > 0;
		}

		public bool HasComponent(IComponentOld component)
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
			EntityManagerOld.UnregisterEntity(this);
		}

		public override void OnCreate()
		{
			base.OnCreate();

			EntityManagerOld.RegisterEntity(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			EntityManagerOld.UnregisterEntity(this);
			RemoveAllComponents(false);
		}

		protected virtual void RaiseOnComponentAddedEvent(IComponentOld component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(this, component);

			EntityManagerOld.UpdateEntity(this);
		}

		protected virtual void RaiseOnComponentRemovedEvent(IComponentOld component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(this, component);

			EntityManagerOld.UpdateEntity(this);
		}

		void AddComponent(IComponentOld component, bool raiseEvent)
		{
			allComponents.Add(component);
			RegisterComponent(component);

			// Raise event
			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		void AddComponents(IList<IComponentOld> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				AddComponent(components[i], raiseEvent);
		}

		void RemoveComponent(IComponentOld component, bool raiseEvent)
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

		void RemoveComponents(IList<IComponentOld> components, bool raiseEvent)
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

		void RegisterComponent(IComponentOld component)
		{
			component.Entity = this;
			RegisterComponentToGroups(component);
			RegisterComponentToMessageGroups(component);
			RegisterComponentToUpdateCallbacks(component);
		}

		void UnregisterComponent(IComponentOld component)
		{
			component.Entity = null;
			UnregisterComponentFromGroups(component);
			UnregisterComponentFromMessageGroups(component);
			UnregisterComponentFromUpdateCallbacks(component);
		}
	}
}