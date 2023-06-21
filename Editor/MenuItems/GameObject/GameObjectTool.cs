using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// GameObject 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221206</version>
    public static class GameObjectTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.GameObjectMenuBasePath + "GameObject Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.GameObjectMenuBasePriority + 101;

        /// <summary>
        /// 递归设置 GameObject 的 Layer
        /// </summary>
        [MenuItem(k_MenuPath + "Set Layer Recursively", false, k_MenuPriority)]
        private static void Menu_SetLayerRecursively()
        {
            if (!Selection.activeGameObject) return;
            GameObject gameObject = Selection.activeGameObject;
            InputDialogWindow inputDialog = InputDialogWindow.Create("New Layer");
            void InputDialogConfirmCallback(string input)
            {
                if (int.TryParse(input, out int layer))
                {
                    GameObjectUtility.SetLayerRecursively(gameObject, layer);
                }
            };
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

    }

}
