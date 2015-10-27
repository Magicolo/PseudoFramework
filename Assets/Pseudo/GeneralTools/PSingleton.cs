using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	public abstract class PSingleton<T> : PMonoBehaviour where T : PSingleton<T>
	{
		protected static T instance;
		public static T Instance { get { return instance; } }

		public static T Find()
		{
			instance = FindObjectOfType<T>();

			return instance;
		}

		protected virtual void Awake()
		{
			if (instance == null)
				instance = this as T;
		}
	}
}