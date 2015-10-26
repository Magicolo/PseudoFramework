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
		float minRadius = 1;
		public float MinRadius
		{
			get { return minRadius; }
			set
			{
				minRadius = Mathf.Clamp(value, 0, MaxRadius / 2);
				maxRadius = Mathf.Max(maxRadius, MinRadius);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float maxRadius = 10;
		public float MaxRadius
		{
			get { return maxRadius; }
			set
			{
				maxRadius = Mathf.Max(value, MinRadius);
				minRadius = Mathf.Clamp(minRadius, 0, MaxRadius / 2);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 360)]
		float cone = 360;
		public float Cone
		{
			get { return cone; }
			set
			{
				cone = value;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 360)]
		float angle = 360;
		public float Angle
		{
			get { return angle; }
			set
			{
				angle = value;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float strength = 1;
		public float Strength
		{
			get { return strength; }
			set
			{
				strength = Mathf.Clamp(value, 0, 1);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float preFalloff = 0.99F;
		public float PreFalloff
		{
			get { return preFalloff; }
			set
			{
				preFalloff = Mathf.Clamp(value, 0, 1);
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float falloff = 1;
		public float Falloff
		{
			get { return falloff; }
			set
			{
				falloff = Mathf.Clamp(value, 0, 1);
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

		Dictionary<FogOfWar, Vector3> relativePositionsDict;
		Dictionary<FogOfWar, Vector3> RelativePositionsDict
		{
			get
			{
				if (relativePositionsDict == null)
					relativePositionsDict = new Dictionary<FogOfWar, Vector3>();

				return relativePositionsDict;
			}
		}

		[SerializeField, Empty(PrefixLabel = "Fog Of War")]
		List<FogOfWar> fogsOfWar;
		List<FogOfWar> FogsOfWar
		{
			get
			{
				fogsOfWar = fogsOfWar ?? new List<FogOfWar>();

				return fogsOfWar;
			}
		}

		void OnEnable()
		{
			for (int i = 0; i < FogsOfWar.Count; i++)
			{
				FogOfWar fogOfWar = FogsOfWar[i];
				fogOfWar.AddAgent(this);
				fogOfWar.UpdateFogOfWar = true;
				RelativePositionsDict[fogOfWar] = Vector3.zero;
			}
		}

		void OnDisable()
		{
			for (int i = 0; i < FogsOfWar.Count; i++)
			{
				FogOfWar fogOfWar = FogsOfWar[i];
				fogOfWar.RemoveAgent(this);
				fogOfWar.UpdateFogOfWar = true;
				RelativePositionsDict.Remove(fogOfWar);
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
					for (int i = 0; i < FogsOfWar.Count; i++)
					{
						FogOfWar fogOfWar = FogsOfWar[i];
						fogOfWar.UpdateFogOfWar = true;
					}
				}
			}
			else
			{
				for (int i = 0; i < FogsOfWar.Count; i++)
				{
					FogOfWar fogOfWar = FogsOfWar[i];
					Vector3 lastRelativePosition = RelativePositionsDict[fogOfWar];
					Vector3 currentRelativePosition = fogOfWar.Transform.position - position;
					RelativePositionsDict[fogOfWar] = currentRelativePosition;

					if (HasChanged || (lastRelativePosition != currentRelativePosition && rect.Intersects(fogOfWar.Area)))
					{
						fogOfWar.UpdateFogOfWar = true;
					}
				}
			}
		}

		public void AddFogOfWar(FogOfWar fogOfWar)
		{
			fogOfWar.UpdateFogOfWar = true;
			FogsOfWar.Add(fogOfWar);
			RelativePositionsDict[fogOfWar] = Vector3.zero;
		}

		public void RemoveFogOfWar(FogOfWar fogOfWar)
		{
			fogOfWar.UpdateFogOfWar = true;
			FogsOfWar.Remove(fogOfWar);
			RelativePositionsDict.Remove(fogOfWar);
		}

		public FogOfWar[] GetFogsOfWar()
		{
			return FogsOfWar.ToArray();
		}

		void CleanUp()
		{
			for (int i = FogsOfWar.Count - 1; i >= 0; i--)
			{
				if (FogsOfWar[i] == null)
					FogsOfWar.RemoveAt(i);
			}
		}
	}
}