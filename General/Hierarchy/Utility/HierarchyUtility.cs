using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public static class HierarchyUtility
	{
		static readonly GetComponentHierarchyAction getComponent = new GetComponentHierarchyAction();
		static readonly SendMessageHierarchyAction sendMessage = new SendMessageHierarchyAction();
		static readonly SendMessageWithArgumentHierarchyAction sendMessageWithArgument = new SendMessageWithArgumentHierarchyAction();

		public static Component GetComponent(Transform transform, Type type, HierarchyScopes scope)
		{
			return getComponent.ApplyToHierarchy(transform, ref type, scope);
		}

		public static void SendMessage(Transform transform, string message, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			var info = new MessageInfo { Message = message, Options = options };
			sendMessage.ApplyToHierarchy(transform, ref info, scope);
		}

		public static void SendMessage(Transform transform, string message, object value, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			var info = new MessageInfo { Message = message, Value = value, Options = options };
			sendMessageWithArgument.ApplyToHierarchy(transform, ref info, scope);
		}
	}
}
