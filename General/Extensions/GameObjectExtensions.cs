using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public static class GameObjectExtensions
	{
		public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : class
		{
			component = gameObject.GetComponent<T>();

			return component != null;
		}

		public static bool TryGetComponent(this GameObject gameObject, Type type, out Component component)
		{
			component = gameObject.GetComponent(type);
			var success = component != null;

			return success;
		}

		public static bool TryGetComponent<T>(this GameObject gameObject, out T component, HierarchyScopes scope) where T : class
		{
			component = gameObject.GetComponent<T>(scope);

			return component != null;
		}

		public static bool TryGetComponent(this GameObject gameObject, Type type, out Component component, HierarchyScopes scope)
		{
			component = gameObject.GetComponent(type, scope);

			return component != null;
		}

		public static T GetComponent<T>(this GameObject gameObject, HierarchyScopes scope) where T : class
		{
			return gameObject.GetComponent(typeof(T), scope) as T;
		}

		public static Component GetComponent(this GameObject gameObject, Type type, HierarchyScopes scope)
		{
			return HierarchyUtility.GetComponent(gameObject.transform, type, scope);
		}

		public static void SendMessage(this GameObject gameObject, string message, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			HierarchyUtility.SendMessage(gameObject.transform, message, scope, options);
		}

		public static void SendMessage(this GameObject gameObject, string message, object value, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			HierarchyUtility.SendMessage(gameObject.transform, message, value, scope, options);
		}

		public static GameObject[] GetParents(this GameObject child)
		{
			return child.transform.GetParents().Convert(t => t.gameObject);
		}

		public static GameObject[] GetChildren(this GameObject parent, bool recursive = false)
		{
			return parent.transform.GetChildren(recursive).Convert(t => t.gameObject);
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
			return parent.transform.FindChildren(childName, recursive).Convert(t => t.gameObject);
		}

		public static GameObject[] FindChildren(this GameObject parent, Predicate<Transform> predicate, bool recursive = false)
		{
			return parent.transform.FindChildren(predicate, recursive).Convert(t => t.gameObject);
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

		public static bool RemoveComponent<T>(this GameObject gameObject) where T : class
		{
			var toRemove = gameObject.GetComponent<T>() as Component;

			if (toRemove == null)
				return false;

			toRemove.Destroy();

			return true;
		}

		public static bool RemoveComponents<T>(this GameObject gameObject) where T : class
		{
			var toRemove = gameObject.GetComponents<T>();

			for (int i = 0; i < toRemove.Length; i++)
				(toRemove[i] as Component).Destroy();

			return toRemove.Length > 0;
		}
	}
}
