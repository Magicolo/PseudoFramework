using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public interface IEntity
	{
		event Action<IEntity, IComponent> OnComponentAdded;
		event Action<IEntity, IComponent> OnComponentRemoved;

		ByteFlag Groups { get; set; }
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
	}

}
