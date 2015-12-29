using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

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
		public List<LayerData> Layers = new List<LayerData>();

		void Awake()
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[0];
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
			clearAllLayer();
			addLayer();
			SelectedLayer = Layers[0];
		}

		private void clearAllLayer()
		{
			for (int i = 0; i < Layers.Count; i++)
			{
				Layers[i].LayerTransform.gameObject.Destroy();
			}
			Layers.Clear();
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
						AddSelectedTileType(SelectedLayer, worldP, tilePoint);
					}
					else if (SelectedLayer[tilePoint.X, tilePoint.Y].TileType != SelectedTileType)
					{
						removeTile(tilePoint);
						AddSelectedTileType(SelectedLayer, worldP, tilePoint);
					}

				}
			}
		}

		public void AddSelectedTileType(LayerData layer, Vector3 worldP, Point2 tilePoint)
		{
			layer.AddTile(tilePoint, SelectedTileType);
		}

		public void AddTile(LayerData layer, Vector3 worldP, Point2 tilePoint, TileType tileType)
		{
			layer.AddTile(tilePoint, tileType);
		}

		public void RemoveSelectedLayer()
		{
			if (SelectedLayer != null)
			{
				SelectedLayer.LayerTransform.gameObject.Destroy();
				Layers.Remove(SelectedLayer);
				SelectedLayer = null;
			}
		}

		public void MoveDownSelectedLayer()
		{
			int selectIndex = Layers.IndexOf(SelectedLayer);
			if (selectIndex == Layers.Count - 1) return;
			Layers.Switch(selectIndex, selectIndex + 1);
		}

		public void MoveUpSelectedLayer()
		{
			int selectIndex = Layers.IndexOf(SelectedLayer);
			if (selectIndex == 0) return;
			Layers.Switch(selectIndex, selectIndex - 1);
		}

		public void DuplicateSelectedLayer()
		{
			LayerData newLayer = SelectedLayer.Clone();

			Layers.Insert(SelectedIndex, newLayer);
		}

		void removeTile(Point2 tilePoint)
		{
			SelectedLayer[tilePoint.X, tilePoint.Y].GameObject.Destroy();
			SelectedLayer[tilePoint.X, tilePoint.Y] = TileData.Empty;
		}

		public int SelectedIndex { get { return Layers.IndexOf(SelectedLayer); } }

		public LayerData addLayer()
		{
			return addLayer(null, "Layer", 1, 1);
		}
		public LayerData addLayer(Transform parent, string name, int tileHeight, int tileWidth)
		{
			LayerData newLayer = new LayerData(parent, name, 20, 20);
			newLayer.TileHeight = tileHeight;
			newLayer.TileWidth = tileWidth;
			Layers.Add(newLayer);
			return newLayer;
		}
	}

}
