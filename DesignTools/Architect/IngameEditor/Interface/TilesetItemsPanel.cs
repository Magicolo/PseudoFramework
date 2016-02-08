﻿using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class TilesetItemsPanel : MonoBehaviour
{
	private ArchitectOld architect;

	private List<Button> tilesetButtons = new List<Button>();

	public Color SelectedColor;
	public Color BaseColor;
	int currentSelectId;

	void Awake()
	{
		architect = GetComponentInParent<ArchitectOld>();
	}

	void Start()
	{
		Refresh();
	}

	void Update()
	{

	}

	void showTileset(TileSet tileset)
	{

		float width = GetComponent<RectTransform>().rect.width;
		int x = 20; int y = -20;
		Vector2 dimension = new Vector2(32, 32);

		for (int i = 0; i < tileset.Tiles.Count; i++)
		{
			TileType tileType = tileset[i];
			Vector3 position = new Vector3(x, y);
			UnityAction action = () => buttonClicked(tileType.Id);
			Button button = architect.UiFactory.CreateImageButton(transform, position, dimension, tileType.PreviewSprite, BaseColor, action);
			button.GetComponent<RectTransform>().localScale = Vector3.one;
			tilesetButtons.Add(button);

			x += 40;
			if (x + 40 >= width)
			{
				x = 20;
				y -= 40;
			}
		}
	}

	void buttonClicked(int id)
	{
		architect.setSelectedTile(id);
	}

	public void Refresh()
	{
		TileSet tileset = architect.Linker.Tilesets[0];
		clearTilesetButtons();
		showTileset(tileset);

		selectTile(architect.SelectedTileType.Id - 1);
	}

	private void clearTilesetButtons()
	{
		for (int i = 0; i < tilesetButtons.Count; i++)
		{
			tilesetButtons[i].gameObject.Destroy();
		}
		tilesetButtons.Clear();
	}

	private void selectTile(int id)
	{
		if (currentSelectId >= 0)
			tilesetButtons[currentSelectId].GetComponent<Image>().color = BaseColor;

		currentSelectId = id;

		if (currentSelectId >= 0)
			tilesetButtons[currentSelectId].GetComponent<Image>().color = SelectedColor;

	}
}
