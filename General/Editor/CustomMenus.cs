using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using Pseudo.Input.Internal;
using Pseudo.Pooling;

namespace Pseudo.Editor.Internal
{
	public static class CustomMenus
	{
		[MenuItem("Assets/Create/Mesh")]
		static void CreateMesh()
		{
			var mesh = new Mesh();
			var path = AssetDatabaseUtility.GenerateUniqueAssetPath("Mesh");
			AssetDatabase.CreateAsset(mesh, path);
			Selection.activeObject = mesh;
		}

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

		[MenuItem("Pseudo/Select/Sprite Renderers", false, -8)]
		static void SelectAllSpriteRenderers()
		{
			SelectGameObjectsOfType<SpriteRenderer>();
		}

		static void SelectGameObjectsOfType<T>() where T : Component
		{
			var selected = new List<GameObject>();

			if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
			{
				for (int i = 0; i < Selection.gameObjects.Length; i++)
				{
					var gameObject = Selection.gameObjects[i];
					var children = gameObject.GetChildren(true);

					if (gameObject.GetComponent<T>() != null)
						selected.Add(gameObject);

					for (int j = 0; j < children.Length; j++)
					{
						var child = children[j];

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
	}
}
