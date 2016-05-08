using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Architect
{
	public class ArchitectLayerControler
	{
		public ArchitectControler Architect;
		public LayerData SelectedLayer;

		ArchitectTilePositionGetter tilePositionGetter = new ArchitectTilePositionGetter(Vector3.zero, null);
		public ArchitectTilePositionGetter TilePositionGetter { get { return tilePositionGetter; } }

		void Update()
		{
			UpdateTileGetter();
		}

		private void UpdateTileGetter()
		{
			ArchitectTilePositionGetter newTilePositionGetter = new ArchitectTilePositionGetter(Architect.MapCam.GetMouseWorldPosition(), SelectedLayer);
			if (newTilePositionGetter.TilePosition != tilePositionGetter.TilePosition)
				tilePositionGetter = newTilePositionGetter;
		}

	}
}