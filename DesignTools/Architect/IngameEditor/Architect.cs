using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace Pseudo
{
    public class Architect : MonoBehaviour
    {

        public ArchitectLinker Linker;

        public TileType SelectedTileType;

        [Range(0, 1)]
        public float DrawingWidth = 0.8f;
        [Range(0, 1)]
        public float DrawingHeight = 0.9f;
        private Rect drawingRect;

        public Camera Cam;

        public LayerData Layer = new LayerData(90, 90);

        void Start()
        {
            SelectedTileType = Linker.Tilesets[0].Tiles[0];
            Layer.TileHeight = 1;
            Layer.TileWidth = 1;
        }

        void Update()
        {
            drawingRect = new Rect(0, 0, Screen.width * DrawingWidth, Screen.height * DrawingHeight);
            if (Input.GetMouseButton(0))
            {
                handleUseClick();

            }
            else if (Input.GetMouseButton(1))
            {
                HandleErase();
            }
        }

        private void HandleErase()
        {
            if (drawingRect.Contains(Input.mousePosition))
            {
                Vector3 p = Cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 TileP = p.Div(new Vector3(Layer.TileWidth, Layer.TileHeight, 1)).Round().SetValues(0, Axes.Z);
                // Vector3 worldP = TileP.Mult(new Vector3(Layer.TileWidth, Layer.TileHeight, 1));
                Point2 tilePoint = new Point2(TileP.ToVector2());


                if (Layer.IsInArrayBound(tilePoint))
                {
                    if (Layer[tilePoint.X, tilePoint.Y] != null)
                    {
                        Layer[tilePoint.X, tilePoint.Y].GameObject.Destroy();
                        Layer[tilePoint.X, tilePoint.Y] = null;
                    }
                }
            }
        }

        void handleUseClick()
        {

            if (drawingRect.Contains(Input.mousePosition))
            {
                Vector3 p = Cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 TileP = p.Div(new Vector3(Layer.TileWidth, Layer.TileHeight, 1)).Round().SetValues(0, Axes.Z);
                Vector3 worldP = TileP.Mult(new Vector3(Layer.TileWidth, Layer.TileHeight, 1));
                Point2 tilePoint = new Point2(TileP.ToVector2());


                if (Layer.IsInArrayBound(tilePoint))
                {
                    if (Layer[tilePoint.X, tilePoint.Y] == null)
                    {
                        GameObject newTile = (UnityEngine.GameObject)PrefabUtility.InstantiatePrefab(SelectedTileType.Prefab);
                        newTile.transform.SetPosition(worldP);

                        TileData tileData = new TileData(SelectedTileType, newTile);
                        Layer[tilePoint.X, tilePoint.Y] = tileData;
                    }

                }
            }
        }
    }

}
