using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.Events;

namespace Pseudo
{
	[System.Serializable]
	public class UIDoubleClickHandler : MonoBehaviour
	{
		public float MaxTimeBetween = 0.3f;
		public float lastClickTime;

		RectTransform rectTransform;

		public DoubleClickEvent OnDoubleClick = new DoubleClickEvent();

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		void Update()
		{
			if (lastClickTime > 0)
				lastClickTime -= Time.deltaTime;


			if (Input.GetMouseButtonDown(0))
			{
				if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
					mouseClicked();

			}

		}

		private void mouseClicked()
		{
			if (lastClickTime <= 0)
			{
				lastClickTime = MaxTimeBetween;
			}
			else
			{
				lastClickTime = 0;
				OnDoubleClick.Invoke();
			}
		}
	}

	public class DoubleClickEvent : UnityEngine.Events.UnityEvent
	{

	}
}
