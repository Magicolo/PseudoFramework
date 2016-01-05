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
			//TODO REFACTOR ME PLEASE !!!!!
			else if (Layer[TilePosition].TileType != architect.SelectedTileType
				|| Layer[TilePosition].Transform.rotation != architect.PreviewSprite.transform.rotation
				|| Layer[TilePosition].Transform.localScale != architect.PreviewSprite.transform.localScale)
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
