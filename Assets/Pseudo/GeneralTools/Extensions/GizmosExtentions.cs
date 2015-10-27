using UnityEngine;

public static class GizmosExtentions
{
	public static void DrawText(this Gizmos gizmos, Vector3 position, string text)
	{
		DrawText(gizmos, position, text, Color.gray);
	}
	public static void DrawText(this Gizmos gizmos, Vector3 position, string text, Color color)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.color = color;
		UnityEditor.Handles.Label(position, text);
#endif
	}

	public static void nothing(this Gizmos gizmos)
	{
	}
}
