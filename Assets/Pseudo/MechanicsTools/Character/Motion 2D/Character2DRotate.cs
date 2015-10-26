using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character2DRotate : StateLayer
{
	public bool Invert;

	[Disable]
	public float CurrentAngle;
	[Disable]
	public float TargetAngle;
	[Disable]
	public float CurrentFacingDirection;
	[Disable]
	public float TargetFacingDirection;

	public Rigidbody2D Rigidbody { get { return Layer.Rigidbody; } }

	bool _spriteTransformCached;
	Transform _spriteTransform;
	public Transform SpriteTransform
	{
		get
		{
			_spriteTransform = _spriteTransformCached ? _spriteTransform : transform.FindChild("Sprite").GetComponent<Transform>();
			_spriteTransformCached = true;
			return _spriteTransform;
		}
	}

	new public Character2DMotion Layer { get { return ((Character2DMotion)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		TargetAngle = -Layer.Gravity.Angle + 90;

		if (Layer.HorizontalAxis > 0)
			TargetFacingDirection = Invert ? -1 : 1;
		else if (Layer.HorizontalAxis < 0)
			TargetFacingDirection = Invert ? 1 : -1;
	}
}
