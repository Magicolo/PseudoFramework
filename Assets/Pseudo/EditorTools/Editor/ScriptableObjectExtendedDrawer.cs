//using UnityEngine;
//using UnityEditor;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Pseudo.Internal.Editor;
//using System.IO;
//using System.Reflection;

//namespace Pseudo.Internal.Editor
//{
//	[CustomPropertyDrawer(typeof(ScriptableObjectExtended), true)]
//	public class ScriptableObjectExtendedDrawer : CustomPropertyDrawerBase
//	{
//		SerializedObject _serialized;
//		List<SerializedProperty> _toShow = new List<SerializedProperty>();

//		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//		{
//			Begin(position, property, label);

//			ShowMain();
//			ShowProperties();

//			End();
//		}

//		void ShowMain()
//		{
//			_currentPosition.height = EditorGUI.GetPropertyHeight(_currentProperty, _currentLabel, true);

//			ShowContextMenu();

//			if (_currentProperty.objectReferenceValue != null)
//				CustomEditorBase.DragArea(EditorGUI.IndentedRect(_currentPosition), _currentProperty.objectReferenceValue);

//			EditorGUI.PropertyField(_currentPosition, _currentProperty);
//		}

//		void ShowContextMenu()
//		{
//			Type referenceType = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
//			Type[] derivedTypes = referenceType.GetAssignableTypes();

//			List<GUIContent> options = new List<GUIContent>();
//			List<GenericMenu.MenuFunction2> callbacks = new List<GenericMenu.MenuFunction2>();
//			List<Type> data = new List<Type>();

//			for (int i = 0; i < derivedTypes.Length; i++)
//			{
//				Type derivedType = derivedTypes[i];

//				options.Add(("Create/" + derivedType.GetName()).ToGUIContent());
//				callbacks.Add(OnContextCreate);
//				data.Add(derivedType);
//			}

//			if (_currentProperty.objectReferenceValue != null)
//			{
//				options.Add("Remove".ToGUIContent());
//				callbacks.Add(OnContextRemove);
//				data.Add(null);
//			}

//			CustomEditorBase.ContextMenu(_currentPosition, options.ToArray(), callbacks.ToArray(), data.ToArray());
//		}

//		void ShowProperties()
//		{
//			if (_serialized == null)
//				return;

//			EditorGUI.BeginChangeCheck();

//			_currentProperty.isExpanded = EditorGUI.Foldout(_currentPosition, _currentProperty.isExpanded, GUIContent.none);
//			_currentPosition.y += _currentPosition.height;

//			if (_currentProperty.isExpanded)
//			{
//				SerializedProperty iterator = _serialized.GetIterator();

//				iterator = _serialized.GetIterator();
//				iterator.NextVisible(true);

//				EditorGUI.indentLevel += 1;
//				int currentIndent = EditorGUI.indentLevel;

//				while (iterator.NextVisible(iterator.isExpanded))
//				{
//					_currentPosition.height = EditorGUI.GetPropertyHeight(iterator, iterator.ToGUIContent(), false);

//					EditorGUI.indentLevel = currentIndent + iterator.depth;
//					EditorGUI.PropertyField(_currentPosition, iterator);

//					_currentPosition.y += _currentPosition.height;
//				}

//				EditorGUI.indentLevel = currentIndent;
//				EditorGUI.indentLevel -= 1;
//			}

//			if (EditorGUI.EndChangeCheck())
//				_serialized.ApplyModifiedProperties();
//		}

//		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//		{
//			float height = base.GetPropertyHeight(property, label);
//			_serialized = property.objectReferenceValue == null ? null : new SerializedObject(property.objectReferenceValue);
//			_toShow.Clear();

//			if (property.isExpanded && _serialized != null)
//			{
//				SerializedProperty iterator = _serialized.GetIterator();

//				iterator = _serialized.GetIterator();
//				iterator.NextVisible(true);

//				while (iterator.NextVisible(iterator.isExpanded))
//					height += EditorGUI.GetPropertyHeight(iterator, iterator.ToGUIContent(), false);
//			}

//			return height;
//		}

//		void OnContextCreate(object data)
//		{
//			UnityEngine.Object instance = ScriptableObject.CreateInstance(data as Type);
//			instance.name = "";
//			string extension = Path.GetExtension(AssetDatabase.GetAssetPath(_target)).ToLower();

