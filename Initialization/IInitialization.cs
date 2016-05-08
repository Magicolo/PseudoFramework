using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization
{
	public interface IInitialization<T> : IInitialization
	{
		void Initialize(ref T instance);
	}

	public interface IInitialization
	{
		void Initialize(ref object instance);
	}
}
