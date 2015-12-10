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

        public Transform layerTransform;

        public LayerData(Transform parent, string name, int layerWidth, int layerheight)
        {
            LayerWidth = layerWidth;
            LayerHeight = layerheight;
            tiles = new TileData[layerWidth * LayerHeight];
            tiles.Fill(TileData.Empty);

            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            layerTransform = gameObject.transform;
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

        /// <summary>
        /// After a Destroy, this instance is unusable, a new instance must be instanciated.
        /// </summary>
        public void DestroyAllAndClear()
        {
            if (tiles == null) return;
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].GameObject.Destroy();
            }
            layerTransform.gameObject.Destroy();
        }

        public bool IsInArrayBound(int x, int y)
        {
            return x.IsBetweenInclusive(0, LayerWidth) && y.IsBetweenInclusive(0, LayerHeight);
        }

        public bool IsInArrayBound(Vector2 vector2)
        {
            return IsInArrayBound((int)vector2.x, (int)vector2.y);
        }

        public int Count
        {
            get { return tiles.Length; }
        }
    }
}


