using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class ThisNode : ReturnNodeBase
	{
		public void Initialize(Type returnType, Schema schema)
		{
			Initialize("this", returnType, schema);
		}

		public override void Write(SchemaWriter writer)
		{
			writer.Append("this");
		}
	}
}
