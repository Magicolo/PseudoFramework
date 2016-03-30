using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pseudo.Internal
{
	public class GameManager : MonoBehaviour, IGameManager
	{
		[Serializable]
		public class FadeData
		{
			public float Duration = 0.5f;
			public Canvas Canvas;
			public Image Image;
		}

		public FadeData FadeSettings;

		public GameStates State { get; private set; }
		public Scene Scene { get; private set; }

		public void LoadScene(string scene)
		{
			if (State == GameStates.Loading)
				return;

			StartCoroutine(LoadRoutine(scene));
		}

		public void ReloadScene()
		{
			LoadScene(SceneManager.GetActiveScene().name);
		}

		void Awake()
		{
			Scene = SceneManager.GetActiveScene();
		}

		void SwitchState(GameStates state)
		{
			switch (State)
			{
				case GameStates.Loading:
					break;
				case GameStates.Playing:
					break;
			}

			State = state;

			switch (State)
			{
				case GameStates.Loading:
					break;
				case GameStates.Playing:
					break;
			}
		}

		IEnumerator LoadRoutine(string scene)
		{
			SwitchState(GameStates.Loading);
			yield return new WaitForEndOfFrame();

			// Fade In
			if (FadeSettings.Canvas != null && FadeSettings.Image != null)
			{
				FadeSettings.Canvas.gameObject.SetActive(true);

				for (float i = 0; i < FadeSettings.Duration; i += UnityEngine.Time.unscaledDeltaTime)
				{
					FadeSettings.Image.color = new Color(0f, 0f, 0f, i / FadeSettings.Duration);
					yield return null;
				}
			}

			// Load Scene
			Scene = SceneManager.GetSceneByName(scene);
			var loadingTask = SceneManager.LoadSceneAsync(scene);

			while (!loadingTask.isDone)
				yield return null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			// Fade Out
			if (FadeSettings.Canvas != null && FadeSettings.Image != null)
			{
				for (float i = 0; i < FadeSettings.Duration; i += UnityEngine.Time.unscaledDeltaTime)
				{
					FadeSettings.Image.color = new Color(0f, 0f, 0f, 1f - i / FadeSettings.Duration);
					yield return null;
				}

				FadeSettings.Canvas.gameObject.SetActive(false);
			}

			SwitchState(GameStates.Playing);
		}
	}
}
