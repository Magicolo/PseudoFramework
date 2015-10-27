using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal
{
	public class ScreenLogger : Singleton<ScreenLogger>
	{
		public static int FontSize = 12;
		public static float Brightness = 0.75F;
		public static float Alpha = 0.9F;
		public static int MaxLines = 100;
		public static float FadeOutTime = 1;
		public static float LifeTime = 60;

		readonly List<GUIText> lines = new List<GUIText>();

		static Queue<ScreenLoggerLine> QueuedLines = new Queue<ScreenLoggerLine>();

		public static void Initialize()
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("Screen Logger");
				gameObject.transform.Reset();
				instance = gameObject.AddComponent<ScreenLogger>();
			}
		}

		public static void Log(string toLog)
		{
			string[] lines = toLog.Split('\n');

			for (int i = 0; i < lines.Length; i++)
				QueuedLines.Enqueue(new ScreenLoggerLine(lines[i], new Color(Brightness, Brightness, Brightness, Alpha)));
		}

		public static void LogWarning(string toLog)
		{
			string[] lines = toLog.Split('\n');

			for (int i = 0; i < lines.Length; i++)
				QueuedLines.Enqueue(new ScreenLoggerLine(lines[i], new Color(Brightness, Brightness, 0, Alpha)));
		}

		public static void LogError(string toLog)
		{
			string[] lines = toLog.Split('\n');

			for (int i = 0; i < lines.Length; i++)
				QueuedLines.Enqueue(new ScreenLoggerLine(lines[i], new Color(Brightness, 0, 0, Alpha)));
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts()
		{
			if (!Application.isPlaying && Instance != null)
				Instance.gameObject.Destroy();
		}
#endif

		void Update()
		{
			for (int i = QueuedLines.Count; i-- > 0;)
				AddLine(QueuedLines.Dequeue());
		}

		void AddLine(ScreenLoggerLine line)
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("Can not log to screen while application is not playing.");
				return;
			}

			GameObject child = transform.AddChild("Line").gameObject;
			child.transform.Reset();

			GUIText text = child.AddComponent<GUIText>();
			text.pixelOffset = new Vector2(5, 5);
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text.fontSize = FontSize;
			text.anchor = TextAnchor.LowerLeft;
			text.alignment = TextAlignment.Left;
			text.color = line.Color;
			text.text = line.Text;

			StartCoroutine(Fade(text));

			for (int i = 0; i < lines.Count; i++)
			{
				GUIText guiText = lines[i];
				if (guiText != null)
					guiText.pixelOffset += new Vector2(0, FontSize);
			}

			lines.Add(text);
			if (lines.Count > MaxLines)
				RemoveLine(lines[0]);
		}

		void RemoveLine(GUIText text)
		{
			lines.Remove(text);
			text.gameObject.Destroy();
		}

		IEnumerator Fade(GUIText text)
		{
			float counter = 0;

			yield return new WaitForSeconds(LifeTime);

			while (counter < FadeOutTime && text != null)
			{
				counter += Time.deltaTime;
				text.SetColor((1 - (counter / FadeOutTime)) * Alpha, Channels.A);
				yield return null;
			}

			if (text != null)
			{
				text.SetColor(0, Channels.A);
				RemoveLine(text);
			}
		}
	}
}

