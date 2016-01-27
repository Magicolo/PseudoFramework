using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class ArchitectToolControler : MonoBehaviour
	{

		Architect architect;

		[Disable]
		public ArchitectRotationFlip RotationFlip;
		public bool FlipY { get { return RotationFlip.FlipY; } set { RotationFlip.FlipY = value; } }
		public bool FlipX { get { return RotationFlip.FlipX; } set { RotationFlip.FlipX = value; } }
		public float Rotation { get { return RotationFlip.Angle; } set { RotationFlip.Angle = value; } }


		[Space(), Disable]
		public ToolFactory.ToolType SelectedToolType;

		[Disable]
		public TileType SelectedTileType;
	}


}
