using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class GameObjectExtensions
	{
		public static GameObject[] GetParents(this GameObject child)
		{
			Transform[] parentsTransform = child.transform.GetParents();
			GameObject[] parents = new GameObject[parentsTransform.Length];

			for (int i = 0; i < parentsTransform.Length; i++)
				parents[i] = parentsTransform[i].gameObject;

			return parents;
		}

		public static GameObject[] GetChildren(this GameObject parent)
		{
			Transform[] childrenTransform = parent.transform.GetChildren();
			GameObject[] children = new GameObject[childrenTransform.Length];

			for (int i = 0; i < childrenTransform.Length; i++)
				children[i] = childrenTransform[i].gameObject;

			return children;
		}

		public static GameObject[] GetChildrenRecursive(this GameObject parent)
		{
			Transform[] childrenTransform = parent.transform.GetChildrenRecursive();
			GameObject[] children = new GameObject[childrenTransform.Length];

			for (int i = 0; i < childrenTransform.Length; i++)
				children[i] = childrenTransform[i].gameObject;

			return children;
		}

		public static int GetChildCount(this GameObject parent)
		{
			return parent.transform.childCount;
		}

		public static GameObject GetChild(this GameObject parent, int index)
		{
			return parent.transform.GetChild(index).gameObject;
		}

		public static GameObject FindChild(this GameObject parent, string childName)
		{
			return parent.transform.FindChild(childName).gameObject;
		}

		public static GameObject FindChild(this GameObject parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChild(predicate).gameObject;
		}

		public static GameObject FindChildRecursive(this GameObject parent, string childName)
		{
			return parent.transform.FindChildRecursive(childName).gameObject;
		}

		public static GameObject FindChildRecursive(this GameObject parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildRecursive(predicate).gameObject;
		}

		public static GameObject[] FindChildren(this GameObject parent, string childName)
		{
			return parent.transform.FindChildren(childName).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildren(this GameObject parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildren(predicate).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildrenRecursive(this GameObject parent, string childName)
		{
			return parent.transform.FindChildrenRecursive(childName).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildrenRecursive(this GameObject parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildrenRecursive(predicate).Convert<Transform, GameObject>(transform => transform.gameObject);
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

		public static void SortChildren(this GameObject parent)
		{
			parent.transform.SortChildren();
		}

		public static void SortChildrenRecursive(this GameObject parent)
		{
			parent.transform.SortChildrenRecursive();
		}

		public static int GetHierarchyDepth(this GameObject gameObject)
		{
			return gameObject.transform.GetHierarchyDepth();
		}

		public static T GetComponentInChildrenExclusive<T>(this GameObject gameObject) where T : Component
		{
			return (T)gameObject.GetComponentInChildrenExclusive(typeof(T));
		}

		public static Component GetComponentInChildrenExclusive(this GameObject gameObject, Type componentType)
		{
			Component component = null;

			foreach (GameObject child in gameObject.GetChildren())
			{
				component = child.GetComponentInChildren(componentType);

				if (component != null)
					break;
			}

			return component;
		}

		public static T[] GetComponentsInChildrenExclusive<T>(this GameObject gameObject) where T : Component
		{
			return (T[])gameObject.GetComponentsInChildrenExclusive(typeof(T));
		}

		public static Component[] GetComponentsInChildrenExclusive(this GameObject gameObject, Type componentType)
		{
			List<Component> components = new List<Component>();

			foreach (GameObject child in gameObject.GetChildren())
				components.AddRange(child.GetComponentsInChildren(componentType));

			return components.ToArray();
		}

		public static T GetComponentInParentExclusive<T>(this GameObject gameObject) where T : Component
		{
			return (T)gameObject.GetComponentInParentExclusive(typeof(T));
		}

		public static Component GetComponentInParentExclusive(this GameObject gameObject, Type componentType)
		{
			Transform parent = gameObject.transform.parent;
			return parent != null ? parent.GetComponentInParent(componentType) : null;
		}

		public static T[] GetComponentsInParentExclusive<T>(this GameObject gameObject) where T : Component
		{
			return (T[])gameObject.GetComponentsInParentExclusive(typeof(T));
		}

		public static Component[] GetComponentsInParentExclusive(this GameObject gameObject, Type componentType)
		{
			Transform parent = gameObject.transform.parent;

			return parent != null ? parent.GetComponentsInParent(componentType) : new Component[0];
		}

		public static T FindComponent<T>(this GameObject gameObject) where T : class
		{
			return gameObject.FindComponent(typeof(T)) as T;
		}

		public static Component FindComponent(this GameObject gameObject, Type componentType)
		{
			Component component = gameObject.GetComponent(componentType);

			if (component == null)
			{
				component = gameObject.GetComponentInChildrenExclusive(componentType);

				if (component == null)
					component = gameObject.GetComponentInParentExclusive(componentType);
			}

			return component;
		}

		public static T[] FindComponents<T>(this GameObject gameObject) where T : class
		{
			return gameObject.FindComponents(typeof(T)) as T[];
		}

		public static Component[] FindComponents(this GameObject gameObject, Type componentType)
		{
			Component[] selfComponents = gameObject.GetComponents(componentType);
			Component[] parentsComponents = gameObject.GetComponentsInParentExclusive(componentType);
			Component[] childrenComponents = gameObject.GetComponentsInChildrenExclusive(componentType);

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

		public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
		{
			Component component = gameObject.GetComponent(componentType);

			if (component == null)
				component = gameObject.AddComponent(componentType);

			return component;
		}

		public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
		{
			T toRemove = gameObject.GetComponent<T>();

			if (toRemove != null)
				toRemove.Destroy();
		}

		public static T[] GetComponents<T>(this IList<GameObject> gameObjects) where T : Component
		{
			List<T> components = new List<T>();

			for (int i = 0; i < gameObjects.Count; i++)
				components.AddRange(gameObjects[i].GetComponents<T>());

			return components.ToArray();
		}
	}
}
