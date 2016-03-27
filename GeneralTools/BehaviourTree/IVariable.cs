using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IVariable
	{
		string Name { get; }
		Type Type { get; }
		object Value { get; set; }
	}
}
