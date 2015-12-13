using UnityEngine;
using System.Collections.Generic;
using System;


namespace Pseudo
{
	public class Architect : MonoBehaviour
	{

		public ArchitectLinker Linker;
		public UIFactory UiFactory;

		public TileType SelectedTileType;

		[Range(0, 1)]
		public float DrawingWidth = 0.8f;
		[Range(0, 1)]
		public float DrawingHeight = 0.9f;
		private Rect drawingRect;

		public Camera Cam;

		public LayerData SelectedLayer;

		void Start()
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[0];
			SelectedLayer.TileHeight = 1;
			SelectedLayer.TileWidth = 1;
			New();
		}

		public void Save()
		{
			SaveWorld.SaveAll(this, "map.txt");
		}

		public void Open(string path)
		{
			New();
			WorldOpener.OpenFile(this, path);
		}

		public void New()
		{
			if (SelectedLayer != null)
				SelectedLayer.DestroyAllAndClear();
			SelectedLayer = new LayerData(null, "GroundLayer", 20, 20);
		}

		void Update()
		{
			drawingRect = new Rect(0, 0, Screen.width * DrawingWidth, Screen.height * DrawingHeight);
			if (Input.GetMouseButton(0))
			{
				handleUseClick();

			}
			else if (Input.GetMouseButton(1))
			{
				HandleErase();
			}
		}

		public void setSelectedTile(int id)
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[id - 1];
		}

		private void HandleErase()
		{
			if (drawingRect.Contains(Input.mousePosition))
			{
				Vector3 p = Cam.ScreenToWorldPoint(Input.mousePosition);
				Vector3 TileP = p.Div(new Vector3(SelectedLayer.TileWidth, SelectedLayer.TileHeight, 1)).Round().SetValues(0, Axes.Z);
				// Vector3 worldP = TileP.Mult(new Vector3(Layer.TileWidth, Layer.TileHeight, 1));
				Point2 tilePoint = new Point2(TileP.ToVector2());


				if (SelectedLayer.IsInArrayBound(tilePoint))
				{
					if (SelectedLayer[tilePoint.X, tilePoint.Y] != null)
					{
						removeTile(tilePoint);
					}
				}
			}
		}

		void handleUseClick()
		{

			if (drawingRect.Contains(Input.mousePosition))
			{
				Vector3 p = Cam.ScreenToWorldPoint(Input.mousePosition);
				Vector3 TileP = p.Div(new Vector3(SelectedLayer.TileWidth, SelectedLayer.TileHeight, 1)).Round().SetValues(0, Axes.Z);
				Vector3 worldP = TileP.Mult(new Vector3(SelectedLayer.TileWidth, SelectedLayer.TileHeight, 1));
				Point2 tilePoint = new Point2(TileP.ToVector2());


				if (SelectedLayer.IsInArrayBound(tilePoint))
				{
					if (SelectedLayer[tilePoint.X, tilePoint.Y] == null)
					{
						addSelectedTileType(SelectedLayer, worldP, tilePoint);
					}
					else if (SelectedLayer[tilePoint.X, tilePoint.Y].TileType != SelectedTileType)
					{
						removeTile(tilePoint);
						addSelectedTileType(SelectedLayer, worldP, tilePoint);
					}

				}
			}
		}

		public void addSelectedTileType(LayerData layer, Vector3 worldP, Point2 tilePoint)
		{
			addTile(layer, worldP, tilePoint, SelectedTileType);
		}

		public void addTile(LayerData layer, Vector3 worldP, Point2 tilePoint, TileType tileType)
		{
			if (tileType == null) return;
			GameObject newTile = GameObject.Instantiate(tileType.Prefab);
			newTile.transform.SetPosition(worldP);
			newTile.transform.parent = layer.layerTransform;

			TileData tileData = new TileData(tileType, newTile);
			layer[tilePoint.X, tilePoint.Y] = tileData;
		}

		void removeTile(Point2 tilePoint)
		{
			SelectedLayer[tilePoint.X, tilePoint.Y].GameObject.Destroy();
			SelectedLayer[tilePoint.X, tilePoint.Y] = TileData.Empty;
		}
	}

}
