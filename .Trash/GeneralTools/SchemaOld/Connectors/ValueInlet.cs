using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ValueInlet<TValue> : ValueInlet
	{
		ValueOutlet<TValue> connection;

		public TValue PullValue()
		{
			return connection == null ? default(TValue) : connection.PullValue();
		}

		public override void Connect(ValueOutlet outlet)
		{
			connection = outlet as ValueOutlet<TValue>;
		}
	}

	public abstract class ValueInlet
	{
		public abstract void Connect(ValueOutlet outlet);
	}
}
