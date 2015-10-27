using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DRotate : PStateLayer
{
	public bool Invert;

	[Disable]
	public float CurrentAngle;
	[Disable]
	public float TargetAngle;
	[Disable]
	public float CurrentFacingDirection;
	[Disable]
	public float TargetFacingAngle;

	public Rigidbody Rigidbody { get { return Layer.Rigidbody; } }

	bool _modelTransformCached;
	Transform _modelTransform;
	public Transform modelTransform
	{
		get
		{
			_modelTransform = _modelTransformCached ? _modelTransform : CachedTransform.FindChild("Model").GetComponent<Transform>();
			_modelTransformCached = true;
			return _modelTransform;
		}
	}

	new public Character3DMotion Layer { get { return ((Character3DMotion)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		TargetAngle = -Layer.Gravity.Angle + 90;

		if (Layer.HorizontalAxis > 0)
			TargetFacingAngle = Invert ? 180 : 0;
		else if (Layer.HorizontalAxis < 0)
			TargetFacingAngle = Invert ? 0 : 180;
	}
}
