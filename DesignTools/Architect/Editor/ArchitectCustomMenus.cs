using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Pseudo.Internal;

namespace Pseudo
{
	public static class ArchitectCustomMenus
	{
		[MenuItem("Pseudo/Create/Architect/Linker", priority = 9)]
		[MenuItem("Assets/Create/Pseudo/Architect/Linker", priority = 9)]
		static void CreateArchitectLinker()
		{
			ArchitectLinker linker = ScriptableObject.CreateInstance<ArchitectLinker>();
			string path = AssetDatabaseUtility.GenerateUniqueAssetPath("Linker");
			AssetDatabase.CreateAsset(linker, path);
		}
	}
}