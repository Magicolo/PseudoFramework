using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class TilesetItemsPanel : MonoBehaviour
{
    private Architect architect;

    private List<Button> tilesetButtons = new List<Button>();

    public Color SelectedColor;
    public Color BaseColor;

    void Awake()
    {
        architect = GetComponentInParent<Architect>();
    }

    void Start()
    {
        TileSet tileset = architect.Linker.Tilesets[0];
        tilesetButtons.Clear();
        showTileset(tileset);
    }

    void Update()
    {

    }

    void showTileset(TileSet tileset)
    {
        int x = 20; int y = -20;
        Vector2 dimension = new Vector2(32, 32);

        for (int i = 0; i < tileset.Tiles.Count; i++)
        {
            TileType tileType = tileset[i];
            Vector3 position = new Vector3(x, y);
            UnityAction action = () => buttonClicked(tileType.Id);
            Button button = architect.UiFactory.CreateImageButton(transform, position, dimension, tileType.PreviewSprite, BaseColor, action);

            tilesetButtons.Add(button);

            x += 40;
        }
    }

    void buttonClicked(int id)
    {
        tilesetButtons[architect.SelectedTileType.Id - 1].GetComponent<Image>().color = BaseColor;
        architect.setSelectedTile(id);
        tilesetButtons[architect.SelectedTileType.Id - 1].GetComponent<Image>().color = SelectedColor;
    }
}
