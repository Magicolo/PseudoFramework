using System;
using UnityEngine;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Smooth/Follow")]
	public class SmoothFollow : PMonoBehaviour, ICopyable<SmoothFollow>
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ, AfterSeparator = true)]
		public Axes Axes = Axes.XYZ;
		public PTime.TimeChannels TimeChannel;

		public Transform Target;
		public Vector3 Offset;
		[Clamp(0, 100)]
		public Vector3 Damping = new Vector3(100, 100, 100);

		void FixedUpdate()
		{
			if (Mode == TransformModes.None || Axes == Axes.None)
				return;

			if ((Mode & TransformModes.Position) != 0)
			{
				Vector3 position = CachedTransform.position;

				position.x = (Axes & Axes.X) != 0 ? Damping.x >= 100 ? Target.position.x + Offset.x : Mathf.Lerp(position.x, Target.position.x + Offset.x, Damping.x * PTime.GetFixedDeltaTime(TimeChannel)) : position.x;
				position.y = (Axes & Axes.Y) != 0 ? Damping.y >= 100 ? Target.position.y + Offset.y : Mathf.Lerp(position.y, Target.position.y + Offset.y, Damping.y * PTime.GetFixedDeltaTime(TimeChannel)) : position.y;
				position.z = (Axes & Axes.Z) != 0 ? Damping.z >= 100 ? Target.position.z + Offset.z : Mathf.Lerp(position.z, Target.position.z + Offset.z, Damping.z * PTime.GetFixedDeltaTime(TimeChannel)) : position.z;

				CachedTransform.position = position;
			}

			if ((Mode & TransformModes.Rotation) != 0)
			{
				Vector3 eulerAngles = CachedTransform.eulerAngles;

				eulerAngles.x = (Axes & Axes.X) != 0 ? Damping.x >= 100 ? Target.eulerAngles.x + Offset.x : Mathf.Lerp(eulerAngles.x, Target.eulerAngles.x + Offset.x, Damping.x * PTime.GetFixedDeltaTime(TimeChannel)) : eulerAngles.x;
				eulerAngles.y = (Axes & Axes.Y) != 0 ? Damping.y >= 100 ? Target.eulerAngles.y + Offset.y : Mathf.Lerp(eulerAngles.y, Target.eulerAngles.y + Offset.y, Damping.y * PTime.GetFixedDeltaTime(TimeChannel)) : eulerAngles.y;
				eulerAngles.z = (Axes & Axes.Z) != 0 ? Damping.z >= 100 ? Target.eulerAngles.z + Offset.z : Mathf.Lerp(eulerAngles.z, Target.eulerAngles.z + Offset.z, Damping.z * PTime.GetFixedDeltaTime(TimeChannel)) : eulerAngles.z;

				CachedTransform.eulerAngles = eulerAngles;
			}

			if ((Mode & TransformModes.Scale) != 0)
			{
				Vector3 scale = CachedTransform.lossyScale;

				scale.x = (Axes & Axes.X) != 0 ? Damping.x >= 100 ? Target.lossyScale.x + Offset.x : Mathf.Lerp(scale.x, Target.lossyScale.x + Offset.x, Damping.x * PTime.GetFixedDeltaTime(TimeChannel)) : scale.x;
				scale.y = (Axes & Axes.Y) != 0 ? Damping.y >= 100 ? Target.lossyScale.y + Offset.y : Mathf.Lerp(scale.y, Target.lossyScale.y + Offset.y, Damping.y * PTime.GetFixedDeltaTime(TimeChannel)) : scale.y;
				scale.z = (Axes & Axes.Z) != 0 ? Damping.z >= 100 ? Target.lossyScale.z + Offset.z : Mathf.Lerp(scale.z, Target.lossyScale.z + Offset.z, Damping.z * PTime.GetFixedDeltaTime(TimeChannel)) : scale.z;

				CachedTransform.SetScale(scale);
			}
		}

		public void Copy(SmoothFollow reference)
		{
			Mode = reference.Mode;
			Axes = reference.Axes;
			TimeChannel = reference.TimeChannel;
			Target = reference.Target;
			Offset = reference.Offset;
			Damping = reference.Damping;
		}
	}
}
