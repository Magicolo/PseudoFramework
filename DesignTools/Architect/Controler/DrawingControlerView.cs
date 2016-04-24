using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class DrawingControlerView : MonoBehaviour
	{

		ArchitectToolControler toolControler;
		Architect architect;
		ArchitectLinker Linker { get { return architect.Linker; } }

		void Awake()
		{
			toolControler = GetComponent<ArchitectToolControler>();
			architect = GetComponent<ArchitectBehavior>().Architect;
		}

		public TileType SelectedTileType
		{
			get { return toolControler.SelectedTileType; }
			set
			{
				toolControler.SelectedTileType = value;
				updatePreviewSprite();
				SelectedToolType = ToolFactory.ToolType.Brush;
				TilesetPanel.Refresh();
			}
		}

		public void SetSelectedTile(int id)
		{
			SelectedTileType = Linker.Tilesets[0].Tiles[id - 1];
		}
	}
}
