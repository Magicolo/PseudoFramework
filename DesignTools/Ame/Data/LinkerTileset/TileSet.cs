using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
    [System.Serializable]
    public class TileSet
    {
        public string Name;
      
        public List<TileType> Tiles = new List<TileType>();


        public TileSet(string name)
        {
            this.Name = name;
        }

        
    }
    
}
