using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo
{
	[Serializable]
	public class PAssembly : ISerializationCallbackReceiver
	{
		public Assembly Assembly
		{
			get { return assembly; }
			set { SetAssembly(value); }
		}

		Assembly assembly;
		[SerializeField]
		string assemblyName;

		public PAssembly(Assembly assembly)
		{
			SetAssembly(assembly);
		}

		void SetAssembly(Assembly assembly)
		{
			Assert.IsNotNull(assembly);

			this.assembly = assembly;
			assemblyName = assembly.FullName;
		}

		public override string ToString()
		{
			return Convert.ToString(assembly);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(assemblyName))
				assembly = null;
			else
				assembly = TypeUtility.GetAssembly(assemblyName);
		}

		public static implicit operator Assembly(PAssembly assembly)
		{
			return assembly.Assembly;
		}

		public static implicit operator PAssembly(Assembly assembly)
		{
			return new PAssembly(assembly);
		}
	}
}
