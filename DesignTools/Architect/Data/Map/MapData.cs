using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class MapData
	{
		public Transform parent;
		public String Name;
		public List<LayerData> Layers = new List<LayerData>();

		public int Width;
		public int Height;

		public MapData(Transform parent, string mapName, int width, int height)
		{
			this.Name = mapName;
			this.Width = width;
			this.Height = height;
		}

		public LayerData AddLayer(string name)
		{
			return AddLayer(parent, name);
		}

		public LayerData AddLayer(Transform parent, string name)
		{
			LayerData newLayer = new LayerData(parent, name, Width, Height);
			Layers.Add(newLayer);
			return newLayer;
		}
	}
}
