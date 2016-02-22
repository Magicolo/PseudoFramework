using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public enum MessagePropagation
	{
		Local,
		Global,
		UpwardsInclusive,
		UpwardsExclusive,
		DownwardsInclusive,
		DownwardsExclusive
	}

	public interface IEntity : IPoolable
	{
		bool Active { get; set; }
		EntityGroups Groups { get; set; }
		IList<IComponent> Components { get; }
		IEntityManager Manager { get; }
		IEntity Parent { get; }
		IList<IEntity> Children { get; }

		IList<int> GetComponentIndices();
		T GetComponent<T>() where T : class, IComponent;
		T GetComponentInChildren<T>(bool includeSelf = true, bool recursive = true) where T : class, IComponent;
		IComponent GetComponent(Type type);
		IList<T> GetComponents<T>() where T : class, IComponent;
		IList<IComponent> GetComponents(Type type);
		bool TryGetComponent<T>(out T component) where T : class, IComponent;
		bool TryGetComponentInChildren<T>(out T component, bool includeSelf = true, bool recursive = true) where T : class, IComponent;
		bool TryGetComponent(Type type, out IComponent component);
		bool HasComponent<T>() where T : class, IComponent;
		bool HasComponentInChildren<T>(bool includeSelf = true, bool recursive = true) where T : class, IComponent;
		bool HasComponent(Type type);
		bool HasComponent(IComponent component);

		void AddComponent(IComponent component);
		void AddComponents(params IComponent[] components);
		void RemoveComponent(IComponent component);
		void RemoveComponents<T>() where T : class, IComponent;
		void RemoveComponents(Type type);
		void RemoveAllComponents();

		void SendMessage(EntityMessage message);
		void SendMessage<TArg>(EntityMessage message, TArg argument);
		void SendMessage<TId>(TId identifier);
		void SendMessage<TId>(TId identifier, MessagePropagation propagation);
		void SendMessage<TId, TArg>(TId identifier, TArg argument);
		void SendMessage<TId, TArg>(TId identifier, TArg argument, MessagePropagation propagation);

		void SetParent(IEntity entity);
		bool HasChild(IEntity entity);
		void AddChild(IEntity entity);
		void RemoveChild(IEntity entity);
		void RemoveAllChildren();
	}
}
