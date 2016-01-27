using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class ArchitectKeyboardMouseInput : MonoBehaviour
	{

		Architect architect;

		InputCombinaisonChecker undoInput = new InputCombinaisonChecker(true, KeyCode.Z, KeyCode.LeftControl);

		public void Awake()
		{
			architect = GetComponent<Architect>();
		}

		void Update()
		{
			undoInput.Update();

			if (Input.GetMouseButton(0))
				architect.HandleLeftMouse();
			else if (Input.GetMouseButton(1))
				architect.HandlePipette();

			undoInput.Update();
			if (undoInput.GetKeyCombinaison())
				architect.Undo();

			handleKeyboardShortcut();
		}

		private void handleKeyboardShortcut()
		{
			if (Input.GetKeyDown(KeyCode.E))
				architect.SelectedToolType = ToolFactory.ToolType.Eraser;
			else if (Input.GetKeyDown(KeyCode.B))
				architect.SelectedToolType = ToolFactory.ToolType.Brush;
			else if (Input.GetKeyDown(KeyCode.R))
				architect.Rotate();
			else if (Input.GetKeyDown(KeyCode.X))
				architect.FlipX();
			else if (Input.GetKeyDown(KeyCode.Y))
				architect.FlipY();
		}

	}

}
