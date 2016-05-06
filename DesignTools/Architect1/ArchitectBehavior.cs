using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectBehavior : MonoBehaviour
	{

		public ArchitectControler Architect;
		public UISkin Skin;
		public UIFactory UIFactory;
		public ArchitectLinker Linker;
		public Camera MapCam;
		public Camera UICam;

		//ArchitectMenus menus;
		LayerPanel layerPanel;
		

		public ArchitectBehavior()
		{
			Architect = new ArchitectControler();
		}

		void Awake()
		{
			Architect.MapData = null;
			Architect.MapCam = MapCam;
			Architect.UICam = UICam;
			//menus = GetComponentInChildren<ArchitectMenus>();
			layerPanel = GetComponentInChildren<LayerPanel>();
		}

		public void CreateNewMap(string text, int width, int height)
		{
			Architect.CreateNewMap(text, width, height);
			layerPanel.RefreshUI();
		}
	}
}
