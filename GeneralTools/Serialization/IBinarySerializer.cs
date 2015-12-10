using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;

namespace Pseudo.Internal
{
	public interface IBinarySerializer
	{
		ushort TypeIdentifier { get; }

		void Serialize(BinaryWriter writer, object value);
		object Deserialize(BinaryReader reader);
	}
}