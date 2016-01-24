using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace Pseudo
{
	class LoadMapContextMenu
	{
		static ArchitectLinker linker;

		[MenuItem("Assets/Create/Pseudo/Architect/LoadThisMapFile", true)]
		static bool LoadMapValidation()
		{
			return Selection.activeObject != null && AssetDatabaseUtility.GetSelectedAssetExtention().Equals(".arc");
		}
		[MenuItem("Assets/Create/Pseudo/Architect/LoadThisMapFile")]
		static void LoadMap()
		{
			if (linker == null)
				Debug.Log("Yo doit select un Linker");
			else
			{
				string path = AssetDatabaseUtility.GetSelectedAssetPath();
				WorldOpener.OpenFile(linker, path);
			}
		}


		[MenuItem("Assets/Create/Pseudo/Architect/SelectValidation", true)]
		static bool SelectLinkerValidation()
		{
			return Selection.activeObject != null && AssetDatabaseUtility.GetSelectedAssetExtention().Equals(".asset");
		}
		[MenuItem("Assets/Create/Pseudo/Architect/SelectValidation")]
		static void SelectLinker()
		{
			linker = (ArchitectLinker)Selection.activeObject;
		}
	}
}
