using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
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
        private const string MenuPath = PipiToolbox.GameObjectMenuBasePath + "GameObject Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.GameObjectMenuBasePriority + 101;

        /// <summary>
        /// 递归设置 GameObject 的 Layer
        /// </summary>
        [MenuItem(MenuPath + "Set Layer Recursively", false, MenuPriority)]
        private static void Menu_SetLayerRecursively()
        {
            if (!Selection.activeGameObject) return;
            GameObject gameObject = Selection.activeGameObject;
            InputDialogWindow inputDialog = InputDialogWindow.Create("New Layer");
            void InputDialogConfirmCallback(string input)
            {
                if (int.TryParse(input, out int layer))
                {
                    SetLayerRecursively(gameObject, layer);
                }
            };
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

        /// <summary>
        /// 递归设置 GameObject 的 Layer
        /// </summary>
        /// <param name="root"></param>
        /// <param name="layer"></param>
        private static void SetLayerRecursively(GameObject root, int layer) {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

    }

}
