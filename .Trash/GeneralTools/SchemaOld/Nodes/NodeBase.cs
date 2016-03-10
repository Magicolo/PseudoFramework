using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public abstract class NodeBase : INode
	{
		public virtual INode ConnectValue(INode node, int outletIndex, int inletIndex)
		{
			var inlet = node.GetValueInlet(inletIndex);

			if (inlet != null)
				inlet.Connect(GetValueOutlet(outletIndex));

			return node;
		}

		public abstract ValueInlet GetValueInlet(int index);
		public abstract ValueOutlet GetValueOutlet(int index);
	}
}
