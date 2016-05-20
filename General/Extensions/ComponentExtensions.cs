using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public static class ComponentExtensions
	{
		public static bool TryGetComponent<T>(this Component self, out T component) where T : class
		{
			return self.gameObject.TryGetComponent(out component);
		}

		public static bool TryGetComponent(this Component self, Type type, out Component component)
		{
			return self.gameObject.TryGetComponent(type, out component);
		}

		public static bool TryGetComponent<T>(this Component self, out T component, HierarchyScopes scope) where T : class
		{
			return self.gameObject.TryGetComponent(out component, scope);
		}

		public static bool TryGetComponent(this Component self, Type type, out Component component, HierarchyScopes scope)
		{
			return self.gameObject.TryGetComponent(type, out component, scope);
		}

		public static T GetComponent<T>(this Component component, HierarchyScopes scope) where T : class
		{
			return component.gameObject.GetComponent<T>(scope);
		}

		public static Component GetComponent(this Component component, Type type, HierarchyScopes scope)
		{
			return component.gameObject.GetComponent(type, scope);
		}

		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.GetOrAddComponent<T>();
		}

		public static Component GetOrAddComponent(this Component component, Type type)
		{
			return component.gameObject.GetOrAddComponent(type);
		}

		public static bool RemoveComponent<T>(this Component component) where T : class
		{
			return component.gameObject.RemoveComponent<T>();
		}

		public static bool RemoveComponents<T>(this Component component) where T : class
		{
			return component.gameObject.RemoveComponent<T>();
		}

		public static void SendMessage(this Component component, string message, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			component.gameObject.SendMessage(message, scope, options);
		}

		public static void SendMessage(this Component component, string message, object value, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			component.gameObject.SendMessage(message, value, scope, options);
		}
	}
}
