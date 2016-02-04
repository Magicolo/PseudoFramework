using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IEntity : IPoolable
	{
		event Action<IEntity, IComponent> OnComponentAdded;
		event Action<IEntity, IComponent> OnComponentRemoved;

		EntityGroups Groups { get; set; }
		IList<IComponent> Components { get; }

		IList<int> GetComponentIndices();
		T GetComponent<T>() where T : IComponent;
		IComponent GetComponent(Type type);
		IList<T> GetComponents<T>() where T : IComponent;
		IList<IComponent> GetComponents(Type type);
		bool TryGetComponent<T>(out T component) where T : IComponent;
		bool TryGetComponent(Type type, out IComponent component);
		bool HasComponent<T>() where T : IComponent;
		bool HasComponent(Type type);
		bool HasComponent(IComponent component);

		void AddComponent(IComponent component);
		void AddComponents(params IComponent[] components);
		void RemoveComponent(IComponent component);
		void RemoveComponents<T>() where T : IComponent;
		void RemoveComponents(Type type);
		void RemoveAllComponents();

		void SendMessage<TId>(TId identifier);
		void SendMessage<TId, TArg>(TId identifier, TArg argument);
		void SendMessage<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2);
		void SendMessage<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3);

		void Recycle();
	}
}
