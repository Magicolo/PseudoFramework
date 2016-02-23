using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	[Flags]
	public enum HierarchyScope
	{
		Local = 1 << 0,
		Global = 1 << 1,
		Upwards = 1 << 2,
		Downwards = 1 << 3,
		Lateral = 1 << 4,
	}

	public interface IEntity : IPoolable
	{
		bool Active { get; set; }
		EntityGroups Groups { get; set; }
		IList<IComponent> Components { get; }
		IEntity Root { get; }
		IEntity Parent { get; }
		IList<IEntity> Children { get; }

		IList<int> GetComponentIndices();
		T GetComponent<T>() where T : class, IComponent;
		T GetComponent<T>(HierarchyScope scope) where T : class, IComponent;
		IComponent GetComponent(Type componentType);
		IComponent GetComponent(Type componentType, HierarchyScope scope);
		IList<T> GetComponents<T>() where T : class, IComponent;
		IList<IComponent> GetComponents(Type componentType);
		bool TryGetComponent<T>(out T component) where T : class, IComponent;
		bool TryGetComponent<T>(out T component, HierarchyScope scope) where T : class, IComponent;
		bool TryGetComponent(Type componentType, out IComponent component);
		bool TryGetComponent(Type componentType, out IComponent component, HierarchyScope scope);
		bool HasComponent<T>() where T : class, IComponent;
		bool HasComponent<T>(HierarchyScope scope) where T : class, IComponent;
		bool HasComponent(Type componentType);
		bool HasComponent(Type componentType, HierarchyScope scope);
		bool HasComponent(IComponent component);
		bool HasComponent(IComponent component, HierarchyScope scope);

		void AddComponent(IComponent component);
		void AddComponents(params IComponent[] components);
		void RemoveComponent(IComponent component);
		void RemoveComponents<T>() where T : class, IComponent;
		void RemoveComponents(Type componentType);
		void RemoveAllComponents();

		void SendMessage(EntityMessage message);
		void SendMessage<TArg>(EntityMessage message, TArg argument);
		void SendMessage<TId>(TId identifier);
		void SendMessage<TId>(TId identifier, HierarchyScope scope);
		void SendMessage<TId, TArg>(TId identifier, TArg argument);
		void SendMessage<TId, TArg>(TId identifier, TArg argument, HierarchyScope scope);

		void SetParent(IEntity entity);
		bool HasChild(IEntity entity);
		void AddChild(IEntity entity);
		void RemoveChild(IEntity entity);
		void RemoveAllChildren();
	}
}