//			if (extension == ".prefab")
//				AssetDatabase.AddObjectToAsset(instance, _target);
//			else if (extension == ".asset")
//			{
//				AssetDatabase.AddObjectToAsset(instance, _target);
//			}

//			_currentProperty.SetValue(instance);
//		}

//		void OnContextRemove(object data)
//		{
//			_currentProperty.SetValue(null);
//		}

//		void OnDropped(UnityEngine.Object dropped)
//		{
//			_currentProperty.SetValue(dropped);
//		}

//		[UnityEditor.Callbacks.DidReloadScripts, InitializeOnLoadMethod]
//		static void OnScriptReload()
//		{
//			PrefabUtility.prefabInstanceUpdated += OnPrefabUpdated;
//		}

//		static void OnPrefabUpdated(GameObject obj)
//		{
//			GameObject prefab = PrefabUtility.GetPrefabParent(obj) as GameObject;
//			string prefabPath = AssetDatabase.GetAssetPath(prefab);
//			MonoBehaviour[] objBehaviours = obj.GetComponents<MonoBehaviour>();
//			MonoBehaviour[] prefabBehaviours = prefab.GetComponents<MonoBehaviour>();
//			UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(prefabPath);
//			bool refresh = false;

//			// Destroy existing ScriptableObjects
//			for (int i = 0; i < assets.Length; i++)
//			{
//				UnityEngine.Object asset = assets[i];

//				if (asset != null && asset is ScriptableObject)
//				{
//					asset.Destroy(true);
//					assets[i] = null;
//				}
//			}

//			// Create new ScriptableObjects
//			for (int i = 0; i < objBehaviours.Length; i++)
//			{
//				MonoBehaviour behaviour = objBehaviours[i];
//				SerializedObject behaviourSerialized = new SerializedObject(behaviour);
//				Dictionary<SerializedProperty, string> scriptableProperties = new Dictionary<SerializedProperty, string>();
//				FillScriptableObjectProperties(scriptableProperties, behaviourSerialized.GetIterator(), behaviour.GetType().Name, new List<ScriptableObject>());

//				foreach (KeyValuePair<SerializedProperty, string> pair in scriptableProperties)
//				{
//					ScriptableObject scriptable = pair.Key.GetValue<ScriptableObject>();
//					ScriptableObject scriptableClone = null;

//					if (scriptable != null)
//					{
//						scriptable.name = pair.Value;
//						scriptableClone = UnityEngine.Object.Instantiate(scriptable);
//						scriptableClone.name = pair.Value;
//						refresh = true;
//						AssetDatabase.AddObjectToAsset(scriptableClone, prefab);
//					}

//					// Set the field on the prefab MonoBehaviour
//					string fieldPath = pair.Value.Split('.').Concat(".", 1);
//					prefabBehaviours[i].SetValueToMemberAtPath(fieldPath, scriptableClone);
//				}
//			}

//			if (refresh)
//				AssetDatabase.Refresh();
//		}

//		static void FillScriptableObjectProperties(Dictionary<SerializedProperty, string> properties, SerializedProperty iterator, string parentPath, List<ScriptableObject> toIgnore)
//		{
//			iterator.NextVisible(true);
//			iterator.NextVisible(true);

//			while (iterator.NextVisible(true))
//			{
//				FieldInfo field = null;

//				try
//				{
//					field = iterator.serializedObject.targetObject.GetMemberInfoAtPath(iterator.GetAdjustedPath()) as FieldInfo;
//				}
//				catch { }

//				if (field == null)
//					continue;

//				if (typeof(ScriptableObject).IsAssignableFrom(field.FieldType))
//				{
//					string path = parentPath + "." + iterator.GetAdjustedPath();
//					properties[iterator.serializedObject.FindProperty(iterator.propertyPath)] = path;

//					ScriptableObject value = field.GetValue<ScriptableObject>(iterator.serializedObject.targetObject);

//					if (value != null && !toIgnore.Contains(value))
//					{
//						toIgnore.Add(value);
//						SerializedObject valueSerialized = new SerializedObject(value);

//						FillScriptableObjectProperties(properties, valueSerialized.GetIterator(), path, toIgnore);
//					}
//				}
//			}
//		}
//	}
//}