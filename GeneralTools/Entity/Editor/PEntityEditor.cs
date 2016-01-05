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

namespace Pseudo.Internal.Entity
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

		Dictionary<string, ComponentCategory> categoryDict = new Dictionary<string, ComponentCategory>();
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

			InitializeCategories();
			ShowGroups();
			Separator();
			ShowComponentCategories();

			End();
		}

		void ShowGroups()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("groups"));
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
				var category = categories[i];

				if (category.Components.Count == 0)
					continue;

				if (i > 0)
					GUILayout.Space(5f);

				BeginBox(CustomEditorStyles.ColoredBox(new Color(0.5f, 0.5f, 0.5f, 0.25f), 0));

				currentCategory = categories[i];
				ArrayFoldout(currentCategory.DummyValue,
					currentCategory.Name.ToGUIContent(),
					disableOnPlay: false,
					foldoutDrawer: ShowComponentCategoryFoldout,
					elementDrawer: ShowComponent,
					onPreElementDraw: OnPreComponent,
					onPostElementDraw: OnPostComponent,
					deleteCallback: DeleteComponent,
					reorderCallback: ReorderComponent);

				GUILayout.Space(4f);
				EndBox();
			}

			if (entity.GetAllComponents().Count > 0)
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

		void OnPreComponent(SerializedProperty arrayProperty, int index, SerializedProperty property)
		{
			currentComponent = currentCategory.Components[index];
			BeginBox();

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			var rect = EditorGUILayout.BeginVertical();

			rect.x += rect.width - 34f;
			rect.width = 16f;
			rect.height = 16f;

			currentComponent.Active = GUI.Toggle(rect, currentComponent.Active, "");
			ShowComponentErrors(rect);

			EditorGUI.indentLevel = indent;
		}

		void ShowComponent(SerializedProperty arrayProperty, int index, SerializedProperty property)
		{
			var name = currentComponent.GetTypeName();

			if (name.EndsWith("Component"))
				name = name.Substring(0, name.Length - "Component".Length);

			ObjectField(currentComponent, name.ToGUIContent());
		}

		void OnPostComponent(SerializedProperty arrayProperty, int index, SerializedProperty property)
		{
			EditorGUILayout.EndVertical();
			EndBox();
		}

		void ShowComponentErrors(Rect rect)
		{
			var errors = new List<GUIContent>();
			var data = new List<object>();

			// Gather errors
			if (currentComponent.GetType().IsDefined(typeof(EntityRequiresAttribute), true))
			{
				var requireAttribute = (EntityRequiresAttribute)currentComponent.GetType().GetCustomAttributes(typeof(EntityRequiresAttribute), true)[0];

				for (int i = 0; i < requireAttribute.Types.Length; i++)
				{
					var type = requireAttribute.Types[i];

					if (type != null && typeof(IComponent).IsAssignableFrom(type) && !entity.HasComponent(type))
					{
						errors.Add(string.Format("Missing required component: {0}", type.Name).ToGUIContent());
						data.Add(new ErrorData(currentCategory, currentComponent, ErrorData.ErrorTypes.MissingComponent, type));
					}
				}
			}

			rect.x -= 21f;
			rect.y -= 1f;
			Errors(rect, errors, data, OnComponentError);
		}

		void AddComponent(Type type)
		{
			entity.AddComponent(type);
			InitializeCategories();
		}

		void DeleteComponent(SerializedProperty arrayProperty, int index)
		{
			entity.RemoveComponent(currentCategory.Components[index]);
			InitializeCategories();
		}

		void ReorderComponent(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			entity.GetAllComponents().Switch(currentCategory.ComponentIndices[sourceIndex], currentCategory.ComponentIndices[targetIndex]);
			InitializeCategories();
		}

		void OnComponentError(object data)
		{
			var errorData = (ErrorData)data;

			switch (errorData.ErrorType)
			{
				case ErrorData.ErrorTypes.MissingComponent:
					if (!errorData.MissingComponentType.IsInterface && !errorData.MissingComponentType.IsAbstract)
						entity.AddComponent(errorData.MissingComponentType);
					break;
				default:
					break;
			}

			InitializeCategories();
		}

		void InitializeCategories()
		{
			// Fixes the "Generating diff of this object for undo because the type tree changed." bug.
			if (EditorApplication.isPlaying != EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			foreach (var pair in categoryDict)
				pair.Value.RemoveAllComponents();

			var components = entity.GetAllComponents();

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

					category.AddComponent(component, i);
				}
				else
				{
					ComponentCategory category;
					const string categoryName = "\0Uncategorized";

					if (!categoryDict.TryGetValue(categoryName, out category))
					{
						category = new ComponentCategory(categoryName, int.MinValue);
						categoryDict[categoryName] = category;
					}

					category.AddComponent(component, i);
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

				if (type.IsAbstract || type.IsInterface || !type.IsDefined(typeof(SerializableAttribute), false))
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
			public readonly List<int> ComponentIndices = new List<int>();
			public readonly SerializedProperty DummyValue;
			public bool IsExpanded
			{
				get { return EditorPrefs.GetBool(expandedPrefix + Name, true); }
				set { EditorPrefs.SetBool(expandedPrefix + Name, value); }
			}

			public ComponentCategory(string name, int order)
			{
				Name = name;
				Order = order;
				DummyValue = DummyUtility.GetSerializedDummy(new string[0]);
			}

			public void AddComponent(IComponent component, int index)
			{
				Components.Add(component);
				ComponentIndices.Add(index);
				DummyValue.Add(component.GetTypeName());
			}

			public void RemoveAllComponents()
			{
				Components.Clear();
				ComponentIndices.Clear();
				DummyValue.arraySize = 0;
			}
		}

		public class ErrorData
		{
			public enum ErrorTypes
			{
				MissingComponent
			}

			public readonly ComponentCategory Category;
			public readonly IComponent Component;
			public readonly ErrorTypes ErrorType;
			public readonly Type MissingComponentType;

			public ErrorData(ComponentCategory category, IComponent component, ErrorTypes errorType, Type missingComponentType) : this(category, component, errorType)
			{
				MissingComponentType = missingComponentType;
			}

			ErrorData(ComponentCategory category, IComponent component, ErrorTypes errorType)
			{
				Category = category;
				Component = component;
				ErrorType = errorType;
			}
		}
	}
}