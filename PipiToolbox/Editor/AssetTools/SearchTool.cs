using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 搜索工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221129</version>
    public static class SearchTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "Search/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 1001;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "SearchTool";

        [MenuItem(MenuPath + "Find Asset By GUID", false, MenuPriority)]
        private static void Menu_FindAssetByGUID()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by GUID");
            inputDialog.description = "Enter the GUID of asset:";
            void InputDialogConfirmCallback(string input)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(input);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    Selection.activeObject = asset;
                    PipiToolbox.LogSuccess(LogHeader, $"Asset found!", asset);
                    PipiToolbox.LogSuccess(LogHeader, $"GUID: <color=yellow>{input}</color>", asset);
                    PipiToolbox.LogSuccess(LogHeader, $"Path: <color=yellow>{assetPath}</color>", asset);
                }
                else
                {
                    PipiToolbox.LogWarning(LogHeader, $"There is no asset with GUID: <color=yellow>{input}</color>");
                }
            }
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

        [MenuItem(MenuPath + "Find Asset By Path", false, MenuPriority)]
        private static void Menu_FindAssetByPath()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by path");
            inputDialog.description = "Enter the path of asset:";
            void InputDialogConfirmCallback(string input)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(input);
                if (asset)
                {
                    Selection.activeObject = asset;
                    PipiToolbox.LogSuccess(LogHeader, $"Asset found!", asset);
                    PipiToolbox.LogSuccess(LogHeader, $"Path: <color=yellow>{input}</color>", asset);
                    PipiToolbox.LogSuccess(LogHeader, $"GUID: <color=yellow>{AssetDatabase.AssetPathToGUID(input)}</color>", asset);
                }
                else
                {
                    PipiToolbox.LogWarning(LogHeader, $"There is no asset with Path: <color=yellow>{input}</color>");
                }
            }
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

    }

}
