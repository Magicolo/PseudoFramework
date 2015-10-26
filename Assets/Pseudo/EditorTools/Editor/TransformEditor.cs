using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo.Internal.Editor
{
	[CustomEditor(typeof(Transform), true), CanEditMultipleObjects]
	public class TransformEditor : CustomEditorBase
	{
		Transform _transform;
		Vector3 _pLocalPosition;
		Vector3 _pLocalRotation;
		Vector3 _pLocalScale;
		bool _snap;
		bool _grid;

		// Snap Settings
		float _moveX;
		float _moveY;
		float _moveZ;
		float _rotation;
		float _scale;
		int _gridSize;
		bool _showCubes;
		bool _showLines;

		public override void OnEnable()
		{
			base.OnEnable();

			_transform = (Transform)target;
			SnapSettings.CleanUp();
		}

		public override void OnInspectorGUI()
		{
			GetSnapSettings();

			Begin(false);

			if (!deleteBreak) ShowUtilityButtons();
			if (!deleteBreak) ShowVectors();
			if (!deleteBreak) ShowGrid();

			End(false);
		}

		void OnSceneGUI()
		{
			GetSnapSettings();
			ShowGrid();

			if (_snap && !Application.isPlaying)
				RoundSelectedTransforms();
		}

		void GetSnapSettings()
		{
			_moveX = SnapSettings.GetValue<float>("moveX");
			_moveY = SnapSettings.GetValue<float>("moveY");
			_moveZ = SnapSettings.GetValue<float>("moveZ");
			_rotation = SnapSettings.GetValue<float>("rotation");
			_scale = SnapSettings.GetValue<float>("scale");
			_gridSize = SnapSettings.GetValue<int>("gridSize");
			_showCubes = SnapSettings.GetValue<bool>("showCubes");
			_showLines = SnapSettings.GetValue<bool>("showLines");
		}

		void RoundSelectedTransforms()
		{
			foreach (Transform t in Selection.transforms)
			{
				if (_pLocalPosition != t.localPosition)
				{
					Vector3 parentScale = t.parent == null ? Vector3.one : t.parent.lossyScale;

					t.localPosition = t.localPosition.Round(_moveX / parentScale.x, Axes.X);
					t.localPosition = t.localPosition.Round(_moveY / parentScale.y, Axes.Y);
					t.localPosition = t.localPosition.Round(_moveZ / parentScale.z, Axes.Z);
					_pLocalPosition = t.localPosition;
				}
				if (_pLocalRotation != t.localEulerAngles)
				{
					t.localEulerAngles = t.localEulerAngles.Round(_rotation);
					_pLocalRotation = t.localEulerAngles;
				}
				if (_pLocalScale != t.localScale)
				{
					t.localScale = t.localScale.Round(_scale);
					_pLocalScale = t.localScale;
				}
			}
		}

		void ShowUtilityButtons()
		{
			EditorGUILayout.BeginHorizontal();

			// Reset Button
			if (GUILayout.Button(new GUIContent(". Reset  .", "Resets the transform to it's original state."), EditorStyles.miniButton, GUILayout.Width("Reset".GetWidth(EditorStyles.miniFont) + 12)))
			{
				Undo.RegisterCompleteObjectUndo(targets, "Transform Reset");

				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					selectedTransform.Reset();
					SnapSettings.DeleteKey("Toggle" + "Snap" + selectedTransform.GetInstanceID());
					SnapSettings.DeleteKey("Toggle" + "Grid" + selectedTransform.GetInstanceID());
					EditorUtility.SetDirty(selectedTransform);
					deleteBreak = true;
				}

				return;
			}

			Separator(false);

			// Snap Button
			_snap = ShowToggleButton("Snap", "Toggles the snapping of the transform to the grid. See Magicolo's Tools/Snap Settings to change the snap settings.", EditorStyles.miniButtonLeft);
			EditorGUI.BeginChangeCheck();
			_grid = ShowToggleButton("Grid", "Toggles the display of the grid. See Magicolo's Tools/Snap Settings to change the grid display settings.", EditorStyles.miniButtonRight);
			if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();

			// Add Button
			if (GUILayout.Button(new GUIContent(". Add  .", "Adds a child to the transform."), EditorStyles.miniButtonLeft, GUILayout.Width("Add".GetWidth(EditorStyles.miniFont) + 12)))
			{
				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					Undo.RegisterCreatedObjectUndo(selectedTransform.AddChild("New Child").gameObject, "New Child");
					EditorUtility.SetDirty(selectedTransform);
				}
			}

			// Sort Button
			if (GUILayout.Button(new GUIContent(". Sort  .", "Sorts the immediate children of the transform alphabetically."), EditorStyles.miniButtonRight, GUILayout.Width("Sort".GetWidth(EditorStyles.miniFont) + 12)))
			{
				Undo.RegisterCompleteObjectUndo(targets, "Transform Sort");

				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					selectedTransform.SortChildren();
					EditorUtility.SetDirty(selectedTransform);
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		void ShowVectors()
		{
			const float sensibility = 0.15F;

			Vector3 parentScale = Vector3.one;

			if (_transform.parent != null)
				parentScale = _transform.parent.lossyScale;

			ShowVectorWithButton(serializedObject.FindProperty("m_LocalPosition"), ". P  .", "Resets the transform's local position to it's initial state.", Vector3.zero, new Vector3(_moveX / parentScale.x, _moveY / parentScale.y, _moveZ / parentScale.z), sensibility);
			ShowQuaternionWithButton(serializedObject.FindProperty("m_LocalRotation"), ". R  .", "Resets the transform's local rotation to it's initial state.", Quaternion.identity, new Vector3(_rotation, _rotation, _rotation), sensibility);
			ShowVectorWithButton(serializedObject.FindProperty("m_LocalScale"), ". S  .", "Resets the transform's local scale to it's initial state.", Vector3.one, new Vector3(_scale, _scale, _scale), sensibility);
		}

		void ShowVectorWithButton(SerializedProperty vectorProperty, string buttonLabel, string tooltip, Vector3 resetValue, Vector3 roundValue, float sensibility)
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 15;

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(new GUIContent(buttonLabel, tooltip), EditorStyles.miniButton, GUILayout.Width(20)))
			{
				vectorProperty.vector3Value = resetValue;
				serializedObject.ApplyModifiedProperties();
				deleteBreak = true;
				return;
			}

			Vector3 previousValue = vectorProperty.vector3Value;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(vectorProperty.FindPropertyRelative("x"));
			if (EditorGUI.EndChangeCheck())
			{
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						vectorProperty.FindPropertyRelative("x").floatValue = (previousValue.x + Event.current.delta.x * roundValue.x * sensibility);

					vectorProperty.FindPropertyRelative("x").floatValue = vectorProperty.FindPropertyRelative("x").floatValue.Round(roundValue.x);
					serializedObject.ApplyModifiedProperties();
				}
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(vectorProperty.FindPropertyRelative("y"));
			if (EditorGUI.EndChangeCheck())
			{
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						vectorProperty.FindPropertyRelative("y").floatValue = (previousValue.y + Event.current.delta.x * roundValue.y * sensibility);

					vectorProperty.FindPropertyRelative("y").floatValue = vectorProperty.FindPropertyRelative("y").floatValue.Round(roundValue.y);
					serializedObject.ApplyModifiedProperties();
				}
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(vectorProperty.FindPropertyRelative("z"));
			if (EditorGUI.EndChangeCheck())
			{
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						vectorProperty.FindPropertyRelative("z").floatValue = (previousValue.z + Event.current.delta.x * roundValue.z * sensibility);

					vectorProperty.FindPropertyRelative("z").floatValue = vectorProperty.FindPropertyRelative("z").floatValue.Round(roundValue.z);
					serializedObject.ApplyModifiedProperties();
				}
			}

			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.labelWidth = labelWidth;
		}

		void ShowQuaternionWithButton(SerializedProperty quaternionProperty, string buttonLabel, string tooltip, Quaternion resetValue, Vector3 roundValue, float sensibility)
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 15;
			Rect rect = EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(new GUIContent(buttonLabel, tooltip), EditorStyles.miniButton, GUILayout.Width(20)))
			{
				quaternionProperty.quaternionValue = resetValue;
				serializedObject.ApplyModifiedProperties();
				deleteBreak = true;
				return;
			}

			Vector3 localEulerAngles = _transform.localEulerAngles;
			EditorGUI.BeginProperty(rect, GUIContent.none, quaternionProperty);
			EditorGUI.BeginChangeCheck();

			localEulerAngles.x = EditorGUILayout.FloatField("X", localEulerAngles.x % 360) % 360;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RegisterCompleteObjectUndo(targets, "Transform Rotate X");
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						localEulerAngles.x = (_transform.localEulerAngles.x + Event.current.delta.x * roundValue.x * sensibility);

					localEulerAngles.x = localEulerAngles.x.Round(roundValue.x) % 360;
				}

				_transform.SetLocalEulerAngles(localEulerAngles, Axes.X);
				foreach (Transform t in Selection.transforms)
				{
					t.SetLocalEulerAngles(_transform.localEulerAngles, Axes.X);
					EditorUtility.SetDirty(t);
				}
			}

			EditorGUI.BeginChangeCheck();
			localEulerAngles.y = EditorGUILayout.FloatField("Y", localEulerAngles.y % 360) % 360;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RegisterCompleteObjectUndo(targets, "Transform Rotate Y");
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						localEulerAngles.y = (_transform.localEulerAngles.y + Event.current.delta.x * roundValue.y * sensibility);

					localEulerAngles.y = localEulerAngles.y.Round(roundValue.y) % 360;
				}

				_transform.SetLocalEulerAngles(localEulerAngles, Axes.Y);
				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					selectedTransform.SetLocalEulerAngles(_transform.localEulerAngles, Axes.Y);
					EditorUtility.SetDirty(selectedTransform);
				}
			}

			EditorGUI.BeginChangeCheck();
			localEulerAngles.z = EditorGUILayout.FloatField("Z", localEulerAngles.z % 360) % 360;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RegisterCompleteObjectUndo(targets, "Transform Rotate Z");
				if (_snap && !deleteBreak && !Application.isPlaying)
				{
					if (Mathf.Abs(Event.current.delta.x) > 0)
						localEulerAngles.z = (_transform.localEulerAngles.z + Event.current.delta.x * roundValue.z * sensibility);

					localEulerAngles.z = localEulerAngles.z.Round(roundValue.z) % 360;
				}

				_transform.SetLocalEulerAngles(localEulerAngles, Axes.Z);
				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					selectedTransform.SetLocalEulerAngles(_transform.localEulerAngles, Axes.Z);
					EditorUtility.SetDirty(selectedTransform);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.EndProperty();
			EditorGUIUtility.labelWidth = labelWidth;
		}

		void ShowGrid()
		{
			if (_grid && !Application.isPlaying)
			{
				if (Selection.activeTransform == _transform)
				{
					bool is3D = SceneView.currentDrawingSceneView == null || !SceneView.currentDrawingSceneView.in2DMode;
					float size = 0.1F * ((_moveX + _moveY + _moveZ) / 3);
					const float alphaFactor = 1.25F;
					float alpha = alphaFactor / 2;

					for (int y = -_gridSize; y <= _gridSize; y++)
					{
						for (int x = -_gridSize; x <= _gridSize; x++)
						{
							if (alpha.Round(0.1f) > 0)
							{
								// X Squares
								Handles.lighting = false;
								Handles.color = new Color(0.1F, 0.25F, 0.75F, alpha);
								Vector3 offset = new Vector3(x * _moveX, y * _moveY, 0);
								Vector3 position = _transform.position + offset;

								position.x = position.x.Round(_moveX);
								position.y = position.y.Round(_moveY);
								position.z = position.z.Round(_moveZ);

								if (_showCubes)
								{
									if (SceneView.currentDrawingSceneView != null)
									{
										if (SceneView.currentDrawingSceneView.camera.WorldPointInView(position))
											Handles.CubeCap(0, position, Quaternion.identity, size);
									}
									else
										Handles.CubeCap(0, position, Quaternion.identity, size);
								}

								// X Lines
								if (_showLines)
								{
									Handles.color = new Color(0.1F, 0.25F, 0.75F, alpha / 2);

									if (x == _gridSize)
										Handles.DrawLine(new Vector3(_transform.position.x - offset.x, position.y, position.z), position);

									if (y == _gridSize)
										Handles.DrawLine(new Vector3(position.x, _transform.position.y - offset.y, position.z), position);
								}

								if (!is3D)
									alpha = 1.5F;

								if (is3D || (!is3D && y == 0))
								{
									// Y Squares
									Handles.color = new Color(0.75F, 0.35F, 0.1F, alpha);
									offset = new Vector3(x * _moveX, 0, y * _moveZ);
									position = _transform.position + offset;

									position.x = position.x.Round(_moveX);
									position.y = position.y.Round(_moveY);
									position.z = position.z.Round(_moveZ);

									if (_showCubes)
									{
										if (SceneView.currentDrawingSceneView != null)
										{
											if (SceneView.currentDrawingSceneView.camera.WorldPointInView(position))
												Handles.CubeCap(0, position, Quaternion.identity, size);
										}
										else Handles.CubeCap(0, position, Quaternion.identity, size);
									}

									// Y Lines
									if (_showLines)
									{
										Handles.color = new Color(0.75F, 0.35F, 0.1F, alpha / 2);
										if (x == _gridSize)
											Handles.DrawLine(new Vector3(_transform.position.x - offset.x, position.y, position.z), position);

										if (y == _gridSize)
											Handles.DrawLine(new Vector3(position.x, position.y, _transform.position.z - offset.z), position);
									}
								}

								if (is3D || (!is3D && x == 0))
								{
									// Z Squares
									Handles.color = new Color(0.75F, 0, 0.25F, alpha);
									offset = new Vector3(0, y * _moveY, x * _moveZ);
									position = _transform.position + offset;

									position.x = position.x.Round(_moveX);
									position.y = position.y.Round(_moveY);
									position.z = position.z.Round(_moveZ);

									if (_showCubes)
									{
										if (SceneView.currentDrawingSceneView != null)
										{
											if (SceneView.currentDrawingSceneView.camera.WorldPointInView(position))
												Handles.CubeCap(0, position, Quaternion.identity, size);
										}
										else
											Handles.CubeCap(0, position, Quaternion.identity, size);
									}

									// Z Lines
									if (_showLines)
									{
										Handles.color = new Color(0.75F, 0, 0.25F, alpha / 2);
										if (y == _gridSize)
											Handles.DrawLine(new Vector3(position.x, _transform.position.y - offset.y, position.z), position);

										if (x == _gridSize)
											Handles.DrawLine(new Vector3(position.x, position.y, _transform.position.z - offset.z), position);
									}
								}
							}
						}
					}
					SceneView.RepaintAll();
				}
			}
		}

		bool ShowToggleButton(string buttonLabel, string tooltip, GUIStyle buttonStyle)
		{
			bool pressed = SnapSettings.GetValue<bool>("Toggle" + buttonLabel + _transform.GetInstanceID());
			float width = buttonLabel.GetWidth(EditorStyles.miniFont) + 12;
			int spacing = buttonStyle == EditorStyles.miniButtonLeft ? -6 : 0;

			Rect position = EditorGUILayout.BeginVertical(GUILayout.Width(buttonLabel.GetWidth(EditorStyles.miniFont) + 16 + spacing), GUILayout.Height(EditorGUIUtility.singleLineHeight));
			EditorGUILayout.Space();
			pressed = EditorGUI.Toggle(new Rect(position.x + 4, position.y + 2, width, position.height - 1), pressed, buttonStyle);

			if (pressed)
				EditorGUI.LabelField(new Rect(position.x + 8, position.y + 1, width, position.height - 1), new GUIContent(buttonLabel, tooltip), EditorStyles.whiteMiniLabel);
			else
				EditorGUI.LabelField(new Rect(position.x + 8, position.y + 1, width, position.height - 1), new GUIContent(buttonLabel, tooltip), EditorStyles.miniLabel);

			if (pressed != SnapSettings.GetValue<bool>("Toggle" + buttonLabel + _transform.GetInstanceID()))
			{
				for (int i = 0; i < Selection.transforms.Length; i++)
				{
					Transform selectedTransform = Selection.transforms[i];
					SnapSettings.SetValue("Toggle" + buttonLabel + selectedTransform.GetInstanceID(), pressed);
					EditorUtility.SetDirty(selectedTransform);
				}
			}

			EditorGUILayout.EndVertical();

			return SnapSettings.GetValue<bool>("Toggle" + buttonLabel + _transform.GetInstanceID());
		}
	}
}