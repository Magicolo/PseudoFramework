using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Internal.Audio
{
	public class AudioSpatializer : IPoolable, ICopyable<AudioSpatializer>
	{
		public enum SpatializeModes
		{
			None,
			Static,
			Dynamic
		}

		Vector3 _position;
		Transform _follow;
		Func<Vector3> _getPosition;
		SpatializeModes _spatializeMode;

		readonly List<Transform> _sources = new List<Transform>();

		public static AudioSpatializer Default = new AudioSpatializer();

		/// <summary>
		/// The behaviour of the spatialization.
		/// </summary>
		public SpatializeModes SpatializeMode { get { return _spatializeMode; } }
		/// <summary>
		/// The current position of the AudioSpatializer
		/// </summary>
		public Vector3 Position { get { return _position; } }

		/// <summary>
		/// Initializes the AudioSpatializer with a static position.
		/// </summary>
		/// <param name="position">The static position.</param>
		public void Initialize(Vector3 position)
		{
			_position = position;
			_spatializeMode = SpatializeModes.Static;
		}

		/// <summary>
		/// Initializes the AudioSpatializer with a dynamic Transform.
		/// </summary>
		/// <param name="follow">The dynamic Transform.</param>
		public void Initialize(Transform follow)
		{
			_follow = follow;
			_position = _follow.position;
			_spatializeMode = SpatializeModes.Dynamic;
		}

		/// <summary>
		/// Initializes the AudioSpatializer with a dynamic delegate.
		/// </summary>
		/// <param name="getPosition">The dynamic delegate.</param>
		public void Initialize(Func<Vector3> getPosition)
		{
			_getPosition = getPosition;
			_position = getPosition();
			_spatializeMode = SpatializeModes.Dynamic;
		}

		/// <summary>
		/// Updates the position of the AudioSpatializer.
		/// </summary>
		public void Spatialize()
		{
			if (_spatializeMode == SpatializeModes.Dynamic)
			{
				if (_getPosition != null)
					_position = _getPosition();
				else if (_follow != null)
					_position = _follow.position;
				else
					_spatializeMode = SpatializeModes.Static;

				for (int i = 0; i < _sources.Count; i++)
					_sources[i].position = _position;
			}
		}

		/// <summary>
		/// Adds a Transform to be spatialized by the AudioSpatializer.
		/// </summary>
		/// <param name="source">The Transform to be added.</param>
		public void AddSource(Transform source)
		{
			_sources.Add(source);
			source.position = _position;
		}

		/// <summary>
		/// Removes a Transform from the AudioSpatializer's list.
		/// </summary>
		/// <param name="source">The Transform to remove.</param>
		public void RemoveSource(Transform source)
		{
			_sources.Remove(source);
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
			_sources.Clear();
		}

		/// <summary>
		/// Copies another AudioSpatializer.
		/// </summary>
		/// <param name="reference"> The AudioSpatializer to copy. </param>
		public void Copy(AudioSpatializer reference)
		{
			_position = reference._position;
			_follow = reference._follow;
			_getPosition = reference._getPosition;
			_spatializeMode = reference._spatializeMode;
		}
	}
}