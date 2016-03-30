using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;
using Pseudo;
using Pseudo.Internal.Mechanics;

// TODO Add reduced update rates option
// TODO Add heightAgents for dynamic lighting

namespace Pseudo
{
	[RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Pseudo/Mechanics/Fog Of War")]
	public class FogOfWar : PMonoBehaviour
	{
		[SerializeField, PropertyField(typeof(RangeAttribute), 1, 25)]
		int definition = 1;
		public int Definition
		{
			get { return definition; }
			set
			{
				definition = Mathf.Clamp(value, 1, 25);

				if (Application.isPlaying)
				{
					CreateTexture();
					CreateHeightMap();
					CreateLineInfos();
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 5000)]
		int renderQueue;
		public int RenderQueue
		{
			get { return renderQueue; }
			set
			{
				renderQueue = value;

				if (Application.isPlaying)
				{
					CachedRenderer.material.renderQueue = renderQueue;
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField]
		FilterMode filterMode = FilterMode.Bilinear;
		public FilterMode FilterMode
		{
			get { return filterMode; }
			set
			{
				filterMode = value;

				if (Application.isPlaying)
				{
					CreateTexture();
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField]
		Color color = Color.black;
		public Color Color
		{
			get { return color; }
			set
			{
				color = value;

				if (Application.isPlaying)
				{
					CreateTexture();
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 25f)]
		float fadeSpeed = 5;
		public float FadeSpeed
		{
			get { return fadeSpeed; }
			set
			{
				fadeSpeed = value;
				UpdateFogOfWar = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 1f)]
		float flicker;
		public float Flicker
		{
			get { return flicker; }
			set
			{
				flicker = value;
				UpdateFogOfWar = true;
			}
		}

		[SerializeField, PropertyField]
		bool inverted;
		public bool Inverted
		{
			get { return inverted; }
			set
			{
				inverted = value;

				if (Application.isPlaying)
				{
					CreateTexture();
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField(typeof(ToggleAttribute))]
		bool generateHeightMap;
		public bool GenerateHeightMap
		{
			get { return generateHeightMap; }
			set
			{
				generateHeightMap = value;

				if (Application.isPlaying)
				{
					CreateHeightMap();
					UpdateFogOfWar = true;
				}
			}
		}

		[SerializeField, PropertyField(DisableBool = "!GenerateHeightMap", Indent = 1)]
		LayerMask layerMask;
		public LayerMask LayerMask { get { return layerMask; } set { layerMask = value; } }

		bool updateFogOfWar = true;
		public bool UpdateFogOfWar
		{
			get { return updateFogOfWar; }
			set
			{
				updateFogOfWar = value;

				if (updateFogOfWar)
					updateCount = 1f;
			}
		}

		int mapWidth;
		public int Width { get { return mapWidth; } }

		int mapHeight;
		public int Height { get { return mapHeight; } }

		Rect area;
		public Rect Area { get { return area; } }

		readonly Lazy<SpriteRenderer> cachedRenderer;
		public SpriteRenderer CachedRenderer { get { return cachedRenderer; } }

		float updateCount;
		int mapDiagonal;
		Texture2D texture;
		Vector3 currentPosition;
		Vector3 currentScale;
		float deltaTime;
		float flickerAmount;
		Color[] currentPixels;
		float[,] currentAlphaMap;
		float[,] currentHeightMap;
		Thread updateThread;
		AutoResetEvent updateWaitHandle;
		List<LineOfSightInfo>[,] currentLineInfos;

		List<FogAgent> fogAgents = new List<FogAgent>();

		public FogOfWar()
		{
			cachedRenderer = new Lazy<SpriteRenderer>(GetComponent<SpriteRenderer>);
		}

		void Awake()
		{
			CachedRenderer.material.renderQueue = renderQueue;
			UpdateArea();
			CreateTexture();
			CreateHeightMap();
			CreateLineInfos();
		}

		void OnEnable()
		{
			UpdateFogOfWar = true;
		}

		protected override void Start()
		{
			base.Start();

			UpdateFogOfWar = true;
			StartCoroutine(UpdateRoutine());
		}

		void OnBecameVisible()
		{
			UpdateFogOfWar = true;
		}

		void OnBecameInvisible()
		{
			UpdateFogOfWar = true;
		}

		void UpdateArea()
		{
			CleanUp();

			currentScale = CachedTransform.lossyScale;
			currentPosition = CachedTransform.position - currentScale / 2;
			area = new Rect(currentPosition.x, currentPosition.y, currentScale.x, currentScale.y);
		}

		IEnumerator UpdateRoutine()
		{
			while (true)
			{
				UpdateArea();

				updateFogOfWar |= updateCount >= 0;

				if (updateFogOfWar && CachedRenderer.isVisible && CachedGameObject.activeInHierarchy)
				{
					deltaTime = Time.deltaTime;
					flickerAmount = (Random.value * 2 - 1) * Flicker;

					if (updateThread == null)
					{
						updateWaitHandle = new AutoResetEvent(false);
						updateThread = new Thread(new ThreadStart(UpdateFowAsync));
						updateThread.Start();
					}
					else
						updateWaitHandle.Set();

					while (updateThread.ThreadState == ThreadState.Running)
						yield return new WaitForSeconds(0f);

					texture.SetPixels(currentPixels);
					texture.Apply();

					updateFogOfWar = false;
					updateCount -= FadeSpeed * deltaTime;
				}

				yield return new WaitForSeconds(0);
			}
		}

		void UpdateFowAsync()
		{
			while (true)
			{
				try
				{
					float[,] alphaMap = new float[mapWidth, mapHeight];

					UpdateAlphaMap(currentAlphaMap);
					UpdateTexture(currentAlphaMap);

					currentAlphaMap = alphaMap;
				}
				catch (System.Exception exception)
				{
					Debug.LogError(exception);
				}

				updateWaitHandle.WaitOne();
			}
		}

		void UpdateAlphaMap(float[,] alphaMap)
		{
			for (int i = fogAgents.Count - 1; i >= 0; i--)
				ModifyFog(alphaMap, fogAgents[i]);
		}

		void UpdateTexture(float[,] alphaMap)
		{
			int xLength = alphaMap.GetLength(0);
			int yLength = alphaMap.GetLength(1);
			float adjustedFadeSpeed = FadeSpeed * deltaTime;

			if (currentPixels.Length >= xLength * yLength)
			{
				int pixelCounter = 0;

				for (int y = 0; y < yLength; y++)
				{
					for (int x = 0; x < xLength; x++)
					{
						float currentAlpha = currentPixels[pixelCounter].a;
						float alpha = Inverted ? alphaMap[x, y] : 1 - alphaMap[x, y];
						alpha += flickerAmount * alpha;
						float difference = alpha - currentAlpha;

						if (difference > adjustedFadeSpeed)
							currentPixels[pixelCounter].a += adjustedFadeSpeed;
						else if (difference < -adjustedFadeSpeed)
							currentPixels[pixelCounter].a -= adjustedFadeSpeed;
						else if (difference != 0)
							currentPixels[pixelCounter].a = alpha;

						pixelCounter += 1;
					}
				}
			}
		}

		void ModifyFog(float[,] alphaMap, Vector3 position, float minRadius, float maxRadius, float cone, float angle, float strength, float preFalloff, float falloff, bool invert)
		{
			Vector2 texturePosition = WorldToPixel(position);
			float minPixelRadius = Mathf.Min(minRadius * Definition, Mathf.Floor(mapDiagonal / 2 - 1));
			float maxPixelRadius = Mathf.Min(maxRadius * Definition, Mathf.Floor(mapDiagonal / 2 - 1));
			float adjustedPrefalloff = preFalloff.Pow(1F / Definition);
			float adjustedFalloff = falloff.Pow(1F / Definition);
			bool insideRect = Area.Contains(position);
			int x = (int)texturePosition.x.Round();
			int y = (int)texturePosition.y.Round();

			if (maxPixelRadius <= 0 || cone <= 0 || strength <= 0 || FadeSpeed <= 0)
				return;

			if (x >= 0 && x < alphaMap.GetLength(0) && y >= 0 && y < alphaMap.GetLength(1))
			{
				alphaMap[(int)texturePosition.x.Round(), (int)texturePosition.y.Round()] = invert ? 1 - strength : strength;
			}

			List<LineOfSightInfo> centerInfos = currentLineInfos[0, 0];
			float halfCone = cone / 2;

			for (int i = 0; i < centerInfos.Count; i++)
			{
				float lineAngle = i * 45;
				float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(lineAngle, angle));
				float difference = deltaAngle - halfCone;
				float adjustedStrength = strength * (1 - Mathf.Clamp01(difference / 45)) * Mathf.Clamp01(halfCone / 45);

				PrimaryLineOfSight lineOfSight = new PrimaryLineOfSight(centerInfos[i], x, y, minPixelRadius, maxPixelRadius, adjustedStrength, adjustedPrefalloff, adjustedFalloff, invert, alphaMap, currentHeightMap, currentLineInfos);

				if (insideRect || (position.x < Area.xMin && lineOfSight.info.directionX >= 0) || (position.x > Area.xMax && lineOfSight.info.directionX <= 0) || (position.y < Area.yMin && lineOfSight.info.directionY >= 0) || (position.y > Area.yMax && lineOfSight.info.directionY <= 0))
					lineOfSight.Complete();
			}
		}

		void ModifyFog(float[,] alphaMap, FogAgent fogAgent)
		{
			if (fogAgent != null && (fogAgent.IsStatic || fogAgent.IsInView))
				ModifyFog(alphaMap, fogAgent.Position, fogAgent.MinRadius, fogAgent.MaxRadius, fogAgent.Cone, fogAgent.Angle, fogAgent.Strength, fogAgent.PreFalloff, fogAgent.Falloff, fogAgent.Inverted);
		}

		void CreateTexture()
		{
			mapWidth = Mathf.Abs((int)(CachedTransform.lossyScale.x * Definition).Round());
			mapHeight = Mathf.Abs((int)(CachedTransform.lossyScale.y * Definition).Round());
			mapDiagonal = (int)Mathf.Ceil(Mathf.Sqrt(mapWidth * mapWidth + mapHeight * mapHeight)) * 2;
			currentAlphaMap = new float[mapWidth, mapHeight];
			texture = new Texture2D(mapWidth, mapHeight, TextureFormat.RGBA32, false);
			texture.filterMode = filterMode;
			texture.wrapMode = TextureWrapMode.Clamp;
			CachedRenderer.material.SetTexture("_AlphaMap", texture);

			currentPixels = new Color[mapWidth * mapHeight];

			for (int i = 0; i < currentPixels.Length; i++)
			{
				currentPixels[i] = Color;
				currentPixels[i].a = Inverted ? 0 : 1;
			}

			texture.SetPixels(currentPixels);
		}

		void CreateHeightMap()
		{
			currentHeightMap = new float[mapWidth, mapHeight];

			if (GenerateHeightMap)
			{
				for (int y = 0; y < mapHeight; y++)
				{
					for (int x = 0; x < mapWidth; x++)
					{
						Vector3 position = new Vector3(Area.xMin + (x + 0.5F) / Definition, Area.yMin + (y + 0.5F) / Definition, CachedTransform.position.z);

						if (Physics.Raycast(position - Vector3.forward * 100, Vector3.forward, Mathf.Infinity, LayerMask))
							currentHeightMap[x, y] = 1;
					}
				}
			}
		}

		void CreateLineInfos()
		{
			currentLineInfos = new List<LineOfSightInfo>[mapDiagonal * 2, mapDiagonal * 2];
			List<LineOfSightInfo> centerInfos = new List<LineOfSightInfo>();

			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, 0));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 0, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, 0));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, 1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 0, 1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, 1));

			currentLineInfos[0, 0] = centerInfos;

			for (int i = 0; i < centerInfos.Count; i++)
			{
				LineOfSightInfo lineInfo = centerInfos[i];
				lineInfo.GeneratePoints(mapDiagonal);

				for (int j = 1; j < lineInfo.points.Length; j++)
				{
					PointInfo point = lineInfo.GetNextPoint();
					List<LineOfSightInfo> infos = new List<LineOfSightInfo>();

					Vector2 direction = new Vector2(lineInfo.directionX, lineInfo.directionY).Rotate(-45).Round();
					infos.Add(new LineOfSightInfo(point.coordinateX, point.coordinateY, (int)direction.x, (int)direction.y));

					direction = new Vector2(lineInfo.directionX, lineInfo.directionY).Rotate(45).Round();
					infos.Add(new LineOfSightInfo(point.coordinateX, point.coordinateY, (int)direction.x, (int)direction.y));
					infos.ForEach(info => info.GeneratePoints(mapDiagonal));

					currentLineInfos[point.coordinateX, point.coordinateY] = infos;
				}
			}
		}

		void CleanUp()
		{
			for (int i = fogAgents.Count - 1; i >= 0; i--)
			{
				if (fogAgents[i] == null)
					fogAgents.RemoveAt(i);
			}
		}

		public Vector2 WorldToPixel(Vector3 worldPoint)
		{
			return new Vector2((worldPoint.x - currentPosition.x) * Definition, (worldPoint.y - currentPosition.y) * Definition);
		}

		public void AddAgent(FogAgent agent)
		{
			fogAgents.Add(agent);
			UpdateFogOfWar = true;
		}

		public void RemoveAgent(FogAgent agent)
		{
			fogAgents.Remove(agent);
			UpdateFogOfWar = true;
		}

		public void RemoveAgent(int index)
		{
			fogAgents.RemoveAt(index);
			UpdateFogOfWar = true;
		}

		public List<FogAgent> GetAgents()
		{
			return fogAgents;
		}

		public FogAgent GetAgent(int index)
		{
			FogAgent agent = null;

			try
			{
				agent = fogAgents[index];
			}
			catch
			{
				Debug.LogError(string.Format("Fog agent at index {0} could not be found.", index));
			}

			return agent;
		}

		public void SetAgents(IList<FogAgent> fogAgents)
		{
			this.fogAgents = new List<FogAgent>(fogAgents);
			UpdateFogOfWar = true;
		}

		public Color GetColor(Vector3 worldPoint)
		{
			return GetColor(WorldToPixel(worldPoint));
		}

		public Color GetColor(Vector2 pixel)
		{
			return currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()];
		}

		public void SetColor(Vector3 worldPoint, Color color)
		{
			SetColor(WorldToPixel(worldPoint), color, Channels.RGB);
		}

		public void SetColor(Vector2 pixel, Color color)
		{
			SetColor(pixel, color, Channels.RGB);
		}

		public void SetColor(Vector3 worldPoint, Color color, Channels channels)
		{
			SetColor(WorldToPixel(worldPoint), color, channels);
		}

		public void SetColor(Vector2 pixel, Color color, Channels channels)
		{
			Color currentColor = currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()];
			currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()] = currentColor.SetValues(color, channels);
		}

