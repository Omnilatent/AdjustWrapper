using System.Collections;
using System.Collections.Generic;
using Omnilatent.AdjustUnity.Editor;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Omnilatent.AdjustUnity
{
    public class InitialSetup : EditorWindow
    {
        [MenuItem("Tools/Omnilatent/Adjust Unity/Import Adjust Assembly Definitions")]
#if !ADJUST_SDK
        [InitializeOnLoadMethod]
#endif
        public static void ShowInstallWindow()
        {
            InitialSetup wnd = GetWindow<InitialSetup>();
            wnd.maxSize = new Vector2(670f, 280f);
            wnd.minSize = new Vector2(500f, 280f);
            wnd.titleContent = new GUIContent("Adjust Wrapper Initial Setup");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            Label label = new Label("Click on the following button to import files needed for Adjust Wrapper to work properly.");
            label.style.marginTop = label.style.marginBottom = 20;
            label.style.marginLeft = label.style.marginRight = 20;
            label.style.alignSelf = new StyleEnum<Align>(Align.Center);
            label.style.whiteSpace = WhiteSpace.Normal;
            root.Add(label);

            Button button = new Button();
            button.style.height = 80;
            button.style.marginTop = new StyleLength(StyleKeyword.Auto);
            button.style.marginBottom = 10;
            button.name = "button";
            button.text = "Import Adjust Wrapper's essential files";
            button.clicked += OnInstall;
            root.Add(button);
        }

        private void OnInstall()
        {
            SetupMenuItems.ImportRequiredFiles();
            AdjustScriptingDefineSymbol.Init();
        }
    }
}