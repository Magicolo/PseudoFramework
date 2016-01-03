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


		public RectTransform drawingRect;
		public bool IsMouseInDrawingRegion { get { return RectTransformUtility.RectangleContainsScreenPoint(drawingRect, Input.mousePosition, UICam); } }


		public Camera Cam;
		public Camera UICam;

		public LayerData SelectedLayer;
		public List<LayerData> Layers = new List<LayerData>();

		ArchitectHistory architectHistory = new ArchitectHistory();

		InputCombinaisonChecker undoInput = new InputCombinaisonChecker(true, KeyCode.Z);

		public ArchitectMenus menu;
		public UISkin UISkin;

		ArchitectTilePositionGetter tilePositionGetter = new ArchitectTilePositionGetter(Vector3.zero, null);

		public bool HasHistory { get { return architectHistory.History.Count > 0; } }
		public bool HasRedoHistory { get { return architectHistory.HistoryRedo.Count > 0; } }

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
			clearAllLayer();
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
			UpdateTileGetter();

			if (Input.GetMouseButton(0))
				HandleLeftMouse();
			else if (Input.GetMouseButton(1))
				HandleErase();

			undoInput.Update();
			if (undoInput.GetKeyCombinaison())
				Undo();
			menu.Refresh();
		}

		private void UpdateTileGetter()
		{
			ArchitectTilePositionGetter newTilePositionGetter = new ArchitectTilePositionGetter(Cam.GetMouseWorldPosition(), SelectedLayer);
			if (newTilePositionGetter.TilePosition != tilePositionGetter.TilePosition)
			{
				tilePositionGetter = newTilePositionGetter;
			}
		}

		private void HandleLeftMouse()
		{
			if (IsMouseInDrawingRegion && SelectedLayer.IsInArrayBound(tilePositionGetter.TilePosition))
				architectHistory.Do(new BrushCommand(this, tilePositionGetter));

		}

		private void HandleErase()
		{
			if (IsMouseInDrawingRegion && SelectedLayer.IsInArrayBound(tilePositionGetter.TilePosition))
				architectHistory.Do(new EraserTool(this, tilePositionGetter));
		}




		public void Undo()
		{
			architectHistory.Undo();
			menu.Refresh();
		}

		public void Redo()
		{
			architectHistory.Redo();
			menu.Refresh();
		}

		public void setSelectedTile(int id)
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[id - 1];
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
			SelectedLayer.LayerTransform.SetSiblingIndex(SelectedLayer.LayerTransform.GetSiblingIndex() + 1);
		}

		public void MoveUpSelectedLayer()
		{
			int selectIndex = Layers.IndexOf(SelectedLayer);
			if (selectIndex == 0) return;
			Layers.Switch(selectIndex, selectIndex - 1);
			SelectedLayer.LayerTransform.SetSiblingIndex(SelectedLayer.LayerTransform.GetSiblingIndex() - 1);
		}

		public void DuplicateSelectedLayer()
		{
			LayerData newLayer = SelectedLayer.Clone();

			Layers.Insert(SelectedIndex, newLayer);
		}

		public void RemoveTile(Point2 tilePoint)
		{
			SelectedLayer.EmptyTile(tilePoint);
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
