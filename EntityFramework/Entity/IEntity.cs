using Pseudo.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework
{
	public interface IEntity
	{
		bool Active { get; set; }
		EntityGroups Groups { get; set; }
		IEntity Root { get; }
		IEntity Parent { get; }
		IList<IEntity> Children { get; }
		int Count { get; }

		// Component
		T Get<T>() where T : class, IComponent;
		T Get<T>(HierarchyScopes scope) where T : class, IComponent;
		IComponent Get(Type componentType);
		IComponent Get(Type componentType, HierarchyScopes scope);
		IList<T> GetAll<T>() where T : class, IComponent;
		IList<T> GetAll<T>(HierarchyScopes scope) where T : class, IComponent;
		void GetAll<T>(List<T> components) where T : class, IComponent;
		void GetAll<T>(List<T> components, HierarchyScopes scope) where T : class, IComponent;
		IList<IComponent> GetAll(Type componentType);
		IList<IComponent> GetAll(Type componentType, HierarchyScopes scope);
		void GetAll(List<IComponent> components, Type componentType);
		void GetAll(List<IComponent> components, Type componentType, HierarchyScopes scope);
		IComponent[] GetAll();
		bool TryGet<T>(out T component) where T : class, IComponent;
		bool TryGet<T>(out T component, HierarchyScopes scope) where T : class, IComponent;
		bool TryGet(Type componentType, out IComponent component);
		bool TryGet(Type componentType, out IComponent component, HierarchyScopes scope);
		bool Has<T>() where T : class, IComponent;
		bool Has<T>(HierarchyScopes scope) where T : class, IComponent;
		bool Has(Type componentType);
		bool Has(Type componentType, HierarchyScopes scope);
		bool Has(IComponent component);
		bool Has(IComponent component, HierarchyScopes scope);
		bool HasAll(params IComponent[] components);
		void Add(IComponent component);
		void AddAll(params IComponent[] components);
		void Remove(IComponent component);
		void RemoveAll(params IComponent[] component);
		void RemoveAll<T>() where T : class, IComponent;
		void RemoveAll(Type componentType);
		void RemoveAll();
		IList<int> GetIndices();

		// Messaging
		void SendMessage(EntityMessage message);
		void SendMessage<TArg>(EntityMessage message, TArg argument);
		void SendMessage<TId>(TId identifier);
		void SendMessage<TId>(TId identifier, HierarchyScopes scope);
		void SendMessage<TId, TArg>(TId identifier, TArg argument);
		void SendMessage<TId, TArg>(TId identifier, TArg argument, HierarchyScopes scope);

		// Hierarchy
		void SetParent(IEntity entity);
		bool HasChild(IEntity entity);
		void AddChild(IEntity entity);
		void RemoveChild(IEntity entity);
		void RemoveAllChildren();
	}
}
