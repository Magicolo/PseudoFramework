using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Diagnostics;

public class zTest : PMonoBehaviour
{
	CachedValue<Rigidbody> cachedRigidbody;
	public Rigidbody Rigidbody { get { return cachedRigidbody.Value; } }

	bool rigidbodyIsCached;
	Rigidbody rigidbodyCached;
	Rigidbody CachedRigidbody
	{
		get
		{
			if (!rigidbodyIsCached)
			{
				rigidbodyIsCached = true;
				rigidbodyCached = GetComponent<Rigidbody>();
			}

			return rigidbodyCached;
		}
	}

	Rigidbody awakeRigidbody;
	Rigidbody AwakeRigidbody { get { return awakeRigidbody; } }

	public zTest()
	{
		cachedRigidbody = new CachedValue<Rigidbody>(GetComponent<Rigidbody>);
	}

	void Awake()
	{
		awakeRigidbody = GetComponent<Rigidbody>();
	}

	[Button]
	public bool test;
	void Test()
	{
		var t = ((MonoBehaviour)this).transform;
		//rigidbodyIsCached = false;
		//cachedRigidbody.Reset();

		//Logger.LogTest("Bool Caching", () => { var r = CachedRigidbody; }, 1000000);
		//Logger.LogTest("Cached Value Caching", () => { var r = cachedRigidbody.Value; }, 1000000);
		//Logger.LogTest("Cached Value Caching + Property", () => { var r = Rigidbody; }, 1000000);
		//Logger.LogTest("Awake Caching", () => { var r = AwakeRigidbody; }, 1000000);
	}
}

public class CachedValue<T>
{
	Func<T> getValue;
	bool valueCached;
	T value;

	public T Value
	{
		get
		{
			if (!valueCached)
			{
				valueCached = Application.isPlaying;
				value = getValue();
			}

			return value;
		}
	}

	public CachedValue(Func<T> getValue)
	{
		this.getValue = getValue;
	}

	public void Reset()
	{
		valueCached = false;
	}

	public static implicit operator T(CachedValue<T> cachedValue)
	{
		return cachedValue.Value;
	}
}
