using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	[Serializable]
	public class DrawingControler
	{
		ArchitectControler Architect;
		ArchitectToolControler ToolControler;
		ArchitectLayerControler LayerControler;

		public GridScallerTiller Grid;
		public SpriteRenderer PreviewSprite;

		[Inject("ArchitectUI")]
		Camera UICam;

		public RectTransform DrawingRect;
		public bool IsMouseInDrawingRegion { get { return RectTransformUtility.RectangleContainsScreenPoint(DrawingRect, UnityEngine.Input.mousePosition, UICam); } }

		TileType SelectedTileType { get { return ToolControler.SelectedTileType; } }
		ArchitectTilePositionGetter TilePositionGetter { get { return LayerControler.TilePositionGetter; } }

		


		void Update() {

			UpdatePreviewSprite();
			ResetGridSize();
			if (IsMouseInDrawingRegion) 
			{
				if (UnityEngine.Input.GetMouseButton(0))
					ToolControler.HandleLeftMouse();
				else if (UnityEngine.Input.GetMouseButton(1))
					ToolControler.HandlePipette();
			}
		}
		
		public void ResetGridSize()
		{
			Grid.NbTilesX = LayerControler.SelectedLayer.LayerWidth;
			Grid.NbTilesY = LayerControler.SelectedLayer.LayerHeight;
			Grid.TileWidth = LayerControler.SelectedLayer.TileWidth;
			Grid.TileHeight = LayerControler.SelectedLayer.TileHeight;
		}

		private void UpdatePreviewSprite()
		{
			PreviewSprite.transform.Reset();
			PreviewSprite.enabled = true;
			/*PreviewSprite.enabled = SelectedLayer.IsActive;
			if (!SelectedLayer.IsActive) return;
			*/
			if (TilePositionGetter.Valid)
			{
				PreviewSprite.enabled = true;
				if (SelectedTileType == null)
					PreviewSprite.sprite = null;
				else
					PreviewSprite.sprite = SelectedTileType.PreviewSprite;
				PreviewSprite.transform.Translate(TilePositionGetter.TileWorldPosition);
				ToolControler.RotationFlip.ApplyTo(PreviewSprite.transform);
			}
			else
			{
				PreviewSprite.enabled = false;
			}
		}
	}
}
