using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public static class ObjectExtensions
	{
		public static void Destroy(this UnityEngine.Object obj, bool allowDestroyingAssets = false)
		{
			if (Application.isPlaying)
				UnityEngine.Object.Destroy(obj);
			else
				UnityEngine.Object.DestroyImmediate(obj, allowDestroyingAssets);
		}

		public static void SendMessageToObjectsOfType<T>(this UnityEngine.Object obj, string methodName, object value, bool sendToSelf = false, SendMessageOptions options = SendMessageOptions.DontRequireReceiver) where T : Component
		{
			T[] objects = UnityEngine.Object.FindObjectsOfType<T>();

			for (int i = 0; i < objects.Length; i++)
			{
				T element = objects[i];

				if (!sendToSelf && element == obj)
					continue;

				if (value == null)
					element.SendMessage(methodName, options);
				else
					element.SendMessage(methodName, value, options);
			}
		}

		public static void SendMessageToObjectsOfType<T>(this UnityEngine.Object obj, string methodName, bool sendToSelf = false, SendMessageOptions options = SendMessageOptions.DontRequireReceiver) where T : Component
		{
			obj.SendMessageToObjectsOfType<T>(methodName, sendToSelf, options);
		}
	}
}

namespace Pseudo.Internal
{
	public static class ObjectExtensions
	{
		public static string GetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName, string oldName)
		{
			int suffix = 0;
			bool uniqueName = false;
			string currentName = "";

			while (!uniqueName)
			{
				uniqueName = true;
				currentName = newName;
				if (suffix > 0) currentName += suffix.ToString();

				for (int i = 0; i < array.Count; i++)
				{
					UnityEngine.Object element = array[i];

					if (element != null && element != obj && element.name == currentName && element.name != oldName)
					{
						uniqueName = false;
						break;
					}
				}

				suffix += 1;
			}

			return currentName;
		}

		public static string GetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName, string oldName, string emptyName)
		{
			string name = obj.GetUniqueName(array, newName, oldName);

			if (string.IsNullOrEmpty(newName))
				name = obj.GetUniqueName(array, emptyName, oldName);

			return name;
		}

		public static string GetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName)
		{
			return obj.GetUniqueName(array, newName, obj.name);
		}

		public static void SetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName, string oldName, string emptyName)
		{
			obj.name = obj.GetUniqueName(array, newName, oldName, emptyName);
		}

		public static void SetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName, string oldName)
		{
			obj.name = obj.GetUniqueName(array, newName, oldName);
		}

		public static void SetUniqueName(this UnityEngine.Object obj, IList<UnityEngine.Object> array, string newName)
		{
			obj.name = obj.GetUniqueName(array, newName, obj.name);
		}
	}
}
