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
		short TypeIdentifier { get; }

		void Serialie(BinaryWriter writer, object instance);
		object Deserialize(BinaryReader reader);
	}
}