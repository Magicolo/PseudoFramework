using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract class ValueOutlet<TValue> : ValueOutlet
	{
		public abstract TValue PullValue();
	}

	public abstract class ValueOutlet { }
}
