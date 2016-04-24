using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class TilesetItemsPanel : MonoBehaviour
{
	ArchitectBehavior architectBehavior;
	Architect architect;
	DrawingControlerView DrawingControler;

	List<Button> tilesetButtons = new List<Button>();

	Color SelectedColor { get { return architectBehavior.Skin.SelectedButtonBackground; } }
	Color BaseColor { get { return architectBehavior.Skin.EnabledButtonBackground; } }

	int currentSelectId;

	void Awake()
	{
		architectBehavior = GetComponentInParent<ArchitectBehavior>();
		architect = architectBehavior.Architect;
		DrawingControler = architectBehavior.GetComponent<DrawingControlerView>();
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
		for (int i = 0; i < tileset.Tiles.Count; i++)
		{
			TileType tileType = tileset[i];
			UnityAction action = () => buttonClicked(tileType.Id);
			Button button = architectBehavior.UIFactory.CreateImageButton(transform, Vector3.zero, Vector3.zero, tileType.PreviewSprite, BaseColor, action);
			button.GetComponent<RectTransform>().localScale = Vector3.one;
			tilesetButtons.Add(button);
		}
	}

	void buttonClicked(int id)
	{
		DrawingControlerView.SetSelectedTile(id);
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