		public float GetAlpha(Vector3 worldPoint)
		{
			return GetAlpha(WorldToPixel(worldPoint));
		}

		public float GetAlpha(Vector2 pixel)
		{
			return currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		}

		public void SetAlpha(Vector3 worldPoint, float alpha)
		{
			if (Area.Contains(worldPoint))
				SetAlpha(WorldToPixel(worldPoint), alpha);
		}

		public void SetAlpha(Vector2 pixel, float alpha)
		{
			currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()] = alpha;
		}

		public float GetHeight(Vector3 worldPoint)
		{
			return GetHeight(WorldToPixel(worldPoint));
		}

		public float GetHeight(Vector2 pixel)
		{
			return currentHeightMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		}

		public void SetHeight(Vector3 worldPoint, float height)
		{
			if (Area.Contains(worldPoint))
				SetHeight(WorldToPixel(worldPoint), height);
		}

		public void SetHeight(Vector2 pixel, float height)
		{
			currentHeightMap[(int)pixel.x.Round(), (int)pixel.y.Round()] = height;
		}

		public void SetHeightMap(float[,] heightMap)
		{
			currentHeightMap = heightMap;
		}

		public bool IsFogged(Vector3 worldPoint)
		{
			return Area.Contains(worldPoint) && IsFogged(WorldToPixel(worldPoint), 0);
		}

		public bool IsFogged(Vector2 pixel)
		{
			return IsFogged(pixel, 0);
		}

		public bool IsFogged(Vector3 worldPoint, float alphaThreshold)
		{
			return Area.Contains(worldPoint) && IsFogged(WorldToPixel(worldPoint), alphaThreshold);
		}

		public bool IsFogged(Vector2 pixel, float alphaThreshold)
		{
			return currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()] <= alphaThreshold;
		}

		protected virtual void Reset()
		{
			CachedRenderer.sprite = AssetDatabaseUtility.LoadAssetInFolder<Sprite>("SquareSprite.png", "GraphicsTools");
			CachedRenderer.sharedMaterial = AssetDatabaseUtility.LoadAssetInFolder<Material>("FogOfWar.mat", "FogOfWar");
			renderQueue = CachedRenderer.sharedMaterial.renderQueue;
			Definition = 1;
		}
	}
}
