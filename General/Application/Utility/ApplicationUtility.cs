using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Threading;
using Pseudo.Internal;

namespace Pseudo
{
	public static class ApplicationUtility
	{
		public static event Action OnUpdate
		{
			add
			{
				if (!IsPlaying)
					return;

				if (updater == null)
					updater = GameObject.AddComponent<ApplicationUpdater>();

				updater.OnUpdate += value;
			}
			remove
			{
				if (!IsPlaying || updater == null)
					return;

				updater.OnUpdate -= value;

				if (updater.OnUpdate == null)
					updater.Destroy();
			}
		}

		public static GameObject GameObject
		{
			get
			{
				if (gameObject == null)
				{
					gameObject = new GameObject("ApplicationCallbacks");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}

				return gameObject;
			}
		}

		public static bool IsPlaying
		{
			get { return isPlaying; }
		}
		public static bool IsMainThread
		{
			get { return mainThread != null && Thread.CurrentThread == mainThread; }
		}

		public static bool IsAOT
		{
			get
			{
#if UNITY_WEBGL || UNITY_IOS
				return true;
#else
				return false;
#endif
			}
		}
		public static bool IsMultiThreaded
		{
			get
			{
#if UNITY_WEBGL
				return false;
#else
				return true;
#endif
			}
		}

		static GameObject gameObject;
		static ApplicationUpdater updater;
		static bool isPlaying;
		static Thread mainThread;

		static void Initialize()
		{
#if UNITY_EDITOR
			if (Application.isPlaying != UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				isPlaying = false;
			else
#endif
				isPlaying = Application.isPlaying;

			mainThread = Thread.CurrentThread;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitializeBefore()
		{
			Initialize();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void InitializeAfter()
		{
			Initialize();
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnScriptReload()
		{
			UnityEditor.EditorApplication.playmodeStateChanged += Initialize;
		}
#endif
	}
}