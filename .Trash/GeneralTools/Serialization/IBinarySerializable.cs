using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo
{
	public interface IBinarySerializable
	{
		void Serialize(BinaryWriter writer);
		void Deserialize(BinaryReader reader);
	}
}
