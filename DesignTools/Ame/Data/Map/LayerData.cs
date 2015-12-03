using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
    [System.Serializable]
    public class LayerData
    {
        public float TileWidth = 1;
        public float TileHeight = 1;

        public readonly int LayerWidth;
        public readonly int LayerHeight;

        TileData[] tiles;

        public LayerData(int layerWidth, int layerheight)
        {
            LayerWidth = layerWidth;
            LayerHeight = layerheight;
            tiles = new TileData[layerWidth * LayerHeight];
        }

        public TileData this[int x, int y]
        {
            get { return tiles[x + y * LayerWidth]; }
            set { tiles[x + y * LayerWidth] = value; }
        }
        public TileData this[Point2 point]
        {
            get
            {
                PDebug.Log(point.X + point.Y * LayerWidth);
                return tiles[point.X + point.Y * LayerWidth];
            }
            set { tiles[point.X + point.Y * LayerWidth] = value; }
        }
        public bool IsInArrayBound(int x, int y)
        {
            return x.IsBetween(0, LayerWidth) && y.IsBetween(0, LayerHeight);
        }

        public bool IsInArrayBound(Vector2 vector2)
        {
            return IsInArrayBound((int)vector2.x, (int)vector2.y);
        }
    }

}
