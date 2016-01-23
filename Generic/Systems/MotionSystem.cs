using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class MotionSystem : SystemBase, IUpdateable
	{
		IEntityGroup entities;

		public override void OnInitialize()
		{
			base.OnInitialize();

			entities = EntityManager.Entities.Filter(new[]
			{
				typeof(TimeComponent),
				typeof(MotionComponent)
			});
		}

		public void Update()
		{
			for (int i = 0; i < entities.Count; i++)
			{
				var entity = entities[i];
				var time = entity.GetComponent<TimeComponent>();
				var motion = entity.GetComponent<MotionComponent>();
				var transform = motion.Agent.CachedTransform;
				var velocity = Vector3.zero;

				UpdateModifiers(entity, ref velocity, transform.position, time.DeltaTime);

				motion.Velocity = motion.Velocity.Lerp(velocity, time.DeltaTime * motion.Acceleration, Axes.XYZ);
				motion.Velocity = motion.Velocity.ClampMagnitude(motion.Speed.Min * time.DeltaTime, motion.Speed.Max * time.DeltaTime);
				transform.Translate(motion.Velocity);
			}
		}

		void UpdateModifiers(IEntity entity, ref Vector3 velocity, Vector3 position, float deltaTime)
		{
			var modifiers = entity.GetComponents<MotionModifierComponentBase>();

			for (int i = 0; i < modifiers.Count; i++)
			{
				var modifier = modifiers[i];

				if (modifier is SeekMotionModifierComponent)
					UpdateModifier((SeekMotionModifierComponent)modifier, ref velocity, position, deltaTime);
				else if (modifier is FearMotionModifierComponent)
					UpdateModifier((FearMotionModifierComponent)modifier, ref velocity, position, deltaTime);
			}
		}

		void UpdateModifier(SeekMotionModifierComponent modifier, ref Vector3 velocity, Vector3 position, float deltaTime)
		{
			if (!modifier.Target.HasTarget)
				return;

			var difference = modifier.Target.Target - position;
			float distance = difference.magnitude;

			if (distance > modifier.PerceptionDistance || distance < modifier.StopDistance)
				return;

			var direction = difference.normalized;
			float strength = modifier.Strength;

			if (distance <= modifier.SlowDistance)
				strength *= (distance - modifier.StopDistance) / (modifier.SlowDistance - modifier.StopDistance);
			else if (distance <= modifier.StopDistance)
				strength = 0f;

			velocity += direction * strength * deltaTime;
		}

		void UpdateModifier(FearMotionModifierComponent modifier, ref Vector3 velocity, Vector3 position, float deltaTime)
		{
			if (!modifier.Target.HasTarget)
				return;

			var difference = position - modifier.Target.Target;
			float distance = difference.magnitude;

			if (distance > modifier.PerceptionDistance)
				return;

			var direction = difference.normalized;
			float strength = modifier.Strength * (1 - distance / modifier.PerceptionDistance).Pow(modifier.Ratio);

			velocity += direction * strength * deltaTime;
		}
	}
}