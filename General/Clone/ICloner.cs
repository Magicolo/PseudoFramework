using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ICloner<T> : ICloner
	{
		T Clone(T source);
	}

	public interface ICloner
	{
		object Clone(object source);
	}
}
