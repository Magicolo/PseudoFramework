using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class ToolFactory
	{

		public static ToolCommandBase Create(ToolType tool, Architect architect, ArchitectTilePositionGetter getter)
		{
			switch (tool)
			{
				case ToolType.Brush: return new BrushCommand(architect, getter);
				case ToolType.Eraser: return new EraserTool(architect, getter);
			}
			return null;
		}
		public enum ToolType { Brush, Eraser };
	}

}
