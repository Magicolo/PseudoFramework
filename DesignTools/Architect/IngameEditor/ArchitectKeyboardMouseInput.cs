using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class ArchitectKeyboardMouseInput : MonoBehaviour
	{

		ArchitectOld architect;

		InputCombinaisonChecker undoInput = new InputCombinaisonChecker(true, KeyCode.Z, KeyCode.LeftControl);

		public void Awake()
		{
			architect = GetComponent<ArchitectOld>();
		}

		void Update()
		{
			undoInput.Update();
			
			/*if (UnityEngine.Input.GetMouseButton(0))
				architect.HandleLeftMouse();
			else if (UnityEngine.Input.GetMouseButton(1))
				architect.HandlePipette();
				*/
			undoInput.Update();
			if (undoInput.GetKeyCombinaison())
				architect.Undo();

			handleKeyboardShortcut();
		}

		private void handleKeyboardShortcut()
		{
			/*if (UnityEngine.Input.GetKeyDown(KeyCode.E))
				architect.SelectedToolType = ToolFactory.ToolType.Eraser;
			else if (UnityEngine.Input.GetKeyDown(KeyCode.B))
				architect.SelectedToolType = ToolFactory.ToolType.Brush;
			else if (UnityEngine.Input.GetKeyDown(KeyCode.R))
				architect.Rotate();
			else if (UnityEngine.Input.GetKeyDown(KeyCode.X))
				architect.FlipX();
			else if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
				architect.FlipY();*/
		}

	}

}
