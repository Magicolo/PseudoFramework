using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Pseudo.Internal;

namespace Pseudo
{
    public static class UIFactoryCustomMenus
    {
        [MenuItem("Pseudo/Create/Other/UIFactory", priority = 9)]
        [MenuItem("Assets/Create/Other/UIFactory", priority = 9)]
        static void CreateUICreationSkinPrefabs()
        {
            UIFactory factory = ScriptableObject.CreateInstance<UIFactory>();
            string path = AssetDatabaseUtility.GenerateUniqueAssetPath("UIFactory");
            AssetDatabase.CreateAsset(factory, path);
        }
    }
}

