using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class WorldOpener
	{
		string[] fileContent;
		int currentLineIndex = 0;
		int currentColIndex = 0;
		string currentLine { get { return fileContent[currentLineIndex]; } }
		bool isEndOfFile { get { return currentLineIndex >= fileContent.Length; } }

		ArchitectLinker linker;
		Transform mapParent;

		private WorldOpener(ArchitectLinker linker, Transform mapParent)
		{
			this.linker = linker;
			this.mapParent = mapParent;
		}

		private List<LayerData> Load(string[] fileContent)
		{
			this.fileContent = fileContent;
			List<LayerData> layers = new List<LayerData>();

			while (!isEndOfFile)
			{
				if (currentLine.StartsWith("Layer:"))
					layers.Add(readLayer());
				nextLine();
			}
			return layers;
		}

		private LayerData readLayer()
		{
			string name = currentLine.Substring(6);
			nextLine();
			currentColIndex = indexOfNext(':') + 1;


			int mapWidth = readNextInt();
			int mapHeight = readNextInt();
			LayerData layer = new LayerData(mapParent, name, mapWidth, mapHeight);

			for (int y = 0; y < mapHeight; y++)
			{
				nextLine();
				readLayerLine(layer, mapHeight - y - 1, mapWidth);
			}
			return layer;
		}

		private void readLayerLine(LayerData layer, int y, int lineWidth)
		{
			for (int x = 0; x < lineWidth; x++)
			{
				int value = readNextInt();
				int id = ArchitectRotationHandler.RemoveRotationFlags(value);
				int rotationFlags = ArchitectRotationHandler.GetRotationFlags(value);
				Point2 position = new Point2(x, y);
				TileType tileType = null;
				if (id == 0)
					continue;
				tileType = linker.Tilesets[0][id - 1];
				layer.AddTile(position, tileType, rotationFlags);

			}
		}

		private int readNextInt()
		{
			int nextCommas = indexOfNext(',');
			int lenght = nextCommas - currentColIndex;

			string intString = currentLine.Substring(currentColIndex, lenght);
			currentColIndex += lenght + 1;

			return Int32.Parse(intString);
		}

		private int indexOfNext(char character)
		{
			return currentLine.IndexOf(character, currentColIndex);
		}

		private void nextLine()
		{
			currentLineIndex++;
			currentColIndex = 0;
		}

		public static List<LayerData> OpenFile(ArchitectLinker linker, string fileName, Transform mapParent)
		{
			WorldOpener wo = new WorldOpener(linker, mapParent);
			string[] fileContent = System.IO.File.ReadAllLines(fileName);
			return wo.Load(fileContent);
		}


	}

}
