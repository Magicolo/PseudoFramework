using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEditor;

namespace Pseudo
{
	public abstract class NodeBase : ScriptableObject
	{
		public string Name
		{
			get { return name; }
		}
		public Schema Schema
		{
			get { return schema; }
		}
		public Rect Rect
		{
			get { return rect; }
			set { rect = value; }
		}
		public Vector2 Position
		{
			get { return rect.position; }
			set { rect.position = value; }
		}
		public Vector2 Size
		{
			get { return rect.size; }
			set { rect.size = value; }
		}
		public bool IsSelected
		{
			get { return selected; }
			set { selected = value; }
		}

		[SerializeField]
		Schema schema;
		[SerializeField]
		Rect rect = new Rect(0f, 0f, 100f, 50f);
		bool selected;

		protected virtual void Initialize(string name, Schema schema)
		{
			this.name = name;
			this.schema = schema;
		}

		public virtual bool IsValid()
		{
			return !string.IsNullOrEmpty(Name);
		}

		public virtual void Draw()
		{
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
			var color = GUI.color;
			GUI.color = IsSelected ? Color.cyan : Color.gray;
			GUI.Box(rect, "");
			GUI.color = color;
		}

		public abstract void Write(SchemaWriter writer);

	}
}
