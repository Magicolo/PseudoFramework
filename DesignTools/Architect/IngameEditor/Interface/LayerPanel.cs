using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace Pseudo
{
	[System.Serializable]
	public class LayerPanel : MonoBehaviour
	{
		private Architect architect;

		public GameObject LayerLinePrefab;

		public List<LayerLineGUI> layerButtons = new List<LayerLineGUI>();

		public Color SelectedColor;
		public Color BaseColor;

		public Transform ItemPanel;

		public Button AddLayerButton;
		public Button RemoveLayerButton;
		public Button MoveUpLayerButton;
		public Button MoveDownLayerButton;
		public Button DuplicateLayerButton;

		int selectedIndex = -1;

		float panelWidth;

		private UISkin skin { get { return architect.UISkin; } }

		void Awake()
		{
			architect = GetComponentInParent<Architect>();
		}

		void Start()
		{
			refreshLayers();
		}

		public void refreshLayers()
		{
			panelWidth = ItemPanel.GetComponent<RectTransform>().rect.width * 0.97f - 10;
			for (int i = 0; i < layerButtons.Count; i++)
			{
				layerButtons[i].gameObject.Destroy();
			}
			layerButtons.Clear();


			for (int i = 0; i < architect.Layers.Count; i++)
			{
				LayerData layer = architect.Layers[i];
				crateLayerButtonItem(layer, i);
			}

			switchLayerSelection(selectedIndex);
			adjustButtons();
		}

		private void crateLayerButtonItem(LayerData layer, int i)
		{
			Vector2 dimension = new Vector2(panelWidth, 20);
			Vector3 position = new Vector3(dimension.x / 2 + 5, -12 - i * 20);

			GameObject layerLine = GameObject.Instantiate(LayerLinePrefab);
			LayerLineGUI layerLineGui = layerLine.GetComponent<LayerLineGUI>();
			layerLineGui.Init(layer, ItemPanel, position, dimension, () => switchLayer(layer));
			layerLineGui.GetComponent<RectTransform>().localScale = Vector3.one;

			layerButtons.Add(layerLineGui);
		}

		void showHide(LayerData layer)
		{

		}

		void switchLayer(LayerData layer)
		{
			switchLayerSelection(architect.Layers.IndexOf(layer));
		}

		void switchLayerSelection(int index)
		{
			if (selectedIndex != -1)
				layerButtons[selectedIndex].SetSelected(false);

			if (architect.Layers.Count > 0)
			{
				selectedIndex = (index == -1) ? 0 : index;
				architect.SelectedLayer = architect.Layers[selectedIndex];
				layerButtons[selectedIndex].GetComponent<Image>().color = SelectedColor;
			}
			adjustButtons();
		}

		public void AddLayer()
		{
			LayerData newLayer = architect.addLayer();
			refreshLayers();
			switchLayer(newLayer);
		}

		public void RemoveSelectedLayer()
		{
			int layerIndex = architect.Layers.IndexOf(architect.SelectedLayer);
			selectedIndex = -1;
			architect.RemoveSelectedLayer();
			refreshLayers();
			switchLayerSelection(layerIndex - 1);
		}

		private void adjustButtons()
		{
			if (architect.Layers.Count == 0)
			{
				skin.Disable(MoveDownLayerButton, MoveUpLayerButton, RemoveLayerButton, DuplicateLayerButton);
				skin.Enable(AddLayerButton);
			}
			else if (architect.Layers.Count == 1)
			{
				skin.Disable(MoveDownLayerButton, MoveUpLayerButton);
				skin.Enable(RemoveLayerButton, DuplicateLayerButton, AddLayerButton);

			}
			else
			{
				if (architect.Layers.Count == 6)
				{
					skin.Disable(AddLayerButton);
				}
				skin.Enable(RemoveLayerButton, DuplicateLayerButton);
				skin.Enable(MoveUpLayerButton, MoveDownLayerButton);
				if (selectedIndex == 0)
					skin.Disable(MoveUpLayerButton);
				else if (selectedIndex == architect.Layers.Count - 1)
					skin.Disable(MoveDownLayerButton);
			}
		}

		public void MoveUpSelectedLayer()
		{
			architect.MoveUpSelectedLayer();
			switchLayerSelection(selectedIndex - 1);
			refreshLayers();

		}

		public void MoveDownSelectedLayer()
		{
			architect.MoveDownSelectedLayer();
			switchLayerSelection(selectedIndex + 1);
			refreshLayers();
		}

		public void DuplicateSelectedLayer()
		{
			architect.DuplicateSelectedLayer();
			refreshLayers();

		}


	}
}
