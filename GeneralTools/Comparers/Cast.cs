using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Runtime.InteropServices;

namespace Pseudo.Internal
{
	public static class Cast<TIn, TOut>
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Caster
		{
			[FieldOffset(0)]
			public TIn Input;
			[FieldOffset(0)]
			public TOut Output;
		}

		public static TOut To(TIn value)
		{
			return new Caster { Input = value }.Output;
		}
	}
}
