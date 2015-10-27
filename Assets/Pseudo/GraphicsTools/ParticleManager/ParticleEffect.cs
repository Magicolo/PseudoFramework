using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[Serializable]
public class ParticleEffect : IPoolable, ICopyable<ParticleEffect>
{
	public string Name;
	public ParticleSystem Prefab;

	public void OnCreate()
	{
	}

	public void OnRecycle()
	{
	}

	public void Copy(ParticleEffect reference)
	{
		Name = reference.Name;
		Prefab = reference.Prefab;
	}
}
