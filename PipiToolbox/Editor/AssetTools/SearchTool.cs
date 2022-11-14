using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 搜索工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221027</version>
    public static class SearchTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.BaseMenuPath + "Search Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.BaseMenuPriority + 51;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "SearchTool";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        [MenuItem(MenuPath + "Find Asset By GUID", false, MenuPriority)]
        private static void Menu_FindByGUID()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by GUID");
            inputDialog.description = "Enter the GUID:";
            void InputDialogConfirmCallback(string input)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(input);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    Selection.activeObject = asset;
                    Debug.Log($"[{LogHeader}] Asset found! GUID: <color={LogKeyColor}>{input}</color> => Path: <color={LogValueColor}>{assetPath}</color>\n\n\n", asset);
                }
                else
                {
                    Debug.LogWarning($"[{LogHeader}] There is no asset with GUID: <color={LogValueColor}>{input}</color>");
                }
            }
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

        [MenuItem(MenuPath + "Find Asset By Path", false, MenuPriority)]
        private static void Menu_FindByPath()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by path");
            inputDialog.description = "Enter the path:";
            void InputDialogConfirmCallback(string input)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(input);
                if (asset)
                {
                    Selection.activeObject = asset;
                    Debug.Log($"[{LogHeader}] Asset found! Path: <color={LogKeyColor}>{input}</color>\n\n\n", asset);
                }
                else
                {
                    Debug.LogWarning($"[{LogHeader}] There is no asset with Path: <color={LogValueColor}>{input}</color>");
                }
            }
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

    }

}
