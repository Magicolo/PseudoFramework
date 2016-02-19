using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class Architect
	{
		public MapData MapData;


		public void CreateNewMap(string text, int width, int height)
		{
			GameObject newRoot = new GameObject("MapRoot");
			CreateNewMap(newRoot.transform, text, width, height);
		}

		public void CreateNewMap(Transform parent, string mapName, int width, int height)
		{
			if(MapData != null)
				MapData.DestroyAndRemoveAllLayers();
			MapData = new MapData(parent, mapName, width, height);
		}

		public LayerData AddLayerData(string layerName)
		{
			return MapData.AddLayer(layerName);
		}

		public void RemoveLayerData(LayerData removeMe)
		{
			MapData.DestroyAndRemoveLayer(removeMe);
		}

		
	}
}
