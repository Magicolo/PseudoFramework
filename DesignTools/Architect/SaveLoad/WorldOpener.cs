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


		Architect architect;

		private WorldOpener(Architect architect)
		{
			this.architect = architect;
		}

		private void Load(string[] fileContent)
		{
			this.fileContent = fileContent;
			if (currentLine.StartsWith("Layer:"))
			{
				readLayer();
			}
		}

		private void readLayer()
		{
			string name = currentLine.Substring(6);
			architect.SelectedLayer = new LayerData(null, name, 20, 20);

			int nbLines = 20;
			int lineWidth = 20;
			for (int y = 0; y < nbLines; y++)
			{
				nextLine();
				readLayerLine(nbLines - y, lineWidth);
			}
		}

		private void readLayerLine(int y, int lineWidth)
		{
			for (int x = 0; x < lineWidth; x++)
			{
				int id = readNextInt();
				Point2 position = new Point2(x, y);
				TileType tileType = null;
				if (id == 0)
					continue;
				tileType = architect.Linker.Tilesets[0][id - 1];
				architect.SelectedLayer.AddTile(position, tileType);
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

		public static void OpenFile(Architect architect, string fileName)
		{
			WorldOpener wo = new WorldOpener(architect);
			string[] fileContent = System.IO.File.ReadAllLines(fileName);
			wo.Load(fileContent);
		}


	}

}
