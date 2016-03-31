using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class PType : ISerializationCallbackReceiver
	{
		public Type Type
		{
			get
			{
				if (type == null)
					Deserialize();

				return type;
			}
			set
			{
				type = value;
				typeName = type == null ? null : type.AssemblyQualifiedName;
			}
		}

		Type type;
		[SerializeField]
		string typeName;

		public PType(Type type)
		{
			Type = type;
		}

		void Deserialize()
		{
			if (!string.IsNullOrEmpty(typeName))
				type = TypeUtility.GetType(typeName);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Deserialize();
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
