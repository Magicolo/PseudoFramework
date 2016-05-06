using UnityEngine;
using System.Collections.Generic;


namespace Pseudo.Architect
{
	[System.Serializable]
	public class ToolFactory
	{

		public static ToolCommandBase Create(ToolType tool, ArchitectToolControler toolControler, ArchitectTilePositionGetter getter)
		{
			switch (tool)
			{
				case ToolType.Brush: return new BrushCommand(getter, toolControler.SelectedTileType, toolControler.RotationFlip);
				//case ToolType.Eraser: return new EraserTool(architect, getter);
			}
			return null;
		}
		public enum ToolType { Brush, Eraser };
	}

}
