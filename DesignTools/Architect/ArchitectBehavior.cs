using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class ArchitectBehavior : MonoBehaviour
	{

		public Architect Architect;
		public UISkin Skin;
		public UIFactory UIFactory;

		//ArchitectMenus menus;
		LayerPanel layerPanel;
		TilesetItemsPanel tilesetPanel;


		public TileType ActiveTileType;

		public ArchitectBehavior()
		{
			Architect = new Architect();
		}

		void Awake()
		{
			Architect.MapData = null;
			//menus = GetComponentInChildren<ArchitectMenus>();
			layerPanel = GetComponentInChildren<LayerPanel>();
		}

		public void CreateNewMap(string text, int width, int height)
		{
			Architect.CreateNewMap(text, width, height);
			layerPanel.RefreshUI();
			tilesetPanel.Refresh();
		}
	}
}
