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
        private ArchitectBehavior architectBehavior;

		int ActiveLayerIndex = -1;
		LayerData ActiveLayer { get { return architect.MapData.Layers[ActiveLayerIndex]; } }

		public GameObject LayerLinePrefab;

		public List<LayerLineGUI> layerButtons = new List<LayerLineGUI>();

		public Transform ItemPanel;

		public Button AddLayerButton;
		public Button RemoveLayerButton;
		public Button MoveUpLayerButton;
		public Button MoveDownLayerButton;
		public Button DuplicateLayerButton;

		private UISkin skin { get { return architectBehavior.Skin; } }

		private List<LayerData> Layers { get { return architect.MapData.Layers; } }

		void Awake()
		{
			architectBehavior = GetComponentInParent<ArchitectBehavior>();
			architect = architectBehavior.Architect;
		}

		void Start()
		{
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

			if (architect.MapData != null)
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
				layerButtons[ActiveLayerIndex].GetComponent<Image>().color = skin.SelectedButtonBackground;

			adjustButtons();
		}

		public void AddLayer()
		{
			LayerData newLayer = architect.AddLayerData("New Layer");
			refreshUILayerLines();
			switchLayer(newLayer);
			adjustButtons();
		}

		public void RemoveSelectedLayer()
		{
			if (ActiveLayerIndex == -1) return;

			architect.RemoveLayerData(ActiveLayer);
			ActiveLayerIndex-=1;
			RefreshUI();
		}

		private void adjustButtons()
		{
			if (architect.MapData == null)
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

		public void MoveUpSelectedLayer()
		{
			/*architect.MoveUpSelectedLayer();
			switchLayerSelection(selectedIndex - 1);
			RefreshLayers();*/

		}

		public void MoveDownSelectedLayer()
		{
			/*architect.MoveDownSelectedLayer();
			switchLayerSelection(selectedIndex + 1);
			RefreshLayers();*/
		}

		public void DuplicateSelectedLayer()
		{
			/*architect.DuplicateSelectedLayer();
			RefreshLayers();*/

		}


	}
}
