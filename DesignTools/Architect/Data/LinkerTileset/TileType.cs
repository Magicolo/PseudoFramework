using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class TileType
	{
		public int Id;
		public GameObject Prefab;
		public Sprite PreviewSprite;

		public TileType(int id, GameObject prefab = null)
		{
			this.Id = id;
			this.Prefab = prefab;
		}

		public override string ToString()
		{
			string prefabName = (Prefab == null) ? "No Prefab" : "Prefab Name:" + Prefab.name;
			return "TileType (" + Id + ", " + prefabName + ")";
		}
	}

	public static class TileTypeExtentions
	{
		public static bool IsNullOrIdZero(this TileType tileType)
		{
			return tileType == null || tileType.Id == 0;
		}
	}
}