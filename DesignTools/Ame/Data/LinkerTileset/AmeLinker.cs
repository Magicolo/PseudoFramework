using UnityEngine;
using System.Collections.Generic;

namespace Pseudo
{
    [System.Serializable]
    public class AmeLinker : ScriptableObject
    {
        public int savedTime = 0;
        public List<TileSet> Tilesets = new List<TileSet>(); 
    }

}
