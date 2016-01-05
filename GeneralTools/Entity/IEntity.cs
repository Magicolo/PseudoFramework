<<<<<<< HEAD
﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public interface IEntity
	{
		event Action<IEntity, IComponent> OnComponentAdded;
		event Action<IEntity, IComponent> OnComponentRemoved;

		bool Active { get; set; }
		Transform Transform { get; }
		GameObject GameObject { get; }
		EntityGroupDefinition Groups { get; set; }

		void AddComponent(IComponent component);
		IComponent AddComponent(Type type);
		T AddComponent<T>() where T : IComponent;
		void RemoveComponent(IComponent component);
		void RemoveComponents(Type type);
		void RemoveComponents<T>();
		void RemoveAllComponents();
		IList<IComponent> GetAllComponents();
		IComponent GetComponent(Type type);
		T GetComponent<T>();
		IList<IComponent> GetComponents(Type type);
		IList<T> GetComponents<T>();
		bool TryGetComponent(Type type, out IComponent component);
		bool TryGetComponent<T>(out T component);
		IComponent GetOrAddComponent(Type type);
		T GetOrAddComponent<T>() where T : IComponent;
		bool HasComponent(Type type);
		bool HasComponent(IComponent component);
		bool HasComponent<T>();
		void SendMessage(EntityMessages message);
		void SendMessage(EntityMessages message, object argument);
		void SendMessage<T>(EntityMessages message, T argument);
	}
=======
﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public interface IEntity
	{
		event Action<IEntity, IComponent> OnComponentAdded;
		event Action<IEntity, IComponent> OnComponentRemoved;

		bool Active { get; set; }
		Transform Transform { get; }
		GameObject GameObject { get; }
		ByteFlag Groups { get; set; }

		void AddComponent(IComponent component);
		IComponent AddComponent(Type type);
		T AddComponent<T>() where T : IComponent;
		void RemoveComponent(IComponent component);
		void RemoveComponents(Type type);
		void RemoveComponents<T>();
		void RemoveAllComponents();
		IList<IComponent> GetAllComponents();
		IComponent GetComponent(Type type);
		T GetComponent<T>();
		IList<IComponent> GetComponents(Type type);
		IList<T> GetComponents<T>();
		bool TryGetComponent(Type type, out IComponent component);
		bool TryGetComponent<T>(out T component);
		IComponent GetOrAddComponent(Type type);
		T GetOrAddComponent<T>() where T : IComponent;
		bool HasComponent(Type type);
		bool HasComponent(IComponent component);
		bool HasComponent<T>();
		void SendMessage(EntityMessages message);
		void SendMessage(EntityMessages message, object argument);
		void SendMessage<T>(EntityMessages message, T argument);
	}
>>>>>>> e6176370f888e6e8807b0b4438b063cfc4318d34
}