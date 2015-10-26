using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor {
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			property.SetValue(EditorGUI.LayerField(_currentPosition, label, property.GetValue<int>()));
			
			End();
		}
	}
}