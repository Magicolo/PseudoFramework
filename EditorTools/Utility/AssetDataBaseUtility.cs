using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pseudo
{
	public static class AssetDatabaseUtility
	{
		public static System.Type FindType(string typeName)
		{
			for (int i = 0; i < TypeExtensions.AllTypes.Length; i++)
			{
				System.Type type = TypeExtensions.AllTypes[i];

				if (type.Name.Contains(typeName))
					return type;
			}

			return null;
		}

		public static string GetExtensionForType<T>() where T : Object
		{
			string extension = "";

			if (typeof(T) == typeof(Material))
				extension = ".mat";
			else if (typeof(T) == typeof(Cubemap))
				extension = ".cubemap";
			else if (typeof(T) == typeof(GUISkin))
				extension = ".GUISkin";
			else if (typeof(T) == typeof(Animation))
				extension = ".anim";
			else
				extension = ".asset";

			return extension;
		}

		public static T[] LoadAllAssetsOfType<T>(string path, string extension) where T : Object
		{
			List<T> assets = new List<T>();

#if UNITY_EDITOR
			string[] paths = UnityEditor.AssetDatabase.GetAllAssetPaths();

			for (int i = 0; i < paths.Length; i++)
			{
				string assetPath = UnityEditor.AssetDatabase.GetAllAssetPaths()[i];
				if (assetPath.StartsWith(path) && assetPath.EndsWith(extension))
					assets.Add(UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T);
			}
#endif

			return assets.ToArray();
		}

		public static T[] LoadAllAssetsOfType<T>(string path) where T : Object
		{
			return LoadAllAssetsOfType<T>(path, GetExtensionForType<T>());
		}

		public static T[] LoadAllAssetsOfType<T>() where T : Object
		{
			return LoadAllAssetsOfType<T>("", GetExtensionForType<T>());
		}

		public static T GetDefaultAssetOfType<T>(string assetName) where T : Object
		{
			T defaultAsset = default(T);

			foreach (Object asset in GetDefaultAssetsOfType<T>())
			{
				if (asset.name == assetName)
					defaultAsset = asset as T;
			}

			return defaultAsset;
		}

		public static Object GetDefaultAsset(string assetName)
		{
			return GetDefaultAssetOfType<Object>(assetName);
		}

		public static T[] GetDefaultAssetsOfType<T>() where T : Object
		{
			List<T> defaultAssets = new List<T>();

#if UNITY_EDITOR
			foreach (Object asset in UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Library/unity default resources"))
			{
				if (asset is T)
				{
					defaultAssets.Add(asset as T);
				}
			}
#endif

			return defaultAssets.ToArray();
		}

		public static string GetAssetPath(Object obj)
		{
			string path = "";

#if UNITY_EDITOR
			path = UnityEditor.AssetDatabase.GetAssetPath(obj).GetRange("/Assets".Length);
#endif

			return path;
		}

		public static string GetResourcesPath(Object obj)
		{
			string resourcesPath = "";

#if UNITY_EDITOR
			resourcesPath = GetResourcesPath(UnityEditor.AssetDatabase.GetAssetPath(obj));
#endif

			return resourcesPath;
		}

		public static string GetResourcesPath(string path)
		{
			string resourcesPath = "";

			resourcesPath = GetPathWithoutExtension(path.Substring(path.IndexOf("Resources/") + "Resources/".Length));

			return resourcesPath;
		}

		public static string GetPathWithoutExtension(string path)
		{
			return path.Substring(0, path.Length - Path.GetExtension(path).Length);
		}

		public static string GetFolderPath(string folderName)
		{
			string folderPath = "";

#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths())
			{
				if (path.EndsWith(folderName))
				{
					folderPath = path;
					break;
				}
			}
#endif

			return folderPath;
		}

		public static string[] GetFolderPaths(string folderName)
		{
			List<string> folderPaths = new List<string>();

#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths())
			{
				if (path.EndsWith(folderName))
				{
					folderPaths.Add(path);
					break;
				}
			}
#endif

			return folderPaths.ToArray();
		}

		public static T LoadAssetInFolder<T>(string assetFileName, string folderName) where T : Object
		{
			T asset = default(T);

#if UNITY_EDITOR
			asset = UnityEditor.AssetDatabase.LoadAssetAtPath(GetFolderPath(folderName) + Path.AltDirectorySeparatorChar + assetFileName, typeof(T)) as T;
#endif

			return asset;
		}

		public static Object[] LoadAssetsInFolder<T>(string assetFileName, string folderName) where T : Object
		{
			Object[] assets = null;

#if UNITY_EDITOR
			assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(GetFolderPath(folderName) + Path.AltDirectorySeparatorChar + assetFileName);
#endif

			return assets;
		}

		public static T GetOrAddAssetOfType<T>(string name, string path) where T : ScriptableObject
		{
			T asset = GetAssetOfType<T>(path);

#if UNITY_EDITOR
			if (asset == null)
			{
				Object[] existingAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);

				if (existingAssets == null || existingAssets.Length == 0)
					asset = CreateAssetOfType<T>(name, path);
				else
					asset = AddAssetOfType<T>(name, path);
			}
#endif

			return asset;
		}

		public static T GetAssetOfType<T>(string path) where T : ScriptableObject
		{
			T asset = null;

#if UNITY_EDITOR
			Object[] existingAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
			asset = System.Array.Find(existingAssets, s => s is T) as T;
#endif

			return asset;
		}

		public static T CreateAssetOfType<T>(string name, string path) where T : ScriptableObject
		{
			T asset = null;

#if UNITY_EDITOR
			asset = ScriptableObject.CreateInstance<T>();
			asset.name = name;
			UnityEditor.AssetDatabase.CreateAsset(asset, path);
#endif

			return asset;
		}

		public static T AddAssetOfType<T>(string name, string path) where T : ScriptableObject
		{
			T asset = null;

#if UNITY_EDITOR
			asset = ScriptableObject.CreateInstance<T>();
			asset.name = name;
			UnityEditor.AssetDatabase.AddObjectToAsset(asset, path);
#endif

			return asset;
		}

		public static void SaveAssets()
		{
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.SaveAssets();
#endif
		}

		public static string GetAssetPath(string assetName)
		{
			string assetPath = "";

#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths())
			{
				if (path.EndsWith(assetName))
				{
					assetPath = path;
					break;
				}
			}
#endif

			return assetPath;
		}

		public static bool PathIsRelativeTo(string path, string relativeTo)
		{
			return path.StartsWith(relativeTo);
		}

        /// <summary>
        /// Return a unique path for the file <i>name</i>.
        /// If a path is provided, it will use it as a root. 
        /// If no path is provided, it will look for currently selected item.
        /// </summary>
        public static string GenerateUniqueAssetPath(string name, string path = "")
        {
            string assetDirectory;

            if (!string.IsNullOrEmpty(path))
                assetDirectory = Path.GetDirectoryName(path);
            if (Selection.activeObject == null)
                assetDirectory = "Assets";
            else if (Selection.activeObject is DefaultAsset)
                assetDirectory = AssetDatabase.GetAssetPath(Selection.activeObject);
            else
                assetDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

            return AssetDatabase.GenerateUniqueAssetPath(assetDirectory + "/" + name + ".asset");
        }
    }
}
