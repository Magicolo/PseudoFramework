using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	/// <summary>
	/// Resets the members of an instance to a reference state.
	/// </summary>
	public interface IInitializer
	{
		ITypeAnalyzer Analyzer { get; set; }

		void Initialize(object instance);
	}
}
