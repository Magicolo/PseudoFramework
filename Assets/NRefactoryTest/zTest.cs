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

	public PDynamicValue DynamicValue;

	readonly CachedValue<Renderer> cachedRenderer;
	public Renderer CachedRenderer { get { return cachedRenderer; } }

	public Transform transformAwake;

	void Awake()
	{
		transformAwake = transform;
	}

	public zTest()
	{
		cachedRenderer = new CachedValue<Renderer>(GetComponent<Renderer>);
	}

	[Button]
	public bool test;
	void Test()
	{
		//PDebug.LogTest("Unity Transform Getter", () => { var position = transform.position; }, 1000000);
		//PDebug.LogTest("GetComponent", () => { var position = GetComponent<Transform>().position; }, 1000000);
		//PDebug.LogTest("Cached Transform Getter", () => { var position = CachedTransform.position; }, 1000000);
		//PDebug.LogTest("Direct Field Get", () => { var position = transformAwake.position; }, 1000000);
	}

	void OnDrawGizmos()
	{
		RaySettings.Cast(CachedTransform.position, CachedTransform.right, CachedTransform.up);
		RaySettings2D.Cast(CachedTransform);
	}
}