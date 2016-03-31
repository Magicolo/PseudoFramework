using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;

namespace Pseudo
{
	public static class SceneUtility
	{
		static readonly List<GameObject> roots = new List<GameObject>();

		public static T[] FindComponents<T>(Scene scene) where T : class
		{
			scene.GetRootGameObjects(roots);

			return roots
				.SelectMany(g => g.GetComponentsInChildren<T>())
				.ToArray();
		}

		public static T FindComponent<T>(Scene scene) where T : class
		{
			scene.GetRootGameObjects(roots);

			for (int i = 0; i < roots.Count; i++)
			{
				var root = roots[i];
				var component = root.GetComponentInChildren<T>();

				if (component != null)
					return component;
			}

			return default(T);
		}
	}
}
