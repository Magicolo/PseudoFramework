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

	[Button]
	public bool test;
	void Test()
	{
	}

	void OnDrawGizmos()
	{
		if (test)
		{
			Curve1 = PRandom.DistributionToCurve(ProbabilityDistributions.Uniform, 5000);
			Curve2 = PRandom.DistributionToCurve(ProbabilityDistributions.Proportional, 5000);
			Curve3 = PRandom.DistributionToCurve(ProbabilityDistributions.Normal, 5000);
		}

		RaySettings.Cast(CachedTransform.position, CachedTransform.right, CachedTransform.up);
		RaySettings2D.Cast(CachedTransform);
	}

	public void Copy(zTest reference)
	{
		throw new NotImplementedException();
	}
}