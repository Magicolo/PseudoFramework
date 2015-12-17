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

namespace Pseudo2
{
	[DisallowMultipleComponent]
	public class PEntity : PMonoBehaviour, ISerializationCallbackReceiver, IPoolInitializable
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
				//EntityManager.UpdateEntity(this);
			}
		}

		//readonly Dictionary<Type, ComponentGroup> componentGroups = new Dictionary<Type, ComponentGroup>();
		//readonly Dictionary<byte, MessageGroup> messageGroups = new Dictionary<byte, MessageGroup>();

		[SerializeField, PropertyField(typeof(EnumFlagsAttribute), typeof(EntityGroups))]
		ByteFlag group;
		[SerializeField]
		string data;
		[NonSerialized]
		bool isDeserialized;
		[InitializeContent, NonSerialized]
		List<IComponent> allComponents = new List<IComponent>(8);

		public void AddComponent(IComponent component)
		{
			allComponents.Add(component);
		}

		public void RemoveComponent(IComponent component)
		{
			allComponents.Remove(component);
		}

		public IList<IComponent> GetComponents()
		{
			return allComponents;
		}

		protected virtual void RaiseOnComponentAddedEvent(Component component)
		{
			if (OnComponentAdded != null)
				OnComponentAdded(component);

			//EntityManager.UpdateEntity(this);
		}

		protected virtual void RaiseOnComponentRemovedEvent(Component component)
		{
			if (OnComponentRemoved != null)
				OnComponentRemoved(component);

			//EntityManager.UpdateEntity(this);
		}

		void AddComponent(Component component, bool raiseEvent)
		{
			//if (component is PEntity)
			//	return;

			//if (component is PComponent)
			//	((PComponent)component).Entity = this;
			//else
			//	EntityUtility.SetEntity(component, this);

			//allComponents.Add(component);

			//// Add component to component groups
			//if (componentGroups.Count > 0)
			//{
			//	var enumerator = componentGroups.GetEnumerator();

			//	while (enumerator.MoveNext())
			//		enumerator.Current.Value.TryAddComponent(component);

			//	enumerator.Dispose();
			//}

			//// Add component to message groups
			//if (messageGroups.Count > 0)
			//{
			//	var enumerator = messageGroups.GetEnumerator();

			//	while (enumerator.MoveNext())
			//		enumerator.Current.Value.TryAddComponent(component);

			//	enumerator.Dispose();
			//}

			//if (raiseEvent)
			//	RaiseOnComponentAddedEvent(component);
		}

		void RemoveComponent(IComponent component, bool raiseEvent)
		{
			//if (allComponents.Remove(component))
			//{
			//	// Remove component from component groups
			//	if (componentGroups.Count > 0)
			//	{
			//		var enumerator = componentGroups.GetEnumerator();

			//		while (enumerator.MoveNext())
			//			enumerator.Current.Value.RemoveComponent(component);

			//		enumerator.Dispose();
			//	}

			//	// Remove component from message groups
			//	var messageEnumerator = messageGroups.GetEnumerator();

			//	while (messageEnumerator.MoveNext())
			//		messageEnumerator.Current.Value.RemoveComponent(component);

			//	messageEnumerator.Dispose();

			//	if (raiseEvent)
			//		RaiseOnComponentRemovedEvent(component);

			//	component.Destroy();
			//}
		}

		void RemoveComponents(List<IComponent> components, bool raiseEvent)
		{
			//for (int i = 0; i < components.Count; i++)
			//	RemoveComponent(components[i], raiseEvent);
		}

		void Serialize()
		{
			//var writer = new StringBuilder();
			//writer.AppendLine(allComponents.Count.ToString());

			//for (int i = 0; i < allComponents.Count; i++)
			//{
			//	var component = allComponents[i];
			//	writer.AppendLine(component.GetType().FullName);
			//	writer.AppendLine(JsonUtility.ToJson(component));
			//}

			//data = writer.ToString();
		}

		void Deserialize()
		{
			//if (isDeserialized || string.IsNullOrEmpty(data))
			//	return;

			//using (var reader = new StringReader(data))
			//{
			//	int count = int.Parse(reader.ReadLine());
			//	allComponents = new List<IComponent>(count);

			//	for (int i = 0; i < count; i++)
			//	{
			//		var type = Type.GetType(reader.ReadLine());
			//		//var component = (IComponent)Activator.CreateInstance(type);
			//		//JsonUtility.FromJsonOverwrite(reader.ReadLine(), component);
			//		//Components.Add(component);
			//		allComponents.Add((IComponent)JsonUtility.FromJson(reader.ReadLine(), type));
			//	}
			//}

			//isDeserialized = true;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (Application.isPlaying)
				return;

			Serialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Deserialize();
		}

		void IPoolInitializable.OnPrePoolInitialize()
		{
			Deserialize();
		}

		void IPoolInitializable.OnPostPoolInitialize(List<IPoolSetter> setters) { }
	}
}