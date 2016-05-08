using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo
{
	public sealed class ExecutionOrderAttribute : Attribute
	{
		public readonly int Order;

		public ExecutionOrderAttribute(int order)
		{
			Order = order;
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnScriptReload()
		{
			var temp = new GameObject("Temp");

			try
			{
				var types = TypeUtility.AllTypes.Where(t => t.Is<MonoBehaviour>());

				foreach (var type in types)
				{
					var attribute = type.GetAttribute<ExecutionOrderAttribute>(true);

					if (attribute != null)
					{
						var behaviour = temp.AddComponent(type) as MonoBehaviour;
						behaviour.SetExecutionOrder(attribute.Order);
					}
				}
			}
			finally { temp.Destroy(); }
		}
#endif
	}
}
