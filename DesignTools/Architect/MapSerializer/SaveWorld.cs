using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class SaveWorld
	{
		Architect architect;

		string fileContent = "";


		void Save(Architect architect, string filename)
		{
			this.architect = architect;
			addHeader();
			addMapData();

#if !UNITY_WEBGL && !UNITY_WEBPLAYER
			System.IO.File.WriteAllText(filename, fileContent);
#endif
		}

		private void addHeader()
		{

		}

		private void addMapData()
		{
			for (int i = 0; i < architect.Layers.Count; i++)
			{
				addLayer(architect.Layers[i]);
			}
		}

		private void addLayer(LayerData layer)
		{
			fileContent += "Layer:" + layer.LayerTransform.name + "\n";
			fileContent += "Dimension:" + layer.LayerWidth + "," + layer.LayerHeight + ",\n";
			for (int y = layer.LayerHeight - 1; y >= 0; y--)
			{
				for (int x = 0; x < layer.LayerWidth; x++)
				{
					TileType tileType = layer[x, y].TileType;
					if (!tileType.IsNullOrIdZero())
					{
						int rotationFlag = (int)ArchitectRotationHandler.getRotationFlipFlags(layer[x, y].Transform);
						int id = rotationFlag + layer[x, y].TileType.Id;
						fileContent += id + ",";
					}
					else
						fileContent += 0 + ",";
				}
				fileContent += "\n";
			}
		}

		public static void SaveAll(Architect architect, string filename)
		{
			SaveWorld save = new SaveWorld();
			save.Save(architect, filename);

		}
	}

}
