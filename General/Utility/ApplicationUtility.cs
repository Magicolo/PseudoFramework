using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Threading;

namespace Pseudo
{
	public static class ApplicationUtility
	{
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