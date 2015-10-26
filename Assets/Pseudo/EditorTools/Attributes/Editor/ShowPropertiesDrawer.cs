using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal.Editor
{
	[System.Serializable]
	[CustomPropertyDrawer(typeof(ShowPropertiesAttribute)), CanEditMultipleObjects]
	public class ShowPropertiesDrawer : CustomAttributePropertyDrawerBase
	{
		SerializedObject _serialized;
		SerializedProperty _iterator;
		float _totalHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;
			_totalHeight = 0;

			Begin(position, property, label);

			position.height = EditorGUI.GetPropertyHeight(property, label, true);

			EditorGUI.PropertyField(position, property);

			if (property.GetValue() != null)
				property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

			_totalHeight += position.height;
			position.y += position.height;

			if (property.isExpanded && _serialized != null)
			{
				_iterator = _serialized.GetIterator();
				_iterator.NextVisible(true);
				_iterator.NextVisible(true);

				EditorGUI.indentLevel += 1;
				int currentIndent = EditorGUI.indentLevel;

				while (true)
				{
					position.height = EditorGUI.GetPropertyHeight(_iterator, _iterator.displayName.ToGUIContent(), false);

					_totalHeight += position.height;

					EditorGUI.indentLevel = currentIndent + _iterator.depth;
					EditorGUI.PropertyField(position, _iterator);

					position.y += position.height;

					if (!_iterator.NextVisible(_iterator.isExpanded))
					{
						break;
					}
				}

				EditorGUI.indentLevel = currentIndent;
				EditorGUI.indentLevel -= 1;

				_serialized.ApplyModifiedProperties();
			}

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			_serialized = property.objectReferenceValue == null ? null : new SerializedObject(property.objectReferenceValue);

			if (_totalHeight <= 0f)
				InitializeHeight(property, label);

			return _totalHeight + (beforeSeparator ? 16f : 0f) + (afterSeparator ? 16f : 0f);
		}

		public void InitializeHeight(SerializedProperty property, GUIContent label)
		{
			_totalHeight = EditorGUI.GetPropertyHeight(property, label, true);

			if (property.isExpanded && _serialized != null)
			{
				_iterator = _serialized.GetIterator();
				_iterator.NextVisible(true);
				_iterator.NextVisible(true);

				while (true)
				{
					_totalHeight += EditorGUI.GetPropertyHeight(_iterator, _iterator.displayName.ToGUIContent(), false);

					if (!_iterator.NextVisible(_iterator.isExpanded))
					{
						break;
					}
				}
			}
		}
	}
}

