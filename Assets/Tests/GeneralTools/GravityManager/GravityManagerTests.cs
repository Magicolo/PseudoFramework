using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class GravityManagerTests : PhysicalEntity2D
	{
		[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 2f)]
		float playerTimeScale = 1f;
		float PlayerTimeScale
		{
			get { return playerTimeScale; }
			set
			{
				playerTimeScale = value;
				TimeManager.Player.TimeScale = playerTimeScale;
			}
		}

		[SerializeField, PropertyField]
		float playerGravityScale = 1f;
		float PlayerGravityScale
		{
			get { return playerGravityScale; }
			set
			{
				playerGravityScale = value;
				GravityManager.Player.GravityScale = playerGravityScale;
			}
		}

		[SerializeField, PropertyField]
		Vector3 playerGravityRotation;
		Vector3 PlayerGravityRotation
		{
			get { return playerGravityRotation; }
			set
			{
				playerGravityRotation = value;
				GravityManager.Player.Rotation = playerGravityRotation;
			}
		}

		void FixedUpdate()
		{
			UpdateGravity();
		}
	}
}