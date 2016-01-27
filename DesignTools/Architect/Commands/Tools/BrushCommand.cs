using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class BrushCommand : ToolCommandBase
	{
		public TileType OldTileType;
		public ArchitectRotationFlip OldRotationFlip;
		public TileType DoTileType;
		public ArchitectRotationFlip DoRotationFlip;

		public BrushCommand(Architect architect, ArchitectTilePositionGetter tilePositionGetter) : base(architect, tilePositionGetter)
		{
			DoTileType = architect.SelectedTileType;
			DoRotationFlip = architect.RotationFlip;
		}

		public override bool Do()
		{
			if (Layer.IsTileEmpty(TilePosition))
			{
				architect.AddTile(Layer, TileWorldPosition, TilePosition, DoTileType, DoRotationFlip);
				return true;
			}
			else if (Layer[TilePosition].TileType != architect.SelectedTileType)
			{
				OldTileType = Layer[TilePosition].TileType;
				OldRotationFlip = ArchitectRotationFlip.FromTransform(Layer[TilePosition].Transform);
				architect.RemoveTile(TilePosition);
				architect.AddSelectedTileType(Layer, TileWorldPosition, TilePosition);
				OldRotationFlip.ApplyTo(Layer[TilePosition].Transform);
				return true;
			}
			else if (!architect.RotationFlip.Equals(Layer[TilePosition].Transform))
			{

				PDebug.Log(Layer[TilePosition].Transform.localScale, Layer[TilePosition].Transform.localRotation.eulerAngles.z, architect.RotationFlip);
				OldTileType = Layer[TilePosition].TileType;

				OldRotationFlip = ArchitectRotationFlip.FromTransform(Layer[TilePosition].Transform);
				architect.RotationFlip.ApplyTo(Layer[TilePosition].Transform);
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
				OldRotationFlip.ApplyTo(Layer[TilePosition].Transform);
			}
		}
	}

}
