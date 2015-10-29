using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class zTest : PMonoBehaviour, ICopyable<zTest>
{
	public MinMax AttackSpeed;
	public MinMax[] AttackSpeeds;

	public MultipleRaycastSettings RaySettings;
	public MultipleRaycast2DSettings RaySettings2D;

	public AnimationCurve Curve1;
	public AnimationCurve Curve2;
	public AnimationCurve Curve3;

	public ParticleEffect[] SomeEffects;
	const int iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
	}

	public void Copy(zTest reference)
	{
		AttackSpeed = reference.AttackSpeed;
		CopyUtility.CopyTo(reference.AttackSpeeds, ref AttackSpeeds);
		RaySettings = reference.RaySettings;
		RaySettings2D = reference.RaySettings2D;
		Curve1 = reference.Curve1;
		Curve2 = reference.Curve2;
		Curve3 = reference.Curve3;
		CopyUtility.CopyTo(reference.SomeEffects, ref SomeEffects);
		test = reference.test;
	}
}