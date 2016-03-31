using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;

namespace Pseudo.Internal
{
	public class SchemaCanvasWindow : PEditorWindow<SchemaCanvasWindow>
	{
		// Colors
		Color gridLineColor = new Color(0f, 0f, 0f, 0.1f);
		Color backgroundColor = new Color(0.16f, 0.16f, 0.16f, 0.65f);
		Color selectionColor = new Color(0.5f, 0.75f, 0.65f, 0.5f);

		float zoom = 1f;
		float gridLineWidth = 12f;
		Texture2D backgroundTexture;
		Vector2 scrollPosition;
		Vector2 scrollView = new Vector2(5000f, 5000f);
		Vector2 selectionStart;
		Rect selectionRect;
		bool isDraggingNodes;
		bool isDraggingCanvas;
		bool isSelecting;
		bool isOpeningContextMenu;

		Schema schema;

		const int leftMouseButton = 0;
		const int rightMouseButton = 1;
		const int middleMouseButton = 2;

		[MenuItem("Pseudo/Schema Editor")]
		static void Create()
		{
			CreateWindow("Schema Editor", new Vector2(800, 800), typeof(SchemaInspectorWindow));
		}

		void OnGUI()
		{
			if (schema == null)
				DrawInvalidCanvas();
			else
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.BeginArea(new Rect(0f, 0f, position.width - 250f, position.height));

				DrawCanvas();

				GUILayout.EndArea();

				BeginWindows();

				//GUILayout.Window(0, new Rect(position.width - 200f, 0f, 200f, position.height), id => { }, "Inpector", GUILayout.Width(200f), GUILayout.Height(position.height));
				GUI.Window(0, new Rect(position.width - 250f, 0f, 250f, position.height), id => { }, "Inpector");

				EndWindows();

				//GUILayout.Box("", GUILayout.Width(200f), GUILayout.Height(position.height));
				DrawInspector();

				EditorGUILayout.EndHorizontal();
			}
		}

		public override void OnSelectionChange()
		{
			base.OnSelectionChange();

			if (Selection.activeObject is Schema)
				schema = (Schema)Selection.activeObject;

			Repaint();
		}

		// Draw
		void DrawInvalidCanvas()
		{
			var style = new GUIStyle("boldLabel")
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				fontSize = 42
			};

			GUI.Label(new Rect(position) { position = Vector2.zero }, "Select A Schema", style);
		}

		void DrawCanvas()
		{
			var verticalScrollbar = GUI.skin.verticalScrollbar;
			var horizontalScrollbar = GUI.skin.horizontalScrollbar;
			GUI.skin.verticalScrollbar = GUIStyle.none;
			GUI.skin.horizontalScrollbar = GUIStyle.none;
			scrollPosition = GUI.BeginScrollView(new Rect(0, 0, (1f / zoom) * position.width, (1f / zoom) * position.height), scrollPosition, new Rect(0, 0, scrollView.x, scrollView.y), false, false);

			DrawCanvasBackground();
			DrawContent();
			UpdateMouseEvents();

			GUI.EndScrollView();
			GUI.skin.verticalScrollbar = verticalScrollbar;
			GUI.skin.horizontalScrollbar = horizontalScrollbar;
		}

		void DrawCanvasBackground()
		{
			if (Event.current.type != EventType.Repaint)
				return;

			DrawCanvasColor();
			DrawGridLines(gridLineWidth, gridLineColor);
			DrawGridLines(gridLineWidth * 10f, gridLineColor.Mult(2f, Channels.A));
		}

		void DrawCanvasColor()
		{
			if (backgroundTexture == null)
			{
				backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
				backgroundTexture.SetPixel(0, 0, backgroundColor);
				backgroundTexture.Apply();
				backgroundTexture.hideFlags = HideFlags.HideAndDontSave;
			}

			GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundTexture, ScaleMode.StretchToFill);
		}

		void DrawGridLines(float gridSize, Color gridColor)
		{
			Handles.color = gridColor;
			float maxX = position.width + scrollPosition.x; //float maxX = scaledPosition.width + scrollPosition.x; 
			float maxY = position.height + scrollPosition.y; //float maxY = scaledPosition.height + scrollPosition.y; 
			Handles.DrawLine(new Vector2(0, 1), new Vector2(maxX, 1));

			for (float x = 0; x < maxX; x += gridSize)
				Handles.DrawLine(new Vector2(x, 0), new Vector2(x, maxY));

			for (float y = 0; y < maxY; y += gridSize)
				Handles.DrawLine(new Vector2(0, y), new Vector2(maxX, y));
		}

		void DrawContent()
		{
			DrawNodes();
			DrawSelection();
		}

		void DrawNodes()
		{
			foreach (var node in schema.GetNodes())
				node.Draw();

			//BeginWindows();

			//var nodes = schema.GetNodes();

			//for (int i = 0; i < nodes.Length; i++)
			//{
			//	var node = nodes[i];

			//	node.Rect = GUI.Window(i, node.Rect, node.Draw, node.Name.ToGUIContent());
			//}

			//EndWindows();
		}

		void DrawSelection()
		{
			if (isSelecting)
			{
				var originalColor = GUI.color;
				GUI.color = selectionColor;
				GUI.Box(selectionRect, "");
				GUI.color = originalColor;
			}
		}

		void DrawInspector()
		{

		}

		// Events
		void UpdateMouseEvents()
		{
			wantsMouseMove = true;
			NodeBase node;

			switch (Event.current.type)
			{
				case EventType.MouseDown:
					isOpeningContextMenu = IsOpeningContextMenu();
					break;
				case EventType.MouseUp:
					if (isDraggingNodes)
						EndDragNodes();
					else if (isDraggingCanvas)
						EndDragCanvas();
					else if (isSelecting)
						EndSelection();
					else if (isOpeningContextMenu)
						OpenContextMenu();
					else if (IsOverNode(out node))
					{
						ClearSelection();
						node.IsSelected = true;
					}
					else
						ClearSelection();

					isDraggingNodes = false;
					isDraggingCanvas = false;
					isSelecting = false;
					isOpeningContextMenu = false;
					break;
				case EventType.MouseDrag:
					if (isDraggingNodes)
						UpdateDragNodes();
					else if (isDraggingCanvas)
						UpdateDragCanvas();
					else if (isSelecting)
						UpdateSelection();
					else if (isDraggingNodes = IsOverNode(out node) && IsSelecting())
					{
						if (!node.IsSelected)
						{
							ClearSelection();
							node.IsSelected = true;
						}

						BeginDragNodes();
					}
					else if (isDraggingCanvas = IsDraggingCanvas())
						BeginDragCanvas();
					else if (isSelecting = IsSelecting())
						BeginSelection();
					break;
				case EventType.MouseMove:
					isDraggingCanvas &= IsDraggingCanvas();
					isDraggingNodes &= IsSelecting();
					isSelecting &= IsSelecting();
					break;
			}

			if (isDraggingCanvas)
				EditorGUIUtility.AddCursorRect(new Rect(Vector2.zero, position.size), MouseCursor.Pan);

			if (Event.current.isMouse)
				Event.current.Use();
		}

		// Drag Nodes
		void BeginDragNodes()
		{
			var nodes = schema.GetNodes().Where(n => n.IsSelected).ToArray();
			var leftNode = nodes.FindSmallest((n1, n2) => n1.Rect.xMin.CompareTo(n2.Rect.xMin));
			var rightNode = nodes.FindBiggest((n1, n2) => n1.Rect.xMax.CompareTo(n2.Rect.xMax));
			var bottomNode = nodes.FindSmallest((n1, n2) => n1.Rect.yMin.CompareTo(n2.Rect.yMin));
			var topNode = nodes.FindBiggest((n1, n2) => n1.Rect.yMax.CompareTo(n2.Rect.yMax));

			isDraggingNodes = true;
			selectionRect = new Rect
			{
				xMin = leftNode == null ? Event.current.mousePosition.x : leftNode.Rect.xMin,
				xMax = rightNode == null ? Event.current.mousePosition.x : rightNode.Rect.xMax,
				yMin = bottomNode == null ? Event.current.mousePosition.y : bottomNode.Rect.yMin,
				yMax = topNode == null ? Event.current.mousePosition.y : topNode.Rect.yMax,
			};
		}

		void UpdateDragNodes()
		{
			if (!isDraggingNodes)
				return;

			var rect = new Rect(selectionRect.position + Event.current.delta, selectionRect.size);
			var clampedRect = rect.Clamp(new Rect(Vector2.zero, scrollView));
			var delta = clampedRect.position - selectionRect.position;
			selectionRect.position += delta;

			foreach (var node in schema.GetNodes())
			{
				if (node.IsSelected)
					node.Position += delta;
			}
		}

		void EndDragNodes()
		{
			isDraggingNodes = false;
		}

		bool IsOverNode(out NodeBase node)
		{
			node = Array.Find(schema.GetNodes(), n => n.Rect.Contains(Event.current.mousePosition));

			return node != null;
		}

		// Drag Canvas
		void BeginDragCanvas()
		{
			isDraggingCanvas = true;
		}

		void UpdateDragCanvas()
		{
			if (!isDraggingCanvas)
				return;

			scrollPosition -= Event.current.delta;
			scrollPosition.x = Mathf.Max(scrollPosition.x, 0);
			scrollPosition.y = Mathf.Max(scrollPosition.y, 0);
		}

		void EndDragCanvas()
		{
			isDraggingCanvas = false;
		}

		bool IsDraggingCanvas()
		{
			return Event.current.button == middleMouseButton || Event.current.button == rightMouseButton;
		}

		// Selection
		void BeginSelection()
		{
			isSelecting = true;
			selectionStart = Event.current.mousePosition - scrollPosition;
			selectionRect = new Rect();
			ClearSelection();
		}

		void UpdateSelection()
		{
			var currentPosition = Event.current.mousePosition - scrollPosition;

			selectionRect = new Rect
			{
				xMin = Mathf.Min(currentPosition.x, selectionStart.x),
				xMax = Mathf.Max(currentPosition.x, selectionStart.x),
				yMin = Mathf.Min(currentPosition.y, selectionStart.y),
				yMax = Mathf.Max(currentPosition.y, selectionStart.y),
			};
		}

		void EndSelection()
		{
			isSelecting = false;

			foreach (var node in schema.GetNodes())
			{
				if (selectionRect.Overlaps(node.Rect))
					node.IsSelected = true;
			}
		}

		bool IsSelecting()
		{
			return Event.current.button == leftMouseButton;
		}

		bool IsTogglingSelection()
		{
			return Event.current.button == leftMouseButton && Event.current.control;
		}

		void ClearSelection()
		{
			foreach (var node in schema.GetNodes())
				node.IsSelected = false;
		}

		// Context Menu
		void OpenContextMenu()
		{

		}

		bool IsOpeningContextMenu()
		{
			return Event.current.button == rightMouseButton;
		}
	}
}
