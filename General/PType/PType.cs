using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo
{
	[Serializable]
	public class PType : ISerializationCallbackReceiver
	{
		public Type Type
		{
			get { return type; }
			set { SetType(value); }
		}

		Type type;
		[SerializeField]
		string typeName;

		public PType(Type type)
		{
			SetType(type);
		}

		void SetType(Type type)
		{
			Assert.IsNotNull(type);

			this.type = type;
			typeName = type.AssemblyQualifiedName;
		}

		public override string ToString()
		{
			return Convert.ToString(type);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(typeName))
				type = null;
			else
				type = TypeUtility.GetType(typeName);
		}

		public static implicit operator Type(PType type)
		{
			return type.Type;
		}

		public static implicit operator PType(Type type)
		{
			return new PType(type);
		}
	}
}
