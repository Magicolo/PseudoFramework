using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
    [System.Serializable]
    public class TileData
    {
        public TileType TileType;
        public GameObject GameObject;

        public TileData(TileType tileType, GameObject gameObject)
        {
            this.TileType = tileType;
            this.GameObject = gameObject;
        }
    }

}
