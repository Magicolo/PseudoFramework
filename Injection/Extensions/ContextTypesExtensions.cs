using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public static class ContextTypesExtensions
	{
		public static bool Contains(this ContextTypes contextType, ContextTypes other)
		{
			return (contextType & other) == other;
		}
	}
}
