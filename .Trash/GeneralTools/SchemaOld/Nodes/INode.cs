using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface INode
	{
		INode ConnectValue(INode node, int outletIndex, int inletIndex);
		ValueInlet GetValueInlet(int index);
		ValueOutlet GetValueOutlet(int index);
	}
}
