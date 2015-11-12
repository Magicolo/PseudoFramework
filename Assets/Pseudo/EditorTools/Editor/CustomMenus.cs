﻿using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using Pseudo.Internal.Audio;
using Pseudo.Internal.Input;

namespace Pseudo.Internal.Editor
{
	public static class CustomMenus
	{
		[MenuItem("Pseudo/Create/Sprite", false, -10)]
		static void CreateSprite()
		{
			if (Array.TrueForAll(Selection.objects, selected => !(selected is Texture)))
			{
				Debug.LogError("No sprites were selected.");
				return;
			}

			for (int i = 0; i < Selection.objects.Length; i++)
			{
				Texture texture = Selection.objects[i] as Texture;

				if (texture == null)
					continue;

				string textureName = texture.name.EndsWith("Texture") ? texture.name.Substring(0, texture.name.Length - "Texture".Length) : texture.name;
				string texturePath = AssetDatabase.GetAssetPath(texture);
				string materialPath = Path.GetDirectoryName(texturePath) + "/" + textureName + ".mat";

				Sprite sprite = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Sprite)) as Sprite;

				if (sprite == null)
				{
					Debug.LogError(string.Format("Texture {0} must be imported as a sprite.", texture.name));
					continue;
				}

				AssetDatabase.CopyAsset(AssetDatabaseUtility.GetAssetPath("GraphicsTools/SpriteMaterial.mat"), materialPath);
				AssetDatabase.Refresh();

