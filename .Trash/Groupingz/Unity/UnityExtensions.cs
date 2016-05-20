using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Groupingz
{
	public static class UnityExtensions
	{
		public static void SendMessage(this IGroup<GameObject> group, string message, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			for (int i = 0; i < group.Count; i++)
				group[i].SendMessage(message, scope, options);
		}

		public static void SendMessage(this IGroup<GameObject> group, string message, object value, HierarchyScopes scope, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
		{
			for (int i = 0; i < group.Count; i++)
				group[i].SendMessage(message, value, scope, options);
		}
	}
}
