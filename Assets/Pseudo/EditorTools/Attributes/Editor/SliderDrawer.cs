using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor {
	[CustomPropertyDrawer(typeof(SliderAttribute))]
	public class SliderDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			float min = ((SliderAttribute)attribute).min;
			float max = ((SliderAttribute)attribute).max;
		
			EditorGUI.BeginChangeCheck();
			
			_currentPosition.height = 16;
			object value = property.GetValue();
			
			if (value is int) {
				property.SetValue(EditorGUI.IntSlider(_currentPosition, label, (int)value, (int)min, (int)max));
			}
			else if (value is float) {
				property.SetValue(EditorGUI.Slider(_currentPosition, label, (float)value, min, max));
			}
			else if (value is double) {
				property.SetValue(EditorGUI.Slider(_currentPosition, label, (float)(double)value, min, max));
			}
			else {
				EditorGUI.HelpBox(_currentPosition, "The type of the field must be numerical.", MessageType.Error);
			}
			
			if (EditorGUI.EndChangeCheck()) {
				property.Clamp(min, max);
			}
			
			End();
		}
	}
}
