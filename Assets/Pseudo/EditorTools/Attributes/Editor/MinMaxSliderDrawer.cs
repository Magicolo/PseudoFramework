using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : CustomAttributePropertyDrawerBase
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Begin(position, property, label);

            float x = property.FindPropertyRelative("x").floatValue;
            float y = property.FindPropertyRelative("y").floatValue;
            float min = 0;
            float max = 0;
            string minLabel = ((MinMaxSliderAttribute)attribute).minLabel;
            string maxLabel = ((MinMaxSliderAttribute)attribute).maxLabel;

            if (property.FindPropertyRelative("z") != null)
                min = property.FindPropertyRelative("z").floatValue;
            else
                min = ((MinMaxSliderAttribute)attribute).min;
            if (property.FindPropertyRelative("w") != null)
                max = property.FindPropertyRelative("w").floatValue;
            else
                max = ((MinMaxSliderAttribute)attribute).max;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            float width = _currentPosition.width;

            _currentPosition.width = width / 4 - 4f;
            if (!noFieldLabel && !string.IsNullOrEmpty(minLabel) && width / 8 >= 16)
            {
                _currentPosition.width = Mathf.Min(minLabel.GetWidth(EditorStyles.standardFont) + 4, width / 8);
                EditorGUI.LabelField(_currentPosition, minLabel);
                _currentPosition.x += _currentPosition.width;
                _currentPosition.width = width / 4 - _currentPosition.width;
                x = EditorGUI.FloatField(_currentPosition, x);
            }
            else
                x = EditorGUI.FloatField(_currentPosition, x);

            _currentPosition.x += _currentPosition.width + 2;

            _currentPosition.width = width / 2;
            EditorGUI.MinMaxSlider(_currentPosition, ref x, ref y, min, max);

            _currentPosition.x += _currentPosition.width + 2;
            _currentPosition.width = width / 4;

            if (!noFieldLabel && !string.IsNullOrEmpty(maxLabel) && width / 8 >= 16)
            {
                float labelWidth = Mathf.Min(maxLabel.GetWidth(EditorStyles.standardFont) + 4, width / 8);
                _currentPosition.width = width / 4 - labelWidth;
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.alignment = TextAnchor.MiddleRight;
                y = EditorGUI.FloatField(_currentPosition, y);
                _currentPosition.x += _currentPosition.width;
                _currentPosition.width = labelWidth;
                EditorGUI.LabelField(_currentPosition, maxLabel, style);

            }
            else
                y = EditorGUI.FloatField(_currentPosition, y);

            property.FindPropertyRelative("x").floatValue = Mathf.Clamp(x, min, y).Round(0.001f);
            property.FindPropertyRelative("y").floatValue = Mathf.Clamp(y, x, max).Round(0.001f);

            EditorGUI.indentLevel = indent;

            End();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            base.GetPropertyHeight(property, label);

            return EditorGUIUtility.singleLineHeight + (beforeSeparator ? 16 : 0) + (afterSeparator ? 16 : 0); ;
        }
    }
}
