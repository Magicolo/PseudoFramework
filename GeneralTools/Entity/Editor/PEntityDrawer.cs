using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Entity
{
	[CustomPropertyDrawer(typeof(PEntity))]
	public class PEntityDrawer : CustomPropertyDrawerBase
	{
		List<GUIContent> errors = new List<GUIContent>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			currentPosition.height = EditorGUI.GetPropertyHeight(property, label);
			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);
			EditorGUI.PropertyField(currentPosition, property, GUIContent.none);

			if (errors.Count > 0)
			{
				currentPosition.x -= 21f;
				currentPosition.width = 20f;
				currentPosition.height = 20f;
				var errorIcon = new GUIStyle("Wizard Error").normal.background;
				if (GUI.Button(currentPosition, new GUIContent(errorIcon), new GUIStyle()))
				{
					GenericMenu menu = new GenericMenu();

					for (int i = 0; i < errors.Count; i++)
						menu.AddItem(errors[i], false, () => { });

					menu.DropDown(currentPosition);
				}
			}

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = base.GetPropertyHeight(property, label);
			CheckRequiredComponents(property.GetValue<PEntity>());

			return height;
		}

		void CheckRequiredComponents(PEntity entity)
		{
			errors.Clear();

			if (!fieldInfo.IsDefined(typeof(RequiresAttribute), true))
				return;

			var attribute = (RequiresAttribute)fieldInfo.GetCustomAttributes(typeof(RequiresAttribute), true)[0];

			if (entity == null && !attribute.CanBeNull)
				errors.Add(string.Format("Field must be assigned.").ToGUIContent());

			if (entity == null)
				return;

			for (int j = 0; j < attribute.Types.Length; j++)
			{
				var type = attribute.Types[j];

				if (!entity.HasComponent(type))
					errors.Add(string.Format("Missing required component: {0}", type.Name).ToGUIContent());
			}
		}
	}
}