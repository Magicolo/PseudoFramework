using Pseudo.Internal.Editor;
using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Pseudo;
using System.Reflection.Emit;
using System.Collections;
using Pseudo2;

namespace Pseudo2.Internal.Entity
{
	[CustomEditor(typeof(PEntity)), CanEditMultipleObjects]
	public class PEntityEditor : CustomEditorBase
	{
		static string[] addOptions;
		static string[] AddOptions
		{
			get
			{
				if (addOptions == null)
					InitializeAddComponentPopup();

				return addOptions;
			}
		}
		static Type[] addTypes;
		static Type[] AddTypes
		{
			get
			{
				if (addTypes == null)
					InitializeAddComponentPopup();

				return addTypes;
			}
		}

		PEntity entity;
		ComponentCategory[] categories;
		ComponentCategory currentCategory;
		IComponent currentComponent;

		public override void OnEnable()
		{
			base.OnEnable();

			entity = (PEntity)target;
			InitializeCategories();
		}

		public override void OnInspectorGUI()
		{
			Begin();

			ShowGroup();
			Separator();
			ShowComponentCategories();

			End();
		}

		void ShowGroup()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("group"));
		}

		void ShowComponentCategories()
		{
			EditorGUI.BeginChangeCheck();

			var style = new GUIStyle("popup")
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
			};

			for (int i = 0; i < categories.Length; i++)
			{
				if (i > 0)
					GUILayout.Space(5f);

				currentCategory = categories[i];
				ArrayFoldout(currentCategory.DummyValue, currentCategory.Name.ToGUIContent(), disableOnPlay: false, showElementBox: true, foldoutDrawer: ShowComponentCategoryFoldout, elementDrawer: ShowComponent, deleteCallback: DeleteComponent, reorderCallback: ReorderComponent);
			}

			Separator();

			int index = EditorGUILayout.Popup(0, AddOptions, style);
			if (EditorGUI.EndChangeCheck() && index > 0)
				AddComponent(AddTypes[index]);
		}

		void ShowComponentCategoryFoldout(SerializedProperty arrayProperty)
		{
			var style = new GUIStyle("ShurikenModuleTitle")
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				fontSize = 11,
				contentOffset = new Vector2(0f, -1f),
			};

			currentCategory.IsExpanded = GUILayout.Toggle(currentCategory.IsExpanded, currentCategory.Name, style, GUILayout.Height(15f));
			arrayProperty.isExpanded = currentCategory.IsExpanded;
		}

		void ShowComponent(SerializedProperty arrayProperty, int index, SerializedProperty property)
		{
			currentComponent = currentCategory.Components[index];

			var name = currentComponent.GetTypeName();

			if (name.EndsWith("Component"))
				name = name.Substring(0, name.Length - "Component".Length);

			ObjectField(name, currentComponent);
			ShowComponentErrors();
		}

		void ShowComponentErrors()
		{
			var errors = new List<GUIContent>();
			var position = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());

			// Gather errors
			if (currentComponent.GetType().IsDefined(typeof(RequireComponent), true))
			{
				//var requireAttribute = (RequireComponent)currentComponent.GetType().GetCustomAttributes(typeof(RequireComponent), true)[0];
			}

			// Show errors
			if (errors.Count > 0)
			{
				position.x -= 21f;
				position.width = 20f;
				position.height = 20f;
				var errorIcon = new GUIStyle("Wizard Error").normal.background;
				if (GUI.Button(position, new GUIContent(errorIcon), new GUIStyle()))
				{
					GenericMenu menu = new GenericMenu();

					for (int i = 0; i < errors.Count; i++)
						menu.AddItem(errors[i], false, () => { });

					menu.DropDown(position);
				}
			}
		}

		void AddComponent(Type type)
		{
			var component = (IComponent)Activator.CreateInstance(type);
			entity.AddComponent(component);
			InitializeCategories();
		}

		void DeleteComponent(SerializedProperty arrayProperty, int index)
		{
			DeleteFromArray(arrayProperty, index);
			entity.RemoveComponent(currentCategory.Components.Pop(index));
			InitializeCategories();
		}

		void ReorderComponent(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			entity.GetComponents().Switch(currentCategory.Components[sourceIndex], currentCategory.Components[targetIndex]);
			currentCategory.Components.Switch(sourceIndex, targetIndex);
			InitializeCategories();
		}

		object ObjectField(string name, object value)
		{
			if (value == null)
				return null;

			var dummy = DummyUtility.GetDummy(value.GetType());
			dummy.Value = value;
			var serializedDummy = DummyUtility.SerializeDummy(dummy);

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(serializedDummy.FindProperty("Value"), name.ToGUIContent(), true);

			if (EditorGUI.EndChangeCheck())
			{
				serializedDummy.ApplyModifiedProperties();
				value = dummy.Value;
			}

			return value;
		}

		void InitializeCategories()
		{
			var categoryDict = new Dictionary<string, ComponentCategory>();
			var components = entity.GetComponents();

			for (int i = 0; i < components.Count; i++)
			{
				var component = components[i];

				if (component.GetType().IsDefined(typeof(ComponentCategoryAttribute), true))
				{
					var categoryAttribute = (ComponentCategoryAttribute)component.GetType().GetCustomAttributes(typeof(ComponentCategoryAttribute), true)[0];
					ComponentCategory category;

					if (!categoryDict.TryGetValue(categoryAttribute.Category, out category))
					{
						var categoryName = categoryAttribute.Category.Split('/')[0];
						category = new ComponentCategory(categoryName, i);
						categoryDict[categoryName] = category;
					}

					category.AddComponent(component);
				}
				else
				{
					ComponentCategory category;
					const string categoryName = "\0General";

					if (!categoryDict.TryGetValue(categoryName, out category))
					{
						category = new ComponentCategory(categoryName, int.MinValue);
						categoryDict[categoryName] = category;
					}

					category.AddComponent(component);
				}
			}

			string[] keys;
			categoryDict.GetOrderedKeysValues(out keys, out categories);

			Array.Sort(keys, categories);
		}

		static void InitializeAddComponentPopup()
		{
			var types = typeof(IComponent).GetAssignableTypes();
			var categorizedOptionsList = new List<string>(types.Length);
			var categorizedTypesList = new List<Type>(types.Length);
			var otherOptionsList = new List<string>(types.Length);
			var otherTypesList = new List<Type>(types.Length);

			for (int i = 0; i < types.Length; i++)
			{
				var type = types[i];

				if (type.IsAbstract || type.IsInterface)
					continue;

				var option = type.GetName();

				if (option.EndsWith("Component"))
					option = option.Substring(0, option.Length - "Component".Length);

				if (type.IsDefined(typeof(ComponentCategoryAttribute), true))
				{
					var categoryAttribute = (ComponentCategoryAttribute)type.GetCustomAttributes(typeof(ComponentCategoryAttribute), true)[0];
					categorizedOptionsList.Add(categoryAttribute.Category + "/" + option);
					categorizedTypesList.Add(type);
				}
				else
				{
					otherOptionsList.Add(option);
					otherTypesList.Add(type);
				}
			}

			var categorizedOptions = categorizedOptionsList.ToArray();
			var categorizedTypes = categorizedTypesList.ToArray();
			Array.Sort(categorizedOptions, categorizedTypes);

			addOptions = otherOptionsList.ToArray();
			addTypes = otherTypesList.ToArray();
			Array.Sort(addOptions, addTypes);

			addOptions = new string[] { "Add Component" }.Joined(categorizedOptions.Joined(addOptions));
			addTypes = new Type[] { null }.Joined(categorizedTypes.Joined(addTypes));
		}

		public class ComponentCategory
		{
			const string expandedPrefix = "EntityComponentCategoryExpanded_";

			public readonly string Name;
			public readonly int Order;
			public readonly List<IComponent> Components = new List<IComponent>();
			public readonly SerializedObject Dummy;
			public readonly SerializedProperty DummyValue;
			public bool IsExpanded
			{
				get { return EditorPrefs.GetBool(expandedPrefix + Name); }
				set { EditorPrefs.SetBool(expandedPrefix + Name, value); }
			}
			public bool HasErrors;

			public ComponentCategory(string name, int order)
			{
				Name = name;
				Order = order;

				var dummy = DummyUtility.GetDummy(typeof(string[]));
				dummy.Value = new string[0];
				Dummy = DummyUtility.SerializeDummy(dummy);
				DummyValue = Dummy.FindProperty("Value");
			}

			public void AddComponent(IComponent component)
			{
				Components.Add(component);
				DummyValue.Add(component.GetTypeName());
			}
		}
	}
}