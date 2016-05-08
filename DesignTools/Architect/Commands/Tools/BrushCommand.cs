using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo.Architect
{
	[System.Serializable]
	public class BrushCommand : ToolCommandBase
	{
		public TileType OldTileType;
		public ArchitectRotationFlip OldRotationFlip;
		public TileType DoTileType;
		public ArchitectRotationFlip DoRotationFlip;

		public BrushCommand(ArchitectTilePositionGetter tilePositionGetter, TileType tileType, ArchitectRotationFlip RotationFlip) 
			: base(tilePositionGetter)
		{
			DoTileType = tileType;
			DoRotationFlip = RotationFlip;
		}

		public override bool Do()
		{
			if (Layer.IsTileEmpty(TilePosition))
			{
				//architect.AddTile(Layer, TileWorldPosition, TilePosition, DoTileType, DoRotationFlip);
				Layer.AddTile(TilePosition, DoTileType, DoRotationFlip);
				return true;
			}
			else if (Layer[TilePosition].TileType != DoTileType)
			//else if (Layer[TilePosition].TileType != architect.SelectedTileType)
			{
				OldTileType = Layer[TilePosition].TileType;
				OldRotationFlip = ArchitectRotationFlip.FromTransform(Layer[TilePosition].Transform);
				Layer.EmptyTile(TilePosition);
				//architect.AddSelectedTileType(Layer, TileWorldPosition, TilePosition);
				Layer.AddTile(TilePosition, DoTileType, DoRotationFlip);
				OldRotationFlip.ApplyTo(Layer[TilePosition].Transform);
				return true;
			}
			else if (!DoRotationFlip.Equals(Layer[TilePosition].Transform))
			//else if (!architect.RotationFlip.Equals(Layer[TilePosition].Transform))
			{
				//PDebug.Log(Layer[TilePosition].Transform.localScale, Layer[TilePosition].Transform.localRotation.eulerAngles.z, architect.RotationFlip);
				OldTileType = Layer[TilePosition].TileType;

				OldRotationFlip = ArchitectRotationFlip.FromTransform(Layer[TilePosition].Transform);
				DoRotationFlip.ApplyTo(Layer[TilePosition].Transform);
				//architect.RotationFlip.ApplyTo(Layer[TilePosition].Transform);
				return true;
			}
			else
				return false;
		}

		public override void Undo()
		{
			Layer.EmptyTile(TilePosition);
			if (!OldTileType.IsNullOrIdZero())
			{
				Layer.AddTile(TilePosition, OldTileType, DoRotationFlip);
				//architect.AddTile(Layer, TileWorldPosition, TilePosition, OldTileType);
				OldRotationFlip.ApplyTo(Layer[TilePosition].Transform);
			}
		}
	}

}