				Material material = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material)) as Material;

				GameObject gameObject = new GameObject(textureName);
				GameObject child = gameObject.AddChild("Sprite");
				SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();

				spriteRenderer.sprite = sprite;
				spriteRenderer.material = material;

				PrefabUtility.CreatePrefab(Path.GetDirectoryName(texturePath) + "/" + textureName + ".prefab", gameObject);
				AssetDatabase.Refresh();

				gameObject.Destroy();
			}
		}

		[MenuItem("Pseudo/Create/Particle", false, -9)]
		static void CreateParticle()
		{
			if (Array.TrueForAll(Selection.objects, selected => !(selected is Texture)))
			{
				Debug.LogError("No textures were selected.");
				return;
			}

			for (int i = 0; i < Selection.objects.Length; i++)
			{
				Texture texture = Selection.objects[i] as Texture;

				if (texture == null)
					continue;

				string textureName = texture.name.EndsWith("Texture") ? texture.name.Substring(0, texture.name.Length - "Texture".Length) : texture.name;
				string texturePath = AssetDatabase.GetAssetPath(texture);
				string materialPath = Path.GetDirectoryName(texturePath) + "/" + textureName + ".mat";

				AssetDatabase.CopyAsset(AssetDatabaseUtility.GetAssetPath("GraphicsTools/ParticleMaterial.mat"), materialPath);
				AssetDatabase.Refresh();

				Material material = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material)) as Material;
				material.mainTexture = texture;
			}
		}

		[MenuItem("Pseudo/Select/Audio Sources", false, -8)]
		static void SelectAllAudioSources()
		{
			SelectGameObjectsOfType<AudioSource>();
		}

		static void SelectGameObjectsOfType<T>() where T : Component
		{
			List<GameObject> selected = new List<GameObject>();

			if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
			{
				for (int i = 0; i < Selection.gameObjects.Length; i++)
				{
					GameObject gameObject = Selection.gameObjects[i];
					GameObject[] children = gameObject.GetChildren(true);

					if (gameObject.GetComponent<T>() != null)
						selected.Add(gameObject);

					for (int j = 0; j < children.Length; j++)
					{
						GameObject child = children[j];

						if (child.GetComponent<T>() != null)
							selected.Add(child);
					}
				}
			}

			Selection.objects = selected.ToArray();
		}

		[MenuItem("Pseudo/Utility/Setup Input Manager", false, -6)]
		static void SetupInputManager()
		{
			InputUtility.SetupInputManager();
		}

		[MenuItem("Pseudo/Utility/Update Copy Methods", false, -5)]
		static void UpdateCopyMethods()
		{
			bool refresh = false;

			for (int i = 0; i < TypeExtensions.AllTypes.Length; i++)
			{
				Type type = TypeExtensions.AllTypes[i];

				bool copyClass = type.HasAttribute(typeof(CopyAttribute));
				bool isCopyable = Array.Exists(type.GetInterfaces(), interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICopyable<>));

				if (!type.IsInterface && copyClass && isCopyable)
					refresh |= UpdateCopyMethod(type);
			}

			if (refresh)
				AssetDatabase.Refresh();
			else
				Debug.Log("All copy methods are up to date.");
		}

		static bool UpdateCopyMethod(Type type, params string[] membersToIgnore)
		{
			bool refresh = false;
			string[] assetGuids = AssetDatabase.FindAssets(type.GetName());

			for (int i = 0; i < assetGuids.Length; i++)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);

				if (assetPath.EndsWith(type.GetName() + ".cs", StringComparison.Ordinal))
				{
					string script = File.ReadAllText(assetPath);
					string typeName = string.Empty;
					int indent = 0;
					string currentCopyMethod = GetCurrentCopyMethod(type, script, ref typeName, ref indent);
					string newCopyMethod = GetNewCopyMethod(type, typeName, indent, membersToIgnore);

					if (!string.IsNullOrEmpty(currentCopyMethod) && !string.IsNullOrEmpty(newCopyMethod) && currentCopyMethod != newCopyMethod)
					{
						File.WriteAllText(assetPath, script.Replace(currentCopyMethod, newCopyMethod));
						refresh = true;
						Debug.Log(string.Format("Updated copy method in class {0}.\r\nOld method:\r\n{1}\nNew method:\r\n{2}", type.Name, currentCopyMethod, newCopyMethod));
					}
				}
			}

			return refresh;
		}

		static string GetCurrentCopyMethod(Type type, string script, ref string typeName, ref int indent)
		{
			int startIndex = script.IndexOf(string.Format("public void Copy(", type.GetName()), StringComparison.Ordinal);
			int endIndex = -1;
			int bracketCount = 0;

			if (startIndex < 0)
				return string.Empty;

			typeName = script.GetRange(script.IndexOf('(', startIndex) + 1, ' ');
			indent = 0;

			for (int i = startIndex; i-- > 0;)
			{
				if (script[i] == '	')
					indent++;
				else
					break;
			}

			for (int i = script.IndexOf('{', startIndex); i < script.Length; i++)
			{
				char c = script[i];

				if (c == '{')
					bracketCount++;
				else if (c == '}')
					bracketCount--;

				if (bracketCount == 0)
				{
					endIndex = i + 1;
					break;
				}
			}

			return script.Substring(startIndex, endIndex - startIndex);
		}

		static string GetNewCopyMethod(Type type, string typeName, int indent, params string[] membersToIgnore)
		{
			string indentString = string.Empty;

			for (int i = 0; i < indent - 1; i++)
				indentString += "	";

			string body = "public void Copy(" + typeName + " reference)\r\n";

			indentString += '	';
			body += indentString + "{\r\n";
			indentString += '	';

			if (type.BaseType != null && type.BaseType.Is(typeof(ICopyable<>), type.BaseType))
				body += indentString + "base.Copy(reference);\r\n\r\n";

			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];

				if (field.IsInitOnly || field.HasAttribute(typeof(DoNotCopyAttribute)))
					continue;

				body += GetFieldLine(field, indentString, membersToIgnore);

			}

			indentString = indentString.Substring(0, indentString.Length - 1);
			body += indentString + "}";

			return body;
		}

		static string GetFieldLine(FieldInfo field, string indentString, params string[] membersToIgnore)
		{
			string line = "";
			Type fieldType = field.FieldType;
			string fieldName;

			if (field.HasAttribute(typeof(CompilerGeneratedAttribute)) && field.Name.Contains("k__BackingField"))
				fieldName = field.Name.GetRange(field.Name.IndexOf('<') + 1, '>');
			else
				fieldName = field.Name;

			if (!membersToIgnore.Contains(fieldName))
			{
				bool copyTo = fieldType.IsClass && fieldType.Is(typeof(ICopyable<>), fieldType) && field.HasAttribute(typeof(CopyToAttribute));

				if (fieldType.IsArray || fieldType.Is(typeof(ICollection)) || copyTo)
					line += indentString + "CopyUtility.CopyTo(reference." + fieldName + ", ref " + fieldName + ");\r\n";
				else
					line += indentString + fieldName + " = reference." + fieldName + ";\r\n";
			}

			return line;
		}
	}
}
