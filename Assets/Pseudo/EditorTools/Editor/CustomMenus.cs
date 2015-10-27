using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using Pseudo.Internal.Audio;

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

				AssetDatabase.CopyAsset(HelperFunctions.GetAssetPath("GraphicsTools/SpriteMaterial.mat"), materialPath);
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

				AssetDatabase.CopyAsset(HelperFunctions.GetAssetPath("GraphicsTools/ParticleMaterial.mat"), materialPath);
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

		[MenuItem("Pseudo/Misc/Update Copy Methods", false, -5)]
		static void UpdateCopyMethods()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			bool refresh = false;

			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = assemblies[i].GetTypes();

				for (int j = 0; j < types.Length; j++)
				{
					Type type = types[j];

					if (!type.IsInterface && type.GetCustomAttributes(typeof(DoNotCopyAttribute), false).Length == 0 && Array.Exists(type.GetInterfaces(), interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICopyable<>)))
						refresh |= UpdateCopyMethod(type);
				}
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
						Debug.Log(string.Format("Updated copy method in class {0}.\nOld method:\n{1}\nNew method:\n{2}", type.Name, currentCopyMethod, newCopyMethod));
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
			string indentString = "";

			for (int i = 0; i < indent - 1; i++)
				indentString += "	";

			string body = "public void Copy(" + typeName + " reference)\n";

			indentString += '	';

			body += indentString + "{\n";

			indentString += '	';

			if (type.BaseType != null && typeof(ICopyable<>).MakeGenericType(type.BaseType).IsAssignableFrom(type.BaseType))
				body += indentString + "base.Copy(reference);\n\n";

			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];

				if (field.IsInitOnly || field.GetCustomAttributes(typeof(DoNotCopyAttribute), false).Length > 0)
					continue;

				if (field.GetCustomAttributes(true).Contains(typeof(CompilerGeneratedAttribute)) && field.Name.Contains("k__BackingField"))
					body += GetFieldLine(field.FieldType, field.Name.GetRange(field.Name.IndexOf('<') + 1, '>'), indentString, membersToIgnore);
				else
					body += GetFieldLine(field.FieldType, field.Name, indentString, membersToIgnore);

			}

			indentString = indentString.Substring(0, indentString.Length - 1);
			body += indentString + "}";

			return body;
		}

		static string GetFieldLine(Type fieldType, string fieldName, string indentString, params string[] membersToIgnore)
		{
			string line = "";

			if (!membersToIgnore.Contains(fieldName))
			{
				if (fieldType.IsArray || typeof(ICollection).IsAssignableFrom(fieldType))
					line += indentString + "CopyUtility.CopyTo(reference." + fieldName + ", ref " + fieldName + ");\n";
				else
					line += indentString + fieldName + " = reference." + fieldName + ";\n";
			}

			return line;
		}
	}
}
