using UnityEngine;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Editor.Internal
{
	public class DrawUtility
	{

		public static void DrawText(Vector3 position, string text)
		{
			DrawText(position, text, Color.gray);
		}
		public static void DrawText(Vector3 position, string text, Color color)
		{
#if UNITY_EDITOR
			UnityEditor.Handles.color = color;
			UnityEditor.Handles.Label(position, text);
#endif
		}
	}
}