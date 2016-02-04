using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.EntityOld
{
	public interface IEntityOld
	{
		event Action<IEntityOld, IComponentOld> OnComponentAdded;
		event Action<IEntityOld, IComponentOld> OnComponentRemoved;

		bool Active { get; set; }
		Transform Transform { get; }
		GameObject GameObject { get; }
		ByteFlag Groups { get; set; }

		void AddComponent(IComponentOld component);
		IComponentOld AddComponent(Type type);
		T AddComponent<T>() where T : IComponentOld;
		void RemoveComponent(IComponentOld component);
		void RemoveComponents(Type type);
		void RemoveComponents<T>();
		void RemoveAllComponents();
		IList<IComponentOld> GetAllComponents();
		IComponentOld GetComponent(Type type);
		T GetComponent<T>();
		IList<IComponentOld> GetComponents(Type type);
		IList<T> GetComponents<T>();
		bool TryGetComponent(Type type, out IComponentOld component);
		bool TryGetComponent<T>(out T component);
		IComponentOld GetOrAddComponent(Type type);
		T GetOrAddComponent<T>() where T : IComponentOld;
		bool HasComponent(Type type);
		bool HasComponent(IComponentOld component);
		bool HasComponent<T>();
		void SendMessage(EntityMessages message);
		void SendMessage(EntityMessages message, object argument);
		void SendMessage<T>(EntityMessages message, T argument);
	}
}