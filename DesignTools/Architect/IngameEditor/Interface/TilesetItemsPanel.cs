using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using System;
using UnityEngine.UI;

public class TilesetItemsPanel : MonoBehaviour
{

    private CachedValue<Architect> ame;

    public TilesetItemsPanel()
    {
        ame = new CachedValue<Architect>(GetComponentInParent<Architect>);
    }

    void Start()
    {
        TileSet tileset = ame.Value.Linker.Tilesets[0];
        showTileset(tileset);
    }

    void Update()
    {

    }

    void showTileset(TileSet tileset)
    {
        for (int i = 0; i < tileset.Tiles.Count; i++)
        {
            TileType tileType = tileset[i];


        }
    }
}
