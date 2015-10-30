using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(PolarAttribute))]
	public class PolarDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			currentPosition.height = 16f;
			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);
			currentPosition.x -= 1f;

			BeginIndent(0);
			BeginLabelWidth(13f);

			if (property.propertyType == SerializedPropertyType.Vector2)
				ShowPolar2D(property);
			else if (property.propertyType == SerializedPropertyType.Vector3)
				ShowPolar3D(property);

			EndLabelWidth();
			EndIndent();
			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 16f;
		}

		void ShowPolar2D(SerializedProperty property)
		{
			SerializedProperty xProperty = property.FindPropertyRelative("x");
			SerializedProperty yProperty = property.FindPropertyRelative("y");
			Vector2 vector = property.GetValue<Vector2>().ToPolar().Round(0.0001f);
			currentPosition.width = currentPosition.width / 2f - 1f;

			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginProperty(currentPosition, xProperty.ToGUIContent(), xProperty);

			vector.x = EditorGUI.FloatField(currentPosition, "R", vector.x);
			currentPosition.x += currentPosition.width + 2f;

			EditorGUI.EndProperty();

			EditorGUI.BeginProperty(currentPosition, yProperty.ToGUIContent(), yProperty);

			vector.y = EditorGUI.FloatField(currentPosition, "ϴ", vector.y);

			EditorGUI.EndProperty();
			if (EditorGUI.EndChangeCheck())
				property.SetValue(vector.ToCartesian());
		}

		void ShowPolar3D(SerializedProperty property)
		{
			SerializedProperty xProperty = property.FindPropertyRelative("x");
			SerializedProperty yProperty = property.FindPropertyRelative("y");
			SerializedProperty zProperty = property.FindPropertyRelative("z");
			Vector3 vector = property.GetValue<Vector3>().ToPolar().Round(0.0001f);
			currentPosition.width = currentPosition.width / 3f - 1f;

			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginProperty(currentPosition, xProperty.ToGUIContent(), xProperty);

			vector.x = EditorGUI.FloatField(currentPosition, "R", vector.x);
			currentPosition.x += currentPosition.width + 2f;

			EditorGUI.EndProperty();

			EditorGUI.BeginProperty(currentPosition, yProperty.ToGUIContent(), yProperty);

			vector.y = EditorGUI.FloatField(currentPosition, "ϴ", vector.y);
			currentPosition.x += currentPosition.width + 2f;

			EditorGUI.EndProperty();

			EditorGUI.BeginProperty(currentPosition, zProperty.ToGUIContent(), zProperty);

			vector.z = EditorGUI.FloatField(currentPosition, "Z", vector.z);

			EditorGUI.EndProperty();

			if (EditorGUI.EndChangeCheck())
				property.SetValue(vector.ToCartesian());
		}
	}
}