using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
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
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "AssetBundle Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 110;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "AssetBundle";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        /// <summary>
        /// 根据资源的路径设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "Set AssetBundle Name Based On Path", false, MenuPriority)]
        private static async void Menu_SetAssetBundleNameBasedOnPath()
        {
            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
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
        [MenuItem(MenuPath + "Set AssetBundle Name Based On Path (With Extension)", false, MenuPriority)]
        private static async void Menu_SetAssetBundleNameBasedOnPath2()
        {
            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string assetBundleName = assetPath.Substring("Assets/".Length);
                await SetAssetBundleName(assetPath, assetBundleName);
            }
        }

        /// <summary>
        /// 根据资源所在的目录设置资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "Set AssetBundle Name Based On Directory", false, MenuPriority)]
        private static async void Menu_SetAssetBundleNameBasedOnDirectory()
        {
            string[] guids = Selection.assetGUIDs;
            foreach (string guid in guids)
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
        /// 批量设置多个资源的 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "Set AssetBundle Name (Multi-asset support)", false, MenuPriority)]
        private static void Menu_BatchSettingAssetBundleName()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("New AssetBundle Name");
            async void InputDialogConfirmCallback(string s) => await SetAssetBundleNameByGUIDs(Selection.assetGUIDs, s);
            inputDialog.confirmCallback = InputDialogConfirmCallback;
        }

        /// <summary>
        /// 设置资源的 AssetBundle 名称
        /// </summary>
        /// <param name="guids">GUID</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        public static async Task SetAssetBundleNameByGUIDs(IEnumerable<string> guids, string assetBundleName)
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
        public static async Task SetAssetBundleName(IEnumerable<string> assetPaths, string assetBundleName)
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
        public static async Task SetAssetBundleName(string assetPath, string assetBundleName)
        {
            // 获取资源路径
            string[] paths = AssetDatabase.IsValidFolder(assetPath) ? GetAssetsAtPath(assetPath) : new[] { assetPath };
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
                await Task.Delay(1);
                // 是否取消了
                if (hasCanceled)
                {
                    break;
                }
                // 执行操作
                SetAssetBundleNameAndVariant(path, assetBundleName, null);
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Set the AssetBundle name and variant.
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        /// <param name="assetBundleVariant">AssetBundle 变体</param>
        private static void SetAssetBundleNameAndVariant(string assetPath, string assetBundleName, string assetBundleVariant)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            if (assetImporter == null)
            {
                return;
            }
            assetBundleName = assetBundleName.ToLower();
            assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
            assetImporter.SaveAndReimport();
            Debug.Log($"[{LogHeader}] Set AssetBundle Name: <color={LogKeyColor}>{assetPath}</color> => <color={LogValueColor}>{assetBundleName}</color>", assetImporter);
        }

        /// <summary>
        /// 获取指定目录下的所有资源的路径
        /// </summary>
        private static string[] GetAssetsAtPath(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(p => !p.EndsWith(".meta"))
                .ToArray();
        }

    }

}
