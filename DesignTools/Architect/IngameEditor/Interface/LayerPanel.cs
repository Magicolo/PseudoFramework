using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	[System.Serializable]
	public class LayerPanel : MonoBehaviour
	{
		[Inject()]
		ArchitectControler Architect;
		[Inject()]
		ArchitectBehavior ArchitectBehavior;
		[Inject()]
		ArchitectLayerControler LayerControler;

		int ActiveLayerIndex = -1;
		LayerData ActiveLayer { get { return LayerControler.SelectedLayer; } }

		public GameObject LayerLinePrefab;

		public List<LayerLineGUI> layerButtons = new List<LayerLineGUI>();

		public Transform ItemPanel;

		public Button AddLayerButton;
		public Button RemoveLayerButton;
		public Button MoveUpLayerButton;
		public Button MoveDownLayerButton;
		public Button DuplicateLayerButton;

		private UISkin skin { get { return ArchitectBehavior.Skin; } }

		private List<LayerData> Layers { get { return Architect.MapData.Layers; } }

		void Start()
		{
			Architect.OnMapDataChanged += HandleMapDataChanged;
			RefreshUI();
		}

		private void HandleMapDataChanged(MapData mapData) {
			if (mapData.Layers.Count == 0)
				ActiveLayerIndex = -1;
			else if (ActiveLayerIndex == -1 && mapData.Layers.Count > 0)
				ActiveLayerIndex = 0;

			RefreshUI();
		}

		public void RefreshUI()
		{
			refreshUILayerLines();
			switchLayerSelection(ActiveLayerIndex);
			adjustButtons();
		}

		private void refreshUILayerLines()
		{
			for (int i = 0; i < layerButtons.Count; i++)
			{
				layerButtons[i].gameObject.Destroy();
			}
			layerButtons.Clear();

			if (Architect.MapData != null)
			{
				for (int i = 0; i < Layers.Count; i++)
				{
					LayerData layer = Layers[i];
					crateLayerButtonItem(layer, i);
				}
			}
		}

		private void crateLayerButtonItem(LayerData layer, int i)
		{
			GameObject layerLine = GameObject.Instantiate(LayerLinePrefab);
			LayerLineGUI layerLineGui = layerLine.GetComponent<LayerLineGUI>();
			layerLineGui.Init(layer, ItemPanel, () => switchLayer(layer));
			
			layerButtons.Add(layerLineGui);
		}

		void showHide(LayerData layer)
		{

		}

		void switchLayer(LayerData layer)
		{
			switchLayerSelection(Layers.IndexOf(layer));
		}

		void switchLayerSelection(int index)
		{
			
			if (ActiveLayerIndex != -1)
				layerButtons[ActiveLayerIndex].SetSelected(false);

			ActiveLayerIndex = index;

			if (ActiveLayerIndex != -1 && ActiveLayerIndex < Layers.Count) 
			{
				layerButtons[ActiveLayerIndex].GetComponent<Image>().color = skin.SelectedButtonBackground;
				LayerControler.SelectedLayer = Layers[index];
			}
				

			adjustButtons();
		}

		private void adjustButtons()
		{
			if (Architect.MapData == null)
			{
				skin.Disable(MoveDownLayerButton, MoveUpLayerButton, RemoveLayerButton, DuplicateLayerButton, AddLayerButton);
			}
			else if (Layers.Count == 0)
			{
				skin.Disable(MoveDownLayerButton, MoveUpLayerButton, RemoveLayerButton, DuplicateLayerButton);
				skin.Enable(AddLayerButton);
			}
			else if (Layers.Count == 1)
			{
				skin.Disable(MoveDownLayerButton, MoveUpLayerButton);
				skin.Enable(RemoveLayerButton, DuplicateLayerButton, AddLayerButton);

			}
			else
			{
				if (Layers.Count == 6)
				{
					skin.Disable(AddLayerButton);
				}
				skin.Enable(RemoveLayerButton, DuplicateLayerButton);
				skin.Enable(MoveUpLayerButton, MoveDownLayerButton);
				if (ActiveLayerIndex == 0)
					skin.Disable(MoveUpLayerButton);
				else if (ActiveLayerIndex == Layers.Count - 1)
					skin.Disable(MoveDownLayerButton);
			}
		}


// Button Handles
		public void AddLayer()
		{
			LayerData newLayer = Architect.AddLayerData("New Layer");
			switchLayer(newLayer);
		}

		public void RemoveSelectedLayer()
		{
			if (ActiveLayerIndex == -1) return;

			int newSelectIndex = ActiveLayerIndex - 1 ;
			ActiveLayerIndex -= 1;
			Architect.RemoveLayerData(ActiveLayer);
			switchLayerSelection(newSelectIndex);
		}

		public void MoveUpSelectedLayer()
		{
			MoveUpLayer(ActiveLayerIndex);
			switchLayerSelection(ActiveLayerIndex - 1);
			//RefreshLayers();
			RefreshUI();
		}

		public void MoveUpLayer(int index)
		{
			if (index == 0) return;
			Layers.Switch(index, index - 1);
			ActiveLayer.LayerTransform.SetSiblingIndex(ActiveLayer.LayerTransform.GetSiblingIndex() - 1);
		}


		public void MoveDownSelectedLayer()
		{
			MoveDownLayer(ActiveLayerIndex);
			switchLayerSelection(ActiveLayerIndex + 1);
			RefreshUI();
		}

		public void MoveDownLayer(int index)
		{
			if (index == Layers.Count - 1) return;
			Layers.Switch(index, index + 1);
			ActiveLayer.LayerTransform.SetSiblingIndex(ActiveLayer.LayerTransform.GetSiblingIndex() + 1);
			RefreshUI();
		}

		public void DuplicateSelectedLayer()
		{
			/*architect.DuplicateSelectedLayer();
			RefreshLayers();*/

		}


	}
}
