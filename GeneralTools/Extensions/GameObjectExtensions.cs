using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class GameObjectExtensions
	{
		static readonly Func<Transform, GameObject> transformToGameObject = transform => transform.gameObject;

		public static GameObject[] GetParents(this GameObject child)
		{
			return child.transform.GetParents().Convert(transformToGameObject);
		}

		public static GameObject[] GetChildren(this GameObject parent, bool recursive = false)
		{
			return parent.transform.GetChildren(recursive).Convert(transformToGameObject);
		}

		public static GameObject GetChild(this GameObject parent, int index)
		{
			return parent.transform.GetChild(index).gameObject;
		}

		public static GameObject FindChild(this GameObject parent, string childName, bool recursive = false)
		{
			return parent.transform.FindChild(childName, recursive).gameObject;
		}

		public static GameObject FindChild(this GameObject parent, Predicate<Transform> predicate, bool recursive = false)
		{
			return parent.transform.FindChild(predicate, recursive).gameObject;
		}

		public static GameObject[] FindChildren(this GameObject parent, string childName, bool recursive = false)
		{
			return parent.transform.FindChildren(childName, recursive).Convert(transformToGameObject);
		}

		public static GameObject[] FindChildren(this GameObject parent, Predicate<Transform> predicate, bool recursive = false)
		{
			return parent.transform.FindChildren(predicate, recursive).Convert(transformToGameObject);
		}

		public static GameObject AddChild(this GameObject parent, string childName, PrimitiveType primitiveType)
		{
			return parent.transform.AddChild(childName, primitiveType).gameObject;
		}

		public static GameObject AddChild(this GameObject parent, string childName)
		{
			return parent.transform.AddChild(childName).gameObject;
		}

		public static GameObject FindOrAddChild(this GameObject parent, string childName, PrimitiveType primitiveType)
		{
			return parent.transform.FindOrAddChild(childName, primitiveType).gameObject;
		}

		public static GameObject FindOrAddChild(this GameObject parent, string childName)
		{
			return parent.transform.FindOrAddChild(childName).gameObject;
		}

		public static T GetComponentInChildren<T>(this GameObject gameObject, bool exclusive) where T : Component
		{
			return (T)gameObject.GetComponentInChildren(typeof(T), exclusive);
		}

		public static Component GetComponentInChildren(this GameObject gameObject, Type type, bool exclusive)
		{
			if (!exclusive)
				return gameObject.GetComponentInChildren(type);

			Component component = null;
			Transform[] children = gameObject.transform.GetChildren();

			for (int i = 0; i < children.Length; i++)
			{
				GameObject child = children[i].gameObject;
				component = child.GetComponentInChildren(type);

				if (component != null)
					break;
			}

			return component;
		}

		public static T[] GetComponentsInChildren<T>(this GameObject gameObject, bool includeInactive, bool exclusive) where T : class
		{
			return gameObject.GetComponentsInChildren(typeof(T), includeInactive, exclusive) as T[];
		}

		public static Component[] GetComponentsInChildren(this GameObject gameObject, Type type, bool includeInactive, bool exclusive)
		{
			if (!exclusive)
				return gameObject.GetComponentsInChildren(type, includeInactive);

			List<Component> components = new List<Component>();
			Transform[] children = gameObject.transform.GetChildren();

			for (int i = 0; i < children.Length; i++)
			{
				GameObject child = children[i].gameObject;
				components.AddRange(child.GetComponentsInChildren(type, includeInactive, exclusive));
			}

			return components.ToArray();
		}

		public static T GetComponentInParent<T>(this GameObject gameObject, bool exclusive) where T : Component
		{
			return (T)gameObject.GetComponentInParent(typeof(T), exclusive);
		}

		public static Component GetComponentInParent(this GameObject gameObject, Type type, bool exclusive)
		{
			if (!exclusive)
				return gameObject.GetComponentInParent(type);

			Transform parent = gameObject.transform.parent;

			return parent != null ? parent.GetComponentInParent(type) : null;
		}

		public static T[] GetComponentsInParent<T>(this GameObject gameObject, bool includeInactive, bool exclusive) where T : class
		{
			return gameObject.GetComponentsInParent(typeof(T), includeInactive, exclusive) as T[];
		}

		public static Component[] GetComponentsInParent(this GameObject gameObject, Type type, bool includeInactive, bool exclusive)
		{
			if (!exclusive)
				return gameObject.GetComponentsInParent(type);

			Transform parent = gameObject.transform.parent;

			return parent != null ? parent.GetComponentsInParent(type, includeInactive) : new Component[0];
		}

		public static T FindComponent<T>(this GameObject gameObject) where T : class
		{
			return gameObject.FindComponent(typeof(T)) as T;
		}

		public static Component FindComponent(this GameObject gameObject, Type type)
		{
			Component component = gameObject.GetComponent(type);

			if (component == null)
			{
				component = gameObject.GetComponentInChildren(type, true);

				if (component == null)
					component = gameObject.GetComponentInParent(type, true);
			}

			return component;
		}

		public static T[] FindComponents<T>(this GameObject gameObject, bool includeInactive) where T : class
		{
			return gameObject.FindComponents(typeof(T), includeInactive) as T[];
		}

		public static Component[] FindComponents(this GameObject gameObject, Type type, bool includeInactive)
		{
			Component[] selfComponents = gameObject.GetComponents(type);
			Component[] parentsComponents = gameObject.GetComponentsInParent(type, includeInactive, true);
			Component[] childrenComponents = gameObject.GetComponentsInChildren(type, includeInactive, true);
			Component[] components = new Component[selfComponents.Length + parentsComponents.Length + childrenComponents.Length];

			selfComponents.CopyTo(components, 0);
			parentsComponents.CopyTo(components, selfComponents.Length);
			childrenComponents.CopyTo(components, selfComponents.Length + parentsComponents.Length);

			return components;
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return (T)gameObject.GetOrAddComponent(typeof(T));
		}

		public static Component GetOrAddComponent(this GameObject gameObject, Type type)
		{
			Component component = gameObject.GetComponent(type);

			if (component == null)
				component = gameObject.AddComponent(type);

			return component;
		}

		public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
		{
			T toRemove = gameObject.GetComponent<T>();

			if (toRemove != null)
				toRemove.Destroy();
		}

		public static void RemoveComponents<T>(this GameObject gameObject) where T : Component
		{
			T[] toRemove = gameObject.GetComponents<T>();

			for (int i = 0; i < toRemove.Length; i++)
				toRemove[i].Destroy();
		}
	}
}
