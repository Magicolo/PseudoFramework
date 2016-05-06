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
		public Transform ParentTransform;
		public String Name;
		public List<LayerData> Layers = new List<LayerData>();

		public int Width;
		public int Height;

		public MapData(Transform parent, string mapName, int width, int height)
		{
			this.ParentTransform = parent;
			this.Name = mapName;
			this.Width = width;
			this.Height = height;
		}

		public LayerData AddLayer(string name)
		{
			return AddLayer(ParentTransform, name);
		}

		public LayerData AddLayer(Transform parent, string name)
		{
			LayerData newLayer = new LayerData(parent, name, Width, Height);
			Layers.Add(newLayer);
			return newLayer;
		}

		public void DestroyAndRemoveAllLayers()
		{
			while (Layers != null && Layers.Count != 0)
				DestroyAndRemoveLayer(Layers[Layers.Count-1]);
			if(ParentTransform != null)
				ParentTransform.gameObject.Destroy();
		}

		public void DestroyAndRemoveLayer(LayerData removeMe)
		{
			if(removeMe != null)
				removeMe.DestroyAllAndClear();
			Layers.Remove(removeMe);
		}

	}
}
