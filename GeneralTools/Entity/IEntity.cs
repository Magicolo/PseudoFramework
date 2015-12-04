using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pseudo
{
	public interface IEntity
	{
		event Action<Component> OnComponentAdded;
		event Action<Component> OnComponentRemoved;

		Transform CachedTransform { get; }
		GameObject CachedGameObject { get; }
		ByteFlag<EntityGroups> Group { get; set; }

		Component AddComponent(Type type);
		Component AddComponent(GameObject child, Type type);
		T AddComponent<T>() where T : Component;
		T AddComponent<T>(GameObject child) where T : Component;
		void RemoveComponents(Type type);
		void RemoveComponents<T>();
		List<Component> GetAllComponents();
		Component GetComponent(Type type);
		T GetComponent<T>();
		List<Component> GetComponents(Type type);
		List<T> GetComponents<T>();
		bool TryGetComponent(Type type, out Component component);
		bool TryGetComponent<T>(out T component);
		Component GetOrAddComponent(Type type);
		T GetOrAddComponent<T>() where T : Component;
		bool HasComponent(Type type);
		bool HasComponent(Component component);
		bool HasComponent<T>() where T : Component;
		void SendMessage(string method);
		void SendMessage(string method, object argument);
		void SendMessage<T>(string method, T argument);
	}
}