using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	public class ArchitectLayerControler
	{
		//[Inject()]
		//ArchitectControler Architect = null;

		[NonSerialized]
		public LayerData SelectedLayer;

		ArchitectTilePositionGetter tilePositionGetter = new ArchitectTilePositionGetter(Vector3.zero, null);
		public ArchitectTilePositionGetter TilePositionGetter { get { return tilePositionGetter; } }

		[Inject("ArchitectMain")]
		Camera MainCam = null;

		void Update()
		{
			UpdateTileGetter();
		}

		private void UpdateTileGetter()
		{
			ArchitectTilePositionGetter newTilePositionGetter = new ArchitectTilePositionGetter(MainCam.GetMouseWorldPosition(), SelectedLayer);
			if (newTilePositionGetter.TilePosition != tilePositionGetter.TilePosition)
				tilePositionGetter = newTilePositionGetter;
		}

	}
}