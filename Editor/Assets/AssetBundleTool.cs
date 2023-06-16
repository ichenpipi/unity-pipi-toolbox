using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// AssetBundle 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221228</version>
    public static class AssetBundleTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "AssetBundle Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 110;

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority2 = k_MenuPriority + 10;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "AssetBundle";

        /// <summary>
        /// 设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Set AssetBundle Name", false, k_MenuPriority)]
        private static void Menu_SetAssetBundleName()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            InputDialogWindow inputDialog = InputDialogWindow.Create("New AssetBundle Name");
            inputDialog.inputContent = AssetDatabase.GetImplicitAssetBundleName(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]));
#pragma warning disable CS4014
            inputDialog.confirmCallback = (s) => SetAssetBundleNameByGUIDs(assetGUIDs, s);
#pragma warning restore CS4014
        }

        /// <summary>
        /// 根据资源的路径设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Set AssetBundle Name Based On Path", false, k_MenuPriority2)]
        private static async void Menu_SetAssetBundleNameBasedOnPath()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string assetBundleName = assetPath.Substring("Assets/".Length);
                if (assetBundleName.LastIndexOf(".", StringComparison.Ordinal) != -1)
                {
                    assetBundleName = assetBundleName.Substring(0, assetBundleName.LastIndexOf(".", StringComparison.Ordinal));
                }
                await SetAssetBundleName(assetPath, assetBundleName);
            }
        }

        /// <summary>
        /// 根据资源的路径设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Set AssetBundle Name Based On Path (With Extension)", false, k_MenuPriority2)]
        private static async void Menu_SetAssetBundleNameBasedOnPath2()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string assetBundleName = assetPath.Substring("Assets/".Length);
                await SetAssetBundleName(assetPath, assetBundleName);
            }
        }

        /// <summary>
        /// 根据资源所在的目录设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Set AssetBundle Name Based On Directory", false, k_MenuPriority2)]
        private static async void Menu_SetAssetBundleNameBasedOnDirectory()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string directory = Path.GetDirectoryName(assetPath);
                if (directory == null)
                {
                    continue;
                }
                string assetBundleName = directory.Remove(0, 7).ToLower();
                assetBundleName = assetBundleName.Replace("\\", "/");
                await SetAssetBundleName(assetPath, assetBundleName);
            }
        }

        /// <summary>
        /// 移除资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Remove AssetBundle Name", false, k_MenuPriority2)]
        private static async void Menu_RemoveAssetBundleName()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                await SetAssetBundleName(assetPath, string.Empty);
            }
        }

        /// <summary>
        /// 设置资源的 AssetBundle 名称
        /// </summary>
        /// <param name="guids">GUID</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        private static async Task SetAssetBundleNameByGUIDs(IEnumerable<string> guids, string assetBundleName)
        {
            foreach (string guid in guids)
            {
                await SetAssetBundleName(AssetDatabase.GUIDToAssetPath(guid), assetBundleName);
            }
        }

        /// <summary>
        /// 设置资源的 AssetBundle 名称
        /// </summary>
        /// <param name="assetPaths">资源路径</param>
        /// <param name="assetBundleName">新的 AssetBundle 名称</param>
        private static async Task SetAssetBundleName(IEnumerable<string> assetPaths, string assetBundleName)
        {
            foreach (string path in assetPaths)
            {
                await SetAssetBundleName(path, assetBundleName);
            }
        }

        /// <summary>
        /// 设置资源的 AssetBundle 名称
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        private static async Task SetAssetBundleName(string assetPath, string assetBundleName)
        {
            // 获取资源路径
            string[] paths = AssetDatabase.IsValidFolder(assetPath) ? AssetUtility.GetAssetsAtPath(assetPath) : new[] { assetPath };
            // 全小写
            assetBundleName = assetBundleName.ToLower();
            // 遍历处理
            int totalCount = paths.Length;
            for (int i = 0; i < totalCount; i++)
            {
                string path = paths[i];
                // 展示进度
                string title = $"Setting AssetBundle Name... ({i + 1}/{totalCount})";
                float progress = (float)(i + 1) / totalCount;
                bool hasCanceled = EditorUtility.DisplayCancelableProgressBar(title, path, progress);
                // 延迟
                await Task.Delay(0);
                // 是否取消了
                if (hasCanceled)
                {
                    break;
                }
                // 执行操作
                AssetUtility.SetAssetBundleNameAndVariant(path, assetBundleName, null);
                PipiToolboxUtility.LogSuccess(
                    k_LogTag,
                    $"Set AssetBundle Name: <color={LogColor.White}>{assetPath}</color> => <color={LogColor.Yellow}>{assetBundleName}</color>",
                    AssetImporter.GetAtPath(assetPath)
                );
            }
            EditorUtility.ClearProgressBar();
        }

    }

}
