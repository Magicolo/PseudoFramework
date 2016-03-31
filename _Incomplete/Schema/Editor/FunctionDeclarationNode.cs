using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class FunctionDeclarationNode : NodeBase
	{
		[SerializeField]
		List<FunctionNodeBase> functions = new List<FunctionNodeBase>();

		new public void Initialize(string name, Schema schema)
		{
			base.Initialize(name, schema);
		}

		public void AddFunction(FunctionNodeBase function)
		{
			functions.Add(function);
		}

		public FunctionNodeBase[] GetFunctions()
		{
			return functions.ToArray();
		}

		public override void Write(SchemaWriter writer)
		{
			writer.BeginMethodDeclaration(Name, "void");

			for (int i = 0; i < functions.Count; i++)
			{
				writer.AppendIndentation();
				functions[i].Write(writer);
				writer.AppendLine(";", false);
			}

			writer.EndMethodDeclaration();
		}

		public override bool IsValid()
		{
			return functions.All(f => f.IsValid()) && base.IsValid();
		}
	}
}
