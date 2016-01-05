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


		public SpriteRenderer PreviewSprite;

		public RectTransform drawingRect;
		public bool IsMouseInDrawingRegion { get { return RectTransformUtility.RectangleContainsScreenPoint(drawingRect, Input.mousePosition, UICam); } }


		public Camera Cam;
		public Camera UICam;

		public LayerData SelectedLayer;
		public List<LayerData> Layers = new List<LayerData>();

		ArchitectHistory architectHistory = new ArchitectHistory();

		InputCombinaisonChecker undoInput = new InputCombinaisonChecker(true, KeyCode.Z, KeyCode.LeftControl);

		public UISkin UISkin;

		ArchitectTilePositionGetter tilePositionGetter = new ArchitectTilePositionGetter(Vector3.zero, null);

		public bool HasHistory { get { return architectHistory.History.Count > 0; } }
		public bool HasRedoHistory { get { return architectHistory.HistoryRedo.Count > 0; } }

		public GridScallerTiller Grid;

		public bool NextTileFlipX;
		public bool NextTileFlipY;
		public float NextTileRotation;

		[Space()]
		ToolFactory.ToolType selectedToolType;
		public ToolFactory.ToolType SelectedToolType
		{
			get { return selectedToolType; }
			set
			{
				selectedToolType = value;
				Toolbar.Refresh();
			}
		}

		TileType selectedTileType;
		public TileType SelectedTileType
		{
			get { return selectedTileType; }
			set
			{
				selectedTileType = value;
				updatePreviewSprite();

				TilesetPanel.Refresh();
			}
		}

		[Space()]
		public ArchitectMenus Menu;
		public ToolbarPanel Toolbar;
		public TilesetItemsPanel TilesetPanel;
		public LayerPanel LayerPanel;

		void Awake()
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[0];
		}
		void Start()
		{
			SelectedToolType = ToolFactory.ToolType.Brush;
			New();
		}

		private void updatePreviewSprite()
		{
			PreviewSprite.transform.Reset();
			if (SelectedTileType == null)
				PreviewSprite.sprite = null;
			else
				PreviewSprite.sprite = SelectedTileType.PreviewSprite;
			PreviewSprite.transform.Translate(tilePositionGetter.TileWorldPosition);

			ArchitectRotationHandler.ApplyRotationFlip(PreviewSprite.transform, NextTileRotation, NextTileFlipX, NextTileFlipY);
		}

		public void Save()
		{
			SaveWorld.SaveAll(this, "Assets\\Tests\\DesignTools\\Architect\\map.arc");
		}

		public void ResetGridSize()
		{
			Grid.NbTilesX = SelectedLayer.LayerWidth;
			Grid.NbTilesY = SelectedLayer.LayerHeight;
			Grid.TileWidth = SelectedLayer.TileWidth;
			Grid.TileHeight = SelectedLayer.TileHeight;
		}

		public void Open(string path)
		{
			clearAllLayer();
			var layers = WorldOpener.OpenFile(Linker, path);
			Layers.AddRange(layers);
			SelectedLayer = layers[0];
			LayerPanel.RefreshLayers();
		}

		public void New()
		{
			clearAllLayer();
			addLayer();
			SelectedLayer = Layers[0];
			ResetGridSize();
			LayerPanel.RefreshLayers();
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
				HandlePipette();

			undoInput.Update();
			if (undoInput.GetKeyCombinaison())
				Undo();

			handleKeyboardShortcut();
			Menu.Refresh();
		}

		private void handleKeyboardShortcut()
		{
			if (Input.GetKeyDown(KeyCode.E))
				SelectedToolType = ToolFactory.ToolType.Eraser;
			else if (Input.GetKeyDown(KeyCode.B))
				SelectedToolType = ToolFactory.ToolType.Brush;
			else if (Input.GetKeyDown(KeyCode.R))
				Rotate();
			else if (Input.GetKeyDown(KeyCode.X))
				FlipX();
			else if (Input.GetKeyDown(KeyCode.Y))
				FlipY();
		}

		private void FlipX()
		{
			NextTileFlipX = !NextTileFlipX;
			updatePreviewSprite();
		}

		private void FlipY()
		{
			NextTileFlipY = !NextTileFlipY;
			updatePreviewSprite();
		}

		private void Rotate()
		{
			NextTileRotation -= 90;
			NextTileRotation %= 360;
			updatePreviewSprite();
		}

		private void UpdateTileGetter()
		{
			ArchitectTilePositionGetter newTilePositionGetter = new ArchitectTilePositionGetter(Cam.GetMouseWorldPosition(), SelectedLayer);
			if (newTilePositionGetter.TilePosition != tilePositionGetter.TilePosition)
			{
				tilePositionGetter = newTilePositionGetter;
				PreviewSprite.transform.position = tilePositionGetter.TileWorldPosition;
			}
		}

		private void HandleLeftMouse()
		{
			if (IsMouseInDrawingRegion && SelectedLayer.IsInArrayBound(tilePositionGetter.TilePosition))
				architectHistory.Do(ToolFactory.Create(selectedToolType, this, tilePositionGetter));

		}

		private void HandlePipette()
		{
			if (IsMouseInDrawingRegion && SelectedLayer.IsInArrayBound(tilePositionGetter.TilePosition))
				SelectedTileType = SelectedLayer[tilePositionGetter.TilePosition].TileType;
		}




		public void Undo()
		{
			architectHistory.Undo();
			Menu.Refresh();
		}

		public void Redo()
		{
			architectHistory.Redo();
			Menu.Refresh();
		}

		public void setSelectedTile(int id)
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[id - 1];
		}

		public void AddSelectedTileType(LayerData layer, Vector3 worldP, Point2 tilePoint)
		{
			layer.AddTile(tilePoint, SelectedTileType, NextTileRotation, NextTileFlipX, NextTileFlipY);
		}

		public void AddTile(LayerData layer, Vector3 worldP, Point2 tilePoint, TileType tileType)
		{
			layer.AddTile(tilePoint, tileType, NextTileRotation, NextTileFlipX, NextTileFlipY);
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
