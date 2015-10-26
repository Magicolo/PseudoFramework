using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class ComponentExtensions
	{
		public static T AddComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.AddComponent<T>();
		}

		public static Component AddComponent(this Component component, System.Type componentType)
		{
			return component.gameObject.AddComponent(componentType);
		}

		public static T GetComponentInChildrenExclusive<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponentInChildrenExclusive<T>();
		}

		public static Component GetComponentInChildrenExclusive(this Component component, System.Type componentType)
		{
			return component.gameObject.GetComponentInChildrenExclusive(componentType);
		}

		public static T[] GetComponentsInChildrenExclusive<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponentsInChildrenExclusive<T>();
		}

		public static Component[] GetComponentsInChildrenExclusive(this Component component, System.Type componentType)
		{
			return component.gameObject.GetComponentsInChildrenExclusive(componentType);
		}

		public static T GetComponentInParentExclusive<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponentInParentExclusive<T>();
		}

		public static Component GetComponentInParentExclusive(this Component component, System.Type componentType)
		{
			return component.gameObject.GetComponentInParentExclusive(componentType);
		}

		public static T[] GetComponentsInParentExclusive<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponentsInParentExclusive<T>();
		}

		public static Component[] GetComponentsInParentExclusive(this Component component, System.Type componentType)
		{
			return component.gameObject.GetComponentsInParentExclusive(componentType);
		}

		public static T FindComponent<T>(this Component component) where T : class
		{
			return component.gameObject.FindComponent<T>();
		}

		public static Component FindComponent(this Component component, System.Type componentType)
		{
			return component.gameObject.FindComponent(componentType);
		}

		public static T[] FindComponents<T>(this Component component) where T : class
		{
			return component.gameObject.FindComponents<T>();
		}

		public static Component[] FindComponents(this Component component, System.Type componentType)
		{
			return component.gameObject.FindComponents(componentType);
		}

		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.GetOrAddComponent<T>();
		}

		public static Component GetOrAddComponent(this Component component, System.Type componentType)
		{
			return component.gameObject.GetOrAddComponent(componentType);
		}

		public static int GetHierarchyDepth(this Component component)
		{
			int depth = 0;
			Transform currentTransform = component.transform;

			while (currentTransform.parent != null)
			{
				currentTransform = currentTransform.parent;
				depth += 1;
			}

			return depth;
		}

		public static GameObject[] GetParents(this Component child)
		{
			Transform[] parentsTransform = child.transform.GetParents();
			GameObject[] parents = new GameObject[parentsTransform.Length];

			for (int i = 0; i < parentsTransform.Length; i++)
			{
				parents[i] = parentsTransform[i].gameObject;
			}

			return parents;
		}

		public static GameObject[] GetChildren(this Component parent)
		{
			Transform[] childrenTransform = parent.transform.GetChildren();
			GameObject[] children = new GameObject[childrenTransform.Length];

			for (int i = 0; i < childrenTransform.Length; i++)
			{
				children[i] = childrenTransform[i].gameObject;
			}

			return children;
		}

		public static GameObject[] GetChildrenRecursive(this Component parent)
		{
			Transform[] childrenTransform = parent.transform.GetChildrenRecursive();
			GameObject[] children = new GameObject[childrenTransform.Length];

			for (int i = 0; i < childrenTransform.Length; i++)
			{
				children[i] = childrenTransform[i].gameObject;
			}

			return children;
		}

		public static int GetChildCount(this Component parent)
		{
			return parent.transform.childCount;
		}

		public static void SortChildren(this Component parent)
		{
			parent.transform.SortChildren();
		}

		public static void SortChildrenRecursive(this Component parent)
		{
			parent.transform.SortChildrenRecursive();
		}

		public static GameObject GetChild(this Component parent, int index)
		{
			return parent.transform.GetChild(index).gameObject;
		}

		public static GameObject FindChild(this Component parent, string childName)
		{
			return parent.transform.FindChild(childName).gameObject;
		}

		public static GameObject FindChild(this Component parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChild(predicate).gameObject;
		}

		public static GameObject FindChildRecursive(this Component parent, string childName)
		{
			return parent.transform.FindChildRecursive(childName).gameObject;
		}

		public static GameObject FindChildRecursive(this Component parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildRecursive(predicate).gameObject;
		}

		public static GameObject[] FindChildren(this Component parent, string childName)
		{
			return parent.transform.FindChildren(childName).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildren(this Component parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildren(predicate).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildrenRecursive(this Component parent, string childName)
		{
			return parent.transform.FindChildrenRecursive(childName).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject[] FindChildrenRecursive(this Component parent, System.Predicate<Transform> predicate)
		{
			return parent.transform.FindChildrenRecursive(predicate).Convert<Transform, GameObject>(transform => transform.gameObject);
		}

		public static GameObject AddChild(this Component parent, string childName, PrimitiveType primitiveType)
		{
			return parent.transform.AddChild(childName, primitiveType).gameObject;
		}

		public static GameObject AddChild(this Component parent, string childName)
		{
			return parent.transform.AddChild(childName).gameObject;
		}

		public static GameObject FindOrAddChild(this Component parent, string childName, PrimitiveType primitiveType)
		{
			return parent.transform.FindOrAddChild(childName, primitiveType).gameObject;
		}

		public static GameObject FindOrAddChild(this Component parent, string childName)
		{
			return parent.transform.FindOrAddChild(childName).gameObject;
		}

		public static void SetChildrenActive(this Component parent, bool value)
		{
			parent.transform.SetChildrenActive(value);
		}

		public static void DestroyChildren(this Component parent)
		{
			parent.transform.DestroyChildren();
		}

		public static void RemoveComponent<T>(this Component component) where T : Component
		{
			T toRemove = component.GetComponent<T>();

			if (toRemove != null)
				toRemove.Destroy();
		}

		public static T GetClosest<T>(this Component source, IList<T> targets) where T : Component
		{
			float closestDistance = float.MaxValue;
			T closestTarget = null;

			for (int i = 0; i < targets.Count; i++)
			{
				T target = targets[i];
				float distance = Vector3.Distance(source.transform.position, target.transform.position);

				if (distance < closestDistance)
				{
					closestTarget = target;
					closestDistance = distance;
				}
			}

			return closestTarget;
		}

		public static T[] GetComponents<T>(this IList<Component> components) where T : Component
		{
			T[] componentArray = new T[components.Count];

			for (int i = 0; i < components.Count; i++)
				componentArray[i] = components[i].GetComponent<T>();

			return componentArray;
		}
	}
}
