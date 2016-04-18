using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface ITypeAnalyzer
	{
		ITypeInfo Analyze(Type type);
	}
}
