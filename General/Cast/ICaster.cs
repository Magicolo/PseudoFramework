using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ICaster<in TIn, out TOut>
	{
		TOut Cast(TIn value);
	}
}
