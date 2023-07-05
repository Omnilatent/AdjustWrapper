using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.AdjustUnity.Editor
{
    public class SetupMenuItems
    {
        // [MenuItem("Tools/Omnilatent/Adjust Unity/Import Adjust Assembly Definitions")]
        public static void ImportRequiredFiles()
        {
            string path = GetPackagePath("Assets/Omnilatent/AdjustWrapper/AdjustAssemblyDefinition.unitypackage", "AdjustAssemblyDefinition");
            AssetDatabase.ImportPackage(path, true);
        }

        static string GetPackagePath(string path, string filename)
        {
            if (!File.Exists($"{Application.dataPath}/../{path}"))
            {
                Debug.Log($"{filename} not found at {path}, attempting to search whole project for {filename}");
                string[] guids = AssetDatabase.FindAssets($"{filename} l:package", new string[] { "Assets", "Packages" });
                if (guids.Length > 0)
                {
                    path = AssetDatabase.GUIDToAssetPath(guids[0]);
                }
                else
                {
                    Debug.LogError($"{filename} not found at {Application.dataPath}/../{path}");
                    return null;
                }
            }
            return path;
        }
    }
}