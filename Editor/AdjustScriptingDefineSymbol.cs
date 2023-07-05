using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Omnilatent.AdjustUnity
{
    public static class AdjustScriptingDefineSymbol
    {
        const string SYMBOL = "ADJUST_SDK";

        // [InitializeOnLoadMethod]
        public static void Init()
        {
#if !ADJUST_SDK
            // Get current defines
            string defineSymbolString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            // Split at ;
            List<string> symbols = defineSymbolString.Split(';').ToList();
            // check if defines already exist given define
            if (!symbols.Contains(SYMBOL))
            {
                // if not add it at the end with a leading ; separator
                defineSymbolString += $";{SYMBOL}";

                // write the new defines back to the PlayerSettings
                // This will cause a recompilation of your scripts
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineSymbolString);

                Debug.Log($"Scripting Define Symbol '{SYMBOL}' was added.");
            }
#endif
        }
    }
}