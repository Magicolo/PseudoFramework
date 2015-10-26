using System;
using UnityEngine;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Smooth/Follow")]
	public class SmoothFollow : MonoBehaviourExtended, ICopyable<SmoothFollow>
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ, AfterSeparator = true)]
		public Axes Axes = Axes.XYZ;
		public Kronos.TimeChannels TimeChannel;

		public Transform Target;
		public Vector3 Offset;
		[Clamp(0, 100)]
		public Vector3 Damping = new Vector3(100, 100, 100);

		void FixedUpdate()
		{
			if (Mode == TransformModes.None || Axes == Axes.None)
				return;

			if (Mode.Contains(TransformModes.Position))
			{
				Vector3 position = transform.position;

				position.x = Axes.Contains(Axes.X) ? Damping.x >= 100 ? Target.position.x + Offset.x : Mathf.Lerp(position.x, Target.position.x + Offset.x, Damping.x * Kronos.GetFixedDeltaTime(TimeChannel)) : position.x;
				position.y = Axes.Contains(Axes.Y) ? Damping.y >= 100 ? Target.position.y + Offset.y : Mathf.Lerp(position.y, Target.position.y + Offset.y, Damping.y * Kronos.GetFixedDeltaTime(TimeChannel)) : position.y;
				position.z = Axes.Contains(Axes.Z) ? Damping.z >= 100 ? Target.position.z + Offset.z : Mathf.Lerp(position.z, Target.position.z + Offset.z, Damping.z * Kronos.GetFixedDeltaTime(TimeChannel)) : position.z;

				transform.position = position;
			}

			if (Mode.Contains(TransformModes.Rotation))
			{
				Vector3 eulerAngles = transform.eulerAngles;

				eulerAngles.x = Axes.Contains(Axes.X) ? Damping.x >= 100 ? Target.eulerAngles.x + Offset.x : Mathf.Lerp(eulerAngles.x, Target.eulerAngles.x + Offset.x, Damping.x * Kronos.GetFixedDeltaTime(TimeChannel)) : eulerAngles.x;
				eulerAngles.y = Axes.Contains(Axes.Y) ? Damping.y >= 100 ? Target.eulerAngles.y + Offset.y : Mathf.Lerp(eulerAngles.y, Target.eulerAngles.y + Offset.y, Damping.y * Kronos.GetFixedDeltaTime(TimeChannel)) : eulerAngles.y;
				eulerAngles.z = Axes.Contains(Axes.Z) ? Damping.z >= 100 ? Target.eulerAngles.z + Offset.z : Mathf.Lerp(eulerAngles.z, Target.eulerAngles.z + Offset.z, Damping.z * Kronos.GetFixedDeltaTime(TimeChannel)) : eulerAngles.z;

				transform.eulerAngles = eulerAngles;
			}

			if (Mode.Contains(TransformModes.Scale))
			{
				Vector3 scale = transform.lossyScale;

				scale.x = Axes.Contains(Axes.X) ? Damping.x >= 100 ? Target.lossyScale.x + Offset.x : Mathf.Lerp(scale.x, Target.lossyScale.x + Offset.x, Damping.x * Kronos.GetFixedDeltaTime(TimeChannel)) : scale.x;
				scale.y = Axes.Contains(Axes.Y) ? Damping.y >= 100 ? Target.lossyScale.y + Offset.y : Mathf.Lerp(scale.y, Target.lossyScale.y + Offset.y, Damping.y * Kronos.GetFixedDeltaTime(TimeChannel)) : scale.y;
				scale.z = Axes.Contains(Axes.Z) ? Damping.z >= 100 ? Target.lossyScale.z + Offset.z : Mathf.Lerp(scale.z, Target.lossyScale.z + Offset.z, Damping.z * Kronos.GetFixedDeltaTime(TimeChannel)) : scale.z;

				transform.SetScale(scale);
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
