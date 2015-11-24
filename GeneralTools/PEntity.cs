using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class PEntity : PMonoBehaviour
{
	public event Action<Component> OnComponentAdded;
	public event Action<Component> OnComponentRemoved;

	Dictionary<Type, List<Component>> typeComponents;
	Dictionary<Type, List<Component>> TypeComponents
	{
		get
		{
			if (typeComponents == null)
				InitializeComponents();

			return typeComponents;
		}
	}

	public Component AddComponent(Type type)
	{
		return AddComponent(GameObject, type);
	}

	public Component AddComponent(GameObject child, Type type)
	{
		Component component = child.AddComponent(type);
		AddComponent(component, true);

		return component;
	}

	public T AddComponent<T>() where T : Component
	{
		return AddComponent<T>(GameObject);
	}

	public T AddComponent<T>(GameObject child) where T : Component
	{
		return (T)AddComponent(child, typeof(T));
	}

	public bool RemoveComponent(Component component)
	{
		List<Component> components;
		return TryGetComponents(component.GetType(), out components) && RemoveComponent(component, components, true);
	}

	public bool RemoveComponents(Type type)
	{
		List<Component> components;
		if (TryGetComponents(type, out components))
		{
			for (int i = 0; i < components.Count; i++)
			{
				Component component = components[i];
				RaiseOnComponentRemovedEvent(component);
				component.Destroy();
			}

			components.Clear();

			return true;
		}

		return false;
	}

	public bool RemoveComponents<T>()
	{
		return RemoveComponents(typeof(T));
	}

	public bool TryGetComponent(Type type, out Component component)
	{
		List<Component> components;
		bool success = TryGetComponents(type, out components);
		component = success ? components[0] : null;

		return success;
	}

	public bool TryGetComponents<T>(out List<T> components) where T : Component
	{
		List<Component> tempComponents;
		bool success = TryGetComponents(typeof(T), out tempComponents);
		components = tempComponents as List<T>;

		return success;
	}

	public bool TryGetComponent<T>(out T component) where T : Component
	{
		Component tempComponent;
		bool success = TryGetComponent(typeof(T), out tempComponent);
		component = (T)tempComponent;

		return success;
	}

	public bool TryGetComponents(Type type, out List<Component> components)
	{
		return TypeComponents.TryGetValue(type, out components) && components.Count > 0;
	}

	//new public Component GetComponent(Type type)
	//{
	//	Component component;
	//	TryGetComponent(type, out component);

	//	return component;
	//}

	//new public T GetComponent<T>() where T : Component
	//{
	//	return (T)GetComponent(typeof(T));
	//}

	//new public List<Component> GetComponents(Type type)
	//{
	//	List<Component> components;
	//	TryGetComponents(type, out components);

	//	return components;
	//}

	//new public List<T> GetComponents<T>() where T : Component
	//{
	//	return GetComponents(typeof(T)) as List<T>;
	//}

	public Component GetOrAddComponent(Type type)
	{
		Component component;

		if (!TryGetComponent(type, out component))
			component = AddComponent(type);

		return component;
	}

	public T GetOrAddComponent<T>() where T : Component
	{
		return (T)GetOrAddComponent(typeof(T));
	}

	public bool HasComponent(Component component)
	{
		List<Component> components;
		return TryGetComponents(component.GetType(), out components) && components.Contains(component);
	}

	public bool HasComponent(Type type)
	{
		List<Component> components;
		return TryGetComponents(type, out components);
	}

	public bool HasComponent<T>() where T : Component
	{
		return HasComponent(typeof(T));
	}

	protected virtual void RaiseOnComponentAddedEvent(Component component)
	{
		if (OnComponentAdded != null)
			OnComponentAdded(component);
	}

	protected virtual void RaiseOnComponentRemovedEvent(Component component)
	{
		if (OnComponentRemoved != null)
			OnComponentRemoved(component);
	}

	void AddComponent(Component component, bool raiseEvent)
	{
		Type type = component.GetType();
		List<Component> components;

		if (component is PComponent)
			((PComponent)component).Entity = this;

		if (!TryGetComponents(type, out components))
		{
			components = new List<Component>();
			TypeComponents[type] = components;
		}

		components.Add(component);

		if (raiseEvent)
			RaiseOnComponentAddedEvent(component);
	}

	bool RemoveComponent(Component component, List<Component> components, bool raiseEvent, bool removeFromList = true)
	{
		bool success = false;

		if (removeFromList)
			success = components.Remove(component);

		if (success || !removeFromList)
		{
			if (raiseEvent)
				RaiseOnComponentRemovedEvent(component);

			component.Destroy();
		}
		else
			Debug.LogError(string.Format("Component {0} is not owned by entity {1}.", component, this));

		return success;
	}

	void RemoveComponents(List<Component> components, bool raiseEvent)
	{
		for (int i = 0; i < components.Count; i++)
		{
			RemoveComponent(components[i], components, raiseEvent, false);
			components.Clear();
		}
	}

	void InitializeComponents()
	{
		typeComponents = new Dictionary<Type, List<Component>>();
		Component[] components = GetComponentsInChildren<Component>();

		for (int i = 0; i < components.Length; i++)
			AddComponent(components[i], false);
	}
}
