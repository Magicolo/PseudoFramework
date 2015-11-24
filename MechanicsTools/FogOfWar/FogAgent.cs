using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Mechanics/Fog Agent")]
	public class FogAgent : PMonoBehaviour
	{
		[SerializeField, PropertyField]
		Vector3 offset;
		public Vector3 Offset
		{
			get { return offset; }
			set
			{
				offset = value;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float minRadius = 1f;
		public float MinRadius
		{
			get { return minRadius; }
			set
			{
				minRadius = Mathf.Clamp(value, 0f, MaxRadius / 2f);
				maxRadius = Mathf.Max(maxRadius, MinRadius);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float maxRadius = 10f;
		public float MaxRadius
		{
			get { return maxRadius; }
			set
			{
				maxRadius = Mathf.Max(value, MinRadius);
				minRadius = Mathf.Clamp(minRadius, 0f, MaxRadius / 2f);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 360f)]
		float cone = 360f;
		public float Cone
		{
			get { return cone; }
			set
			{
				cone = value;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 360f)]
		float angle = 360f;
		public float Angle
		{
			get { return angle; }
			set
			{
				angle = value;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 1f)]
		float strength = 1f;
		public float Strength
		{
			get { return strength; }
			set
			{
				strength = Mathf.Clamp(value, 0f, 1f);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 1f)]
		float preFalloff = 0.99F;
		public float PreFalloff
		{
			get { return preFalloff; }
			set
			{
				preFalloff = Mathf.Clamp(value, 0f, 1f);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 1f)]
		float falloff = 1f;
		public float Falloff
		{
			get { return falloff; }
			set
			{
				falloff = Mathf.Clamp(value, 0f, 1f);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		bool isStatic;
		public bool IsStatic
		{
			get { return isStatic; }
			set
			{
				isStatic = value;
				hasChanged = true;
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
				hasChanged = true;
			}
		}

		Vector3 position;
		public Vector3 Position { get { return position; } }

		Rect rect;
		public Rect Rect { get { return rect; } }

		bool isInView;
		public bool IsInView
		{
			get { return isInView; }
			set
			{
				if (isInView != value)
				{
					isInView = value;
					hasChanged |= !IsStatic;
				}
			}
		}

		bool hasChanged = true;
		public bool HasChanged
		{
			get
			{
				bool changed = hasChanged;
				hasChanged = false;

				return changed;
			}
			set { hasChanged = value; }
		}

		[SerializeField, Empty(PrefixLabel = "Fog Of War")]
		List<FogOfWar> fogsOfWar = new List<FogOfWar>();

		Dictionary<FogOfWar, Vector3> relativePositionsDict = new Dictionary<FogOfWar, Vector3>();

		void OnEnable()
		{
			for (int i = 0; i < fogsOfWar.Count; i++)
			{
				FogOfWar fogOfWar = fogsOfWar[i];
				fogOfWar.AddAgent(this);
				fogOfWar.UpdateFogOfWar = true;
				relativePositionsDict[fogOfWar] = Vector3.zero;
			}
		}

		void OnDisable()
		{
			for (int i = 0; i < fogsOfWar.Count; i++)
			{
				FogOfWar fogOfWar = fogsOfWar[i];
				fogOfWar.RemoveAgent(this);
				fogOfWar.UpdateFogOfWar = true;
				relativePositionsDict.Remove(fogOfWar);
			}
		}

		void Update()
		{
			CleanUp();

			position = Transform.position + offset;
			rect = new Rect(position.x - MaxRadius, position.y - MaxRadius, MaxRadius * 2, MaxRadius * 2);
			IsInView = Camera.main.WorldRectInView(rect);


			if (IsStatic)
			{
				if (HasChanged)
				{
					for (int i = 0; i < fogsOfWar.Count; i++)
					{
						FogOfWar fogOfWar = fogsOfWar[i];
						fogOfWar.UpdateFogOfWar = true;
					}
				}
			}
			else
			{
				for (int i = 0; i < fogsOfWar.Count; i++)
				{
					FogOfWar fogOfWar = fogsOfWar[i];
					Vector3 lastRelativePosition = relativePositionsDict[fogOfWar];
					Vector3 currentRelativePosition = fogOfWar.Transform.position - position;
					relativePositionsDict[fogOfWar] = currentRelativePosition;

					if (HasChanged || (lastRelativePosition != currentRelativePosition && rect.Intersects(fogOfWar.Area)))
						fogOfWar.UpdateFogOfWar = true;
				}
			}
		}

		public void AddFogOfWar(FogOfWar fogOfWar)
		{
			fogOfWar.UpdateFogOfWar = true;
			fogsOfWar.Add(fogOfWar);
			relativePositionsDict[fogOfWar] = Vector3.zero;
		}

		public void RemoveFogOfWar(FogOfWar fogOfWar)
		{
			fogOfWar.UpdateFogOfWar = true;
			fogsOfWar.Remove(fogOfWar);
			relativePositionsDict.Remove(fogOfWar);
		}

		public List<FogOfWar> GetFogsOfWar()
		{
			return fogsOfWar;
		}

		void CleanUp()
		{
			for (int i = fogsOfWar.Count - 1; i >= 0; i--)
			{
				if (fogsOfWar[i] == null)
					fogsOfWar.RemoveAt(i);
			}
		}
	}
}