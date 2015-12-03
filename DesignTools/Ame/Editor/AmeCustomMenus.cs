using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Pseudo.Internal;

namespace Pseudo
{ 
    public static class AmeCustomMenus
   {


        [MenuItem("Pseudo/Create/Ame/Linker", priority = 9)]
        [MenuItem("Assets/Create/Ame/Linker", priority = 9)]
        static void CreateAmeLinker()
        {
            AmeLinker linker = ScriptableObject.CreateInstance<AmeLinker>();
            string path = AssetDatabaseUtility.GenerateUniqueAssetPath("Linker");
            AssetDatabase.CreateAsset(linker, path);
        }


    }
}