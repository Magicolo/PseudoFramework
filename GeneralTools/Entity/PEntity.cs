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
	public class PEntity : PMonoBehaviour, IEntity, IPoolInitializable
	{
		public event Action<Component> OnComponentAdded;
		public event Action<Component> OnComponentRemoved;
		public Transform Transform { get { return CachedTransform; } }
		public GameObject GameObject { get { return CachedGameObject; } }
		public ByteFlag<EntityGroups> Group
		{
			get { return group; }
			set
			{
				group = value;
				EntityManager.UpdateEntity(this);
			}
		}

		readonly Dictionary<Type, ComponentGroup> componentGroups = new Dictionary<Type, ComponentGroup>();
		readonly Dictionary<byte, MessageGroup> messageGroups = new Dictionary<byte, MessageGroup>();

		[InitializeContent, NonSerialized]
		List<Component> allComponents = new List<Component>(8);
		[SerializeField, PropertyField(typeof(EnumFlagsAttribute), typeof(EntityGroups))]
		ByteFlag group;
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

			AddComponent(typeof(int));

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

		public void RemoveComponents(Type type)
		{
			RemoveComponents(GetComponents(type), true);
		}

		public void RemoveComponents<T>()
		{
			RemoveComponents(typeof(T));
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
			return GetComponentGroup(type).GetComponents().First();
		}

		new public T GetComponent<T>()
		{
			return GetComponentGroup(typeof(T)).GetComponents<T>().First();
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

		public void SendMessage(EntityMessages message)
		{
			GetMessageGroup(message).SendMessage();
		}

		public void SendMessage(EntityMessages message, object argument)
		{
			GetMessageGroup(message).SendMessage(argument);
		}

		public void SendMessage<T>(EntityMessages message, T argument)
		{
			GetMessageGroup(message).SendMessage(argument);
		}

		protected virtual void OnDestroy()
		{
			EntityManager.UnregisterEntity(this);
		}

		protected virtual void Reset()
		{
			this.SetExecutionOrder(-3);
		}

		protected virtual void RaiseOnComponentAddedEvent(Component component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(component);

			EntityManager.UpdateEntity(this);
		}

		protected virtual void RaiseOnComponentRemovedEvent(Component component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(component);

			EntityManager.UpdateEntity(this);
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

		ComponentGroup GetComponentGroup(Type type)
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

		ComponentGroup GetComponentGroup<T>()
		{
			return GetComponentGroup(typeof(T));
		}

		ComponentGroup CreateComponentGroup(Type type)
		{
			var group = new ComponentGroup(type);

			for (int i = 0; i < allComponents.Count; i++)
				group.TryAddComponent(allComponents[i]);

			return group;
		}

		MessageGroup GetMessageGroup(EntityMessages message)
		{
			MessageGroup group;

			if (!messageGroups.TryGetValue((byte)message, out group))
			{
				group = CreateMessageGroup(message);
				messageGroups[(byte)message] = group;
			}

			return group;
		}

		MessageGroup CreateMessageGroup(EntityMessages message)
		{
			var group = new MessageGroup(message.ToString());

			for (int i = 0; i < allComponents.Count; i++)
				group.TryAddComponent(allComponents[i]);

			return group;
		}

		void AddComponent(Component component, bool raiseEvent)
		{
			if (component is PEntity)
				return;

			if (component is PComponent)
				((PComponent)component).Entity = this;
			else
				EntityUtility.SetEntity(component, this);

			allComponents.Add(component);

			// Add component to component groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.TryAddComponent(component);

				enumerator.Dispose();
			}

			// Add component to message groups
			if (messageGroups.Count > 0)
			{
				var enumerator = messageGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.TryAddComponent(component);

				enumerator.Dispose();
			}

			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		void RemoveComponent(Component component, bool raiseEvent)
		{
			if (allComponents.Remove(component))
			{
				// Remove component from component groups
				if (componentGroups.Count > 0)
				{
					var enumerator = componentGroups.GetEnumerator();

					while (enumerator.MoveNext())
						enumerator.Current.Value.RemoveComponent(component);

					enumerator.Dispose();
				}

				// Remove component from message groups
				var messageEnumerator = messageGroups.GetEnumerator();

				while (messageEnumerator.MoveNext())
					messageEnumerator.Current.Value.RemoveComponent(component);

				messageEnumerator.Dispose();

				if (raiseEvent)
					RaiseOnComponentRemovedEvent(component);

				component.Destroy();
			}
		}

		void RemoveComponents(List<Component> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i], raiseEvent);
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