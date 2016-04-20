using UnityEngine;
using System.Collections;
using System;
using Pseudo.Internal;
using Pseudo.Reflection;

namespace Pseudo
{
	public static class MonoBehaviourExtensions
	{
		public static void SetScriptIcon(this MonoBehaviour behaviour, Texture2D icon)
		{
#if UNITY_EDITOR
			var currentIconPath = UnityEditor.EditorPrefs.GetString(behaviour.GetType().AssemblyQualifiedName + "Icon", "");
			var iconPath = AssetDatabaseUtility.GetAssetPath(icon);

			if (currentIconPath != iconPath)
			{
				var script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour);
				typeof(UnityEditor.EditorGUIUtility).GetMethod("SetIconForObject", ReflectionUtility.AllFlags).Invoke(null, new object[] { script, icon });
				typeof(UnityEditor.EditorUtility).GetMethod("ForceReloadInspectors", ReflectionUtility.AllFlags).Invoke(null, null);
				typeof(UnityEditor.MonoImporter).GetMethod("CopyMonoScriptIconToImporters", ReflectionUtility.AllFlags).Invoke(null, new object[] { script });
				UnityEditor.EditorPrefs.SetString(behaviour.GetType().AssemblyQualifiedName + "Icon", iconPath);
			}
#endif
		}

		public static void SetExecutionOrder(this MonoBehaviour behaviour, int order)
		{
#if UNITY_EDITOR
			var script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour);

			if (UnityEditor.MonoImporter.GetExecutionOrder(script) != order)
				UnityEditor.MonoImporter.SetExecutionOrder(script, order);
#endif
		}
	}
}
