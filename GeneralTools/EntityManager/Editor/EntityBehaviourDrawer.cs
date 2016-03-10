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
	[CustomPropertyDrawer(typeof(EntityBehaviour))]
	public class EntityBehaviourDrawer : PPropertyDrawer
	{
		List<GUIContent> errors = new List<GUIContent>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			currentPosition.height = EditorGUI.GetPropertyHeight(property, label);
			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);

			BeginIndent(0);
			EditorGUI.PropertyField(currentPosition, property, GUIContent.none);
			EndIndent();

			currentPosition.x -= 21f;
			currentPosition.y -= 1f;
			PEditor.Errors(currentPosition, errors);

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = base.GetPropertyHeight(property, label);
			CheckRequiredComponents(property.GetValue<EntityBehaviour>());

			return height;
		}

		void CheckRequiredComponents(EntityBehaviour entity)
		{
			errors.Clear();

			if (!fieldInfo.IsDefined(typeof(EntityRequiresAttribute), true))
				return;

			var attribute = (EntityRequiresAttribute)fieldInfo.GetCustomAttributes(typeof(EntityRequiresAttribute), true)[0];

			if (entity == null && !attribute.CanBeNull)
				errors.Add(string.Format("Field cannot be null.").ToGUIContent());

			if (entity == null)
				return;

			for (int j = 0; j < attribute.Types.Length; j++)
			{
				var type = attribute.Types[j];

				if (type != null && typeof(IComponent).IsAssignableFrom(type) && entity.GetComponent(type) == null)
					errors.Add(string.Format("Missing required component: {0}", type.Name).ToGUIContent());
			}
		}
	}
}