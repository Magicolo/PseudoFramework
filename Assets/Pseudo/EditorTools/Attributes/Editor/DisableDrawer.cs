using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor {
	[CustomPropertyDrawer(typeof(DisableAttribute))]
	public class DisableDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
		
			EditorGUI.PropertyField(_currentPosition, property, label, true);
			
			End();
		}
	}
}