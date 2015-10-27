using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class zTest : PMonoBehaviour
{
	public MultipleRaycastSettings RaySettings;
	public MultipleRaycast2DSettings RaySettings2D;

	[Button]
	public bool test;
	void Test()
	{

	}

	void OnDrawGizmos()
	{
		RaySettings.Cast(transform.position, transform.right, transform.up);
		RaySettings2D.Cast(transform);
	}
}