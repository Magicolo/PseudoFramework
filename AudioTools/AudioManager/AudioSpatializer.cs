using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Internal.Audio
{
	public class AudioSpatializer : IPoolable, ICopyable
	{
		public enum SpatializeModes
		{
			None,
			Static,
			Dynamic
		}

		Vector3 position;
		Transform follow;
		Func<Vector3> getPosition;
		SpatializeModes spatializeMode;

		readonly List<Transform> sources = new List<Transform>();

		/// <summary>
		/// The behaviour of the spatialization.
		/// </summary>
		public SpatializeModes SpatializeMode { get { return spatializeMode; } }
		/// <summary>
		/// The current position of the AudioSpatializer
		/// </summary>
		public Vector3 Position { get { return position; } }

		/// <summary>
		/// Initializes the AudioSpatializer with a static position.
		/// </summary>
		/// <param name="position">The static position.</param>
		public void Initialize(Vector3 position)
		{
			this.position = position;
			spatializeMode = SpatializeModes.Static;
		}

		/// <summary>
		/// Initializes the AudioSpatializer with a dynamic Transform.
		/// </summary>
		/// <param name="follow">The dynamic Transform.</param>
		public void Initialize(Transform follow)
		{
			this.follow = follow;
			position = this.follow.position;
			spatializeMode = SpatializeModes.Dynamic;
		}

		/// <summary>
		/// Initializes the AudioSpatializer with a dynamic delegate.
		/// </summary>
		/// <param name="getPosition">The dynamic delegate.</param>
		public void Initialize(Func<Vector3> getPosition)
		{
			this.getPosition = getPosition;
			position = getPosition();
			spatializeMode = SpatializeModes.Dynamic;
		}

		/// <summary>
		/// Updates the position of the AudioSpatializer.
		/// </summary>
		public void Spatialize()
		{
			if (spatializeMode == SpatializeModes.Dynamic)
			{
				if (getPosition != null)
					position = getPosition();
				else if (follow != null)
					position = follow.position;
				else
					spatializeMode = SpatializeModes.Static;

				for (int i = 0; i < sources.Count; i++)
					sources[i].position = position;
			}
		}

		/// <summary>
		/// Adds a Transform to be spatialized by the AudioSpatializer.
		/// </summary>
		/// <param name="source">The Transform to be added.</param>
		public void AddSource(Transform source)
		{
			sources.Add(source);
			source.position = position;
		}

		/// <summary>
		/// Removes a Transform from the AudioSpatializer's list.
		/// </summary>
		/// <param name="source">The Transform to remove.</param>
		public void RemoveSource(Transform source)
		{
			sources.Remove(source);
		}

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public void OnCreate()
		{

		}

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public void OnRecycle()
		{
			sources.Clear();
		}

		/// <summary>
		/// Copies another AudioSpatializer.
		/// </summary>
		/// <param name="reference"> The AudioSpatializer to copy. </param>
		public void Copy(object reference)
		{
			var castedReference = (AudioSpatializer)reference;
			position = castedReference.position;
			follow = castedReference.follow;
			getPosition = castedReference.getPosition;
			spatializeMode = castedReference.spatializeMode;
		}
	}
}