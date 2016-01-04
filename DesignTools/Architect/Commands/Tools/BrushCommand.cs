using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class BrushCommand : ToolCommandBase
	{
		public TileType OldTileType;

		public BrushCommand(Architect architect, ArchitectTilePositionGetter tilePositionGetter) : base(architect, tilePositionGetter)
		{
		}

		public override bool Do()
		{
			if (Layer.IsTileEmpty(TilePosition))
			{
				architect.AddSelectedTileType(Layer, TileWorldPosition, TilePosition);
				return true;
			}
			else if (Layer[TilePosition].TileType != architect.SelectedTileType)
			{
				OldTileType = Layer[TilePosition].TileType;
				architect.RemoveTile(TilePosition);
				architect.AddSelectedTileType(Layer, TileWorldPosition, TilePosition);
				return true;
			}
			else
				return false;
		}

		public override void Undo()
		{
			architect.RemoveTile(TilePosition);
			if (!OldTileType.IsNullOrIdZero())
			{
				architect.AddTile(Layer, TileWorldPosition, TilePosition, OldTileType);
			}
		}
	}

}
