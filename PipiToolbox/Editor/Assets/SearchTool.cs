using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox
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
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "Search/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 1001;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "SearchTool";

        [MenuItem(k_MenuPath + "Find Asset By GUID", false, k_MenuPriority)]
        private static void Menu_FindAssetByGUID()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by GUID");
            inputDialog.description = "Enter the GUID of asset:";
            inputDialog.inputContent = PipiToolboxUtility.GetClipboardContent();

            void InputDialogConfirmCallback(string input)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(input);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    Selection.activeObject = asset;
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"Asset found!", asset);
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"GUID: <color={LogColor.Yellow}>{input}</color>", asset);
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"Path: <color={LogColor.Yellow}>{assetPath}</color>", asset);
                }
                else
                {
                    PipiToolboxUtility.LogWarning(k_LogTag, $"There is no asset with GUID: <color={LogColor.Yellow}>{input}</color>");
                }
            }

            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

        [MenuItem(k_MenuPath + "Find Asset By Path", false, k_MenuPriority)]
        private static void Menu_FindAssetByPath()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("Find asset by path");
            inputDialog.description = "Enter the path of asset:";
            inputDialog.inputContent = PipiToolboxUtility.GetClipboardContent();

            void InputDialogConfirmCallback(string input)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(input);
                if (asset)
                {
                    Selection.activeObject = asset;
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"Asset found!", asset);
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"Path: <color={LogColor.Yellow}>{input}</color>", asset);
                    PipiToolboxUtility.LogSuccess(k_LogTag, $"GUID: <color={LogColor.Yellow}>{AssetDatabase.AssetPathToGUID(input)}</color>", asset);
                }
                else
                {
                    PipiToolboxUtility.LogWarning(k_LogTag, $"There is no asset with Path: <color={LogColor.Yellow}>{input}</color>");
                }
            }

            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

    }

}
