using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo.Architect
{

	public delegate void MapDataChanged(MapData MapData);

	[Serializable]
	public class ArchitectControler
	{
		[Inject()]
		ArchitectLinker Linker;

		public MapData MapData;
		
		public event MapDataChanged OnMapDataChanged;

		public bool MapLoaded { get; private set; }

		public ArchitectHistory ArchitectHistory = new ArchitectHistory();
		public bool HasHistory { get { return ArchitectHistory.History.Count > 0; } }
		public bool HasRedoHistory { get { return ArchitectHistory.HistoryRedo.Count > 0; } }


		void Awake() {
			MapData = null;
		}

		public void CreateNewMap(string text, int width, int height)
		{
			GameObject newRoot = new GameObject("MapRoot");
			CreateNewMap(newRoot.transform, text, width, height);
		}

		public void CreateNewMap(Transform parent, string mapName, int width, int height)
		{
			Debug.Log("Createmap " + mapName);
			MapLoaded = true;
			if (MapData != null)
				MapData.DestroyAndRemoveAllLayers();
			MapData = new MapData(parent, mapName, width, height);
			if (OnMapDataChanged != null)
				OnMapDataChanged(MapData);
		}

		public LayerData AddLayerData(string layerName)
		{
			LayerData newLayerData = MapData.AddLayer(layerName);
			OnMapDataChanged(MapData);
			return newLayerData;
		}

		public void RemoveLayerData(LayerData removeMe)
		{
			MapData.DestroyAndRemoveLayer(removeMe);
			OnMapDataChanged(MapData);
		}

		
	}
}
