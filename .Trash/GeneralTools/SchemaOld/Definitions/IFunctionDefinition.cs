using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal.Schema;

namespace Pseudo
{
	public interface IFunctionDefinition
	{
		string Name { get; }

		IFunctionNode CreateNode();
	}
}