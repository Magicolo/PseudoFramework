using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public static class ButtonExtension
{
	public static void SetAnchors(this Button button, Vector2 dimension, Vector2 anchorMin, Vector2 anchorMax)
	{
		RectTransform trans = button.GetComponent<RectTransform>();
		trans.anchorMin = anchorMin;
		trans.anchorMax = anchorMax;
		trans.sizeDelta = dimension;
	}
}
