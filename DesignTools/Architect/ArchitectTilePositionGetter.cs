using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class ArchitectTilePositionGetter
	{
		Vector3 cursorWorldPosition;
		Point2 tilePosition;
		Vector3 tileWorlPosition;
		LayerData layer;

		public bool Valid;

		public Point2 TilePosition { get { return tilePosition; } }
		public Vector3 TileWorldPosition { get { return tileWorlPosition; } }
		public LayerData Layer { get { return layer; } }

		public ArchitectTilePositionGetter(Vector3 position, LayerData selectedLayer)
		{
			layer = selectedLayer;
			if (selectedLayer == null)
			{
				Clear();
				Valid = false;
			}
			else
			{
				Vector3 TileP = position.Div(new Vector3(selectedLayer.TileWidth, selectedLayer.TileHeight, 1)).Round().SetValues(0, Axes.Z);
				tilePosition = new Point2((int)TileP.x, (int)TileP.y);
				tileWorlPosition = TileP.Mult(new Vector3(selectedLayer.TileWidth, selectedLayer.TileHeight, 1));
				Valid = layer.IsInLayerBound(tilePosition.X, tilePosition.Y);
			}
		}

		private void Clear()
		{
			tilePosition = Point2.Zero;
			tileWorlPosition = Vector3.zero;
		}

		public override string ToString()
		{
			return typeof(ArchitectTilePositionGetter).ToString() + " ( tile:" + tilePosition + " , world:" + TileWorldPosition + ")";
		}
	}

}
