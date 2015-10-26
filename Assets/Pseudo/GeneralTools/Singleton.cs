using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

public abstract class Singleton<T> : PMonoBehaviour where T : Singleton<T>
{
	protected static T _instance;
	public static T Instance { get { return _instance; } }

	public static T Find()
	{
		_instance = FindObjectOfType<T>();

		return _instance;
	}

	protected virtual void Awake()
	{
		if (_instance == null)
			_instance = this as T;
	}
}
