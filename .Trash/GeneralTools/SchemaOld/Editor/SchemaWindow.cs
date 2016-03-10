using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Schema
{
	public class SchemaWindow : PEditorWindow<SchemaWindow>
	{
		[MenuItem("Pseudo/Schema Editor")]
		public static void Create()
		{
			CreateWindow("Schema Editor", new Vector2(800, 600));
		}

		public void LoadSchema(SchemaAsset schema)
		{

		}

		void OnGUI()
		{

		}
	}
}
