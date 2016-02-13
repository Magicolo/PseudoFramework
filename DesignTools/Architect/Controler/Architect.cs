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
		public void CreateNewMap(Transform parent, string mapName, int width, int height)
		{
			MapData = new MapData(parent, mapName, width, height);
		}

		public LayerData AddLayerData(string layerName)
		{
			return MapData.AddLayer(layerName);
		}

	}
}
