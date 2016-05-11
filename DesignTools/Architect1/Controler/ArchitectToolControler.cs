using UnityEngine;
using System.Collections.Generic;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	[System.Serializable]
	public class ArchitectToolControler
	{
		
		[Inject()]
		ArchitectControler Architect = null;
		[Inject()]
		ArchitectLayerControler LayerControler = null;

		[Disable]
		public ArchitectRotationFlip RotationFlip;
		public bool FlipY { get { return RotationFlip.FlipY; } set { RotationFlip.FlipY = value; } }
		public bool FlipX { get { return RotationFlip.FlipX; } set { RotationFlip.FlipX = value; } }
		public float Rotation { get { return RotationFlip.Angle; } set { RotationFlip.Angle = value; } }


		[Space(), Disable]
		public ToolFactory.ToolType SelectedToolType;

		[Disable]
		public TileType SelectedTileType;


		LayerData selectedLayer { get { return LayerControler.SelectedLayer; } }
		ArchitectTilePositionGetter tilePositionGetter { get { return LayerControler.TilePositionGetter; } }
		ArchitectHistory History { get{ return Architect.ArchitectHistory; } }

		public void HandleLeftMouse()
		{
			if (selectedLayer.IsInArrayBound(tilePositionGetter.TilePosition) && selectedLayer.IsActive)
				History.Do(ToolFactory.Create(SelectedToolType, this, tilePositionGetter));
		}

		public void HandlePipette()
		{
			if (selectedLayer.IsInArrayBound(tilePositionGetter.TilePosition))
				SelectedTileType = selectedLayer[tilePositionGetter.TilePosition].TileType;
		}
	}


}
