using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class ToolbarPanel : MonoBehaviour
	{
		public ArchitectOld Architect;

		public Button BrushButton;
		public Button EraserButton;

		Button SelectedButton;

		void Awake()
		{
			Architect = GetComponentInParent<ArchitectOld>();

		}

		void Start()
		{
			SelectedButton = BrushButton;
			Architect.UISkin.Select(BrushButton);
		}

		public void SwitchToBrush()
		{
			Architect.SelectedToolType = ToolFactory.ToolType.Brush;
		}

		public void SwitchToEraser()
		{
			Architect.SelectedToolType = ToolFactory.ToolType.Eraser;
		}

		private void SwitchTo(Button button)
		{
			Architect.UISkin.Enable(SelectedButton);
			Architect.UISkin.Select(button);
			SelectedButton = button;
		}

		public void Refresh()
		{
			switch (Architect.SelectedToolType)
			{
				case ToolFactory.ToolType.Brush:
					SwitchTo(BrushButton);
					break;

				case ToolFactory.ToolType.Eraser:
					SwitchTo(EraserButton);
					break;
			}
		}
	}

}
