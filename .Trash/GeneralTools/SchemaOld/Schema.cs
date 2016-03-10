using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class Schema
	{
		[Serializable]
		public class NodeData
		{
			static int identifierCounter;

			public INode Node;
			public Rect Rect;
			public int Identifier = ++identifierCounter;
			public readonly List<int> Connections = new List<int>();
		}

		readonly List<NodeData> nodes = new List<NodeData>();

		public static string Serialize(Schema schema)
		{
			if (schema == null)
				return null;

			return null;
		}

		public static Schema Deserialize(string data)
		{
			if (string.IsNullOrEmpty(data))
				return null;

			return null;
		}
	}
}
