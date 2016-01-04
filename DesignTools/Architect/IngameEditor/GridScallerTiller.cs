using UnityEngine;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[ExecuteInEditMode, RequireComponent(typeof(MeshRenderer))]
	public class GridScallerTiller : PMonoBehaviour
	{
		const int textureGridSize = 32;

		[Min(1)]
		public int TileWidth = 1;
		[Min(1)]
		public int TileHeight = 1;

		[Min(1)]
		public int NbTilesX = 1;
		[Min(1)]
		public int NbTilesY = 1;

		[Min(1)]
		public int GridWidth = 1;
		[Min(1)]
		public int GridHeight = 1;

		public Color32 Color { set { CachedRenderer.sharedMaterial.color = value; } }

		readonly CachedValue<MeshRenderer> cachedRenderer;
		public MeshRenderer CachedRenderer { get { return cachedRenderer; } }

		public GridScallerTiller()
		{
			cachedRenderer = new CachedValue<MeshRenderer>(GetComponent<MeshRenderer>);
		}

		void Update()
		{
			float tileScaleX = 1f * textureGridSize / TileWidth;
			float tileScaleY = 1f * textureGridSize / TileHeight;
			Vector2 scale = new Vector2(GridWidth * tileScaleX, GridHeight / tileScaleY);
			CachedTransform.localScale = scale;

			CachedRenderer.sharedMaterial.mainTextureScale = new Vector2(GridWidth * tileScaleX, GridHeight * tileScaleY);

		}
	}
}
