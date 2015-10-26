using UnityEngine;
using System.Collections;
using System;

namespace Pseudo
{
	public static class MonoBehaviourExtensions
	{

		public static void SetScriptIcon(this MonoBehaviour behaviour, Texture2D icon)
		{
#if UNITY_EDITOR
			string currentIconPath = UnityEditor.EditorPrefs.GetString(behaviour.GetType().AssemblyQualifiedName + "Icon", "");
			string iconPath = HelperFunctions.GetAssetPath(icon);

			if (currentIconPath != iconPath)
			{
				UnityEditor.MonoScript script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour);
				typeof(UnityEditor.EditorGUIUtility).GetMethod("SetIconForObject", ObjectExtensions.AllFlags).Invoke(null, new object[] { script, icon });
				typeof(UnityEditor.EditorUtility).GetMethod("ForceReloadInspectors", ObjectExtensions.AllFlags).Invoke(null, null);
				typeof(UnityEditor.MonoImporter).GetMethod("CopyMonoScriptIconToImporters", ObjectExtensions.AllFlags).Invoke(null, new object[] { script });
				UnityEditor.EditorPrefs.SetString(behaviour.GetType().AssemblyQualifiedName + "Icon", iconPath);
			}
#endif
		}

		public static void SetExecutionOrder(this MonoBehaviour behaviour, int order)
		{
#if UNITY_EDITOR
			UnityEditor.MonoScript script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour);

			if (UnityEditor.MonoImporter.GetExecutionOrder(script) != order)
			{
				UnityEditor.MonoImporter.SetExecutionOrder(script, order);
			}
#endif
		}

		public static void InvokeDelayed(this MonoBehaviour behaviour, Action action, float delay, Func<float> getDeltaTime = null)
		{
			if (delay > 0f)
				behaviour.StartCoroutine(InvokeRoutine(action, delay, getDeltaTime));
			else
				action();
		}

		public static void SetTransformHasChanged(this MonoBehaviour behaviour, bool hasChanged)
		{
			behaviour.StartCoroutine(SetHasChanged(behaviour.transform, hasChanged));
		}

		public static void SetTransformHasChanged(this MonoBehaviour behaviour, Transform transform, bool hasChanged)
		{
			behaviour.StartCoroutine(SetHasChanged(transform, hasChanged));
		}

		static IEnumerator InvokeRoutine(Action action, float delay, Func<float> getDeltaTime)
		{
			getDeltaTime = getDeltaTime ?? delegate { return Time.deltaTime; };

			for (float counter = 0; counter < delay; counter += getDeltaTime())
				yield return null;

			action();
		}

		static IEnumerator SetHasChanged(Transform transform, bool hasChanged)
		{
			yield return new WaitForEndOfFrame();

			transform.hasChanged = hasChanged;
		}
	}
}
