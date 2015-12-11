using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;
using Pseudo.Internal;
using Pseudo.Internal.Pool;
using System.Text;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	// FIXME Custom property drawers don't seem to work on components
	// TODO PEntity should give access to Unity components
	// FIXME Remove memory allocation when registering/unregistering an Entity   

	[DisallowMultipleComponent]
	[AddComponentMenu("Pseudo/General/Entity")]
	public class PEntity : PMonoBehaviour, IEntity, ISerializationCallbackReceiver, IPoolInitializable
	{
		public event Action<IComponent> OnComponentAdded;
		public event Action<IComponent> OnComponentRemoved;
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

		bool active;
		[SerializeField, PropertyField(typeof(EnumFlagsAttribute), typeof(EntityGroups))]
		ByteFlag group;
		[NonSerialized, InitializeContent]
		List<IComponent> allComponents = new List<IComponent>(8);
		[SerializeField, DoNotInitialize]
		ReferenceData[] references = new ReferenceData[0];
		[SerializeField, DoNotInitialize]
		string data;
		[NonSerialized]
		bool isDeserialized;

		public void AddComponent(IComponent component)
		{
			if (!allComponents.Contains(component))
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
			RemoveComponents(allComponents, true);
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
			return GetComponentGroup(component.GetType()).GetComponents().Contains(component);
		}

		public bool HasComponent<T>()
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

			//for (int i = 0; i < allComponents.Count; i++)
			//{
			//	var component = allComponents[i];

			//	if (component is IPoolable)
			//		((IPoolable)component).OnCreate();
			//}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			EntityManager.UnregisterEntity(this);
			TypePoolManager.RecycleElements(allComponents);
			//for (int i = 0; i < allComponents.Count; i++)
			//{
			//	var component = allComponents[i];

			//	if (component is IPoolable)
			//		((IPoolable)component).OnRecycle();
			//}
		}

		protected virtual void RaiseOnComponentAddedEvent(IComponent component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(component);

			EntityManager.UpdateEntity(this);
		}

		protected virtual void RaiseOnComponentRemovedEvent(IComponent component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(component);

			EntityManager.UpdateEntity(this);
		}

		ComponentGroup GetComponentGroup(Type type)
		{
			ComponentGroup group;

			if (!componentGroups.TryGetValue(type, out group))
			{
				group = CreateComponentGroup(type);
				componentGroups[type] = group;
			}

			return group;
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
				group.TryAdd(allComponents[i]);

			return group;
		}

		void AddComponent(IComponent component, bool raiseEvent)
		{
			component.Entity = this;
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
					enumerator.Current.Value.TryAdd(component);

				enumerator.Dispose();
			}

			if (raiseEvent)
				RaiseOnComponentAddedEvent(component);
		}

		void RemoveComponent(IComponent component, bool raiseEvent)
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

				TypePoolManager.Recycle(component);
			}
		}

		void RemoveComponents(IList<IComponent> components, bool raiseEvent)
		{
			for (int i = 0; i < components.Count; i++)
				RemoveComponent(components[i], raiseEvent);
		}

		void SerializeComponents()
		{
			ComponentSerializer.SerializeComponents(allComponents, out data, out references);
		}

		void DeserializeComponents()
		{
			if (isDeserialized || string.IsNullOrEmpty(data))
				return;

			RemoveComponents(allComponents, false);
			allComponents.Clear();

			var components = ComponentSerializer.DeserializeComponents(data, references);

			for (int i = 0; i < components.Count; i++)
				AddComponent(components[i], false);

			//isDeserialized = PoolUtility.IsPlaying;
			//isDeserialized = true;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (Application.isPlaying)
				return;

			SerializeComponents();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			DeserializeComponents();
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			DeserializeComponents();
		}

		void IPoolInitializable.OnPostPoolInitialize(List<IPoolSetter> setters) { }

		//[UnityEditor.Callbacks.DidReloadScripts]
		//static void OnScriptReload()
		//{
		//	UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabUpdated;
		//}

		//static void OnPrefabUpdated(UnityEngine.Object prefab)
		//{
		//	var entity = ((GameObject)prefab).GetComponent<PEntity>();
		//	entity.isDeserialized = false;
		//	entity.DeserializeComponents();
		//	PDebug.Log(entity, entity.references.Length, entity.allComponents, entity.data);
		//}
	}
}

[Serializable]
public class ReferenceData
{
	public int Index;
	public string Path;
	public UnityEngine.Object Reference;
}