using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
    [System.Serializable]
    public class TileType
    {
        public int Id;
        public GameObject Prefab;

        public TileType(int id, GameObject prefab = null)
        {
            this.Id = id;
            this.Prefab = prefab;
        }
    }
}