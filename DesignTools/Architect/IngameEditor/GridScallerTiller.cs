using UnityEngine;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(MeshRenderer))]
	public class GridScallerTiller : PMonoBehaviour
	{
		const int textureGridSize = 32;

		[Min(0.0001f)]
		public float TileWidth = 1;
		[Min(0.0001f)]
		public float TileHeight = 1;

		[Min(1)]
		public int NbTilesX = 1;
		[Min(1)]
		public int NbTilesY = 1;

		public Color32 Color { set { CachedRenderer.sharedMaterial.color = value; } }

		readonly Lazy<MeshRenderer> cachedRenderer;
		public MeshRenderer CachedRenderer { get { return cachedRenderer; } }

		public Vector3 PositionOffset;

		public GridScallerTiller()
		{
			cachedRenderer = new Lazy<MeshRenderer>(GetComponent<MeshRenderer>);
		}

		void Update()
		{
			var scale = new Vector2(NbTilesX * TileWidth, NbTilesY * TileHeight);
			transform.localScale = scale;
			transform.localPosition = PositionOffset + new Vector3(scale.x / 2, scale.y / 2);
			CachedRenderer.sharedMaterial.mainTextureScale = new Vector2(NbTilesX, NbTilesY);

		}
	}
}
