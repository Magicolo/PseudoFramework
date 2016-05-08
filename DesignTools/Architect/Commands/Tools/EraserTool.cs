using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	//[System.Serializable]
	/*public class EraserTool : ToolCommandBase
	{

		public TileType OldTileType;

		public EraserTool(ArchitectOld architect, ArchitectTilePositionGetter tilePositionGetter) : base(architect, tilePositionGetter)
		{

		}

		public override bool Do()
		{
			return RemoveTile();
		}

		private bool RemoveTile()
		{
			if (!Layer.IsTileEmpty(TilePosition))
			{
				OldTileType = Layer[TilePosition].TileType;
				architect.RemoveTile(TilePosition);
				return true;
			}
			else
				return false;
		}

		public override void Undo()
		{
			if (!OldTileType.IsNullOrIdZero())
			{
				architect.AddTile(Layer, TileWorldPosition, TilePosition, OldTileType);
			}
		}

	}*/
}
