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
    /// <version>20220905</version>
    public static class AssetBundleUtility
    {

        /// <summary>
        /// 批量设置 AssetBundle 名称
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + "Batch Setting AssetBundle Name", false, 4)]
        private static void MenuBatchSettingAssetBundleName()
        {
            var inputDialog = InputDialogWindow.Create("New AssetBundle Name");
            inputDialog.SetConfirmCallback(s => SetAssetBundleNameByGUIDs(Selection.assetGUIDs, s));
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
            string[] paths = AssetDatabase.IsValidFolder(assetPath) ? GetAssetsAtPath(assetPath) : new[] {assetPath};
            // 遍历处理
            int totalCount = paths.Length;
            for (int i = 0; i < totalCount; i++)
            {
                string path = paths[i];
                // 展示进度
                string title = $"Setting AssetBundle Name... ({i + 1}/{totalCount})";
                float progress = (float) (i + 1) / totalCount;
                bool hasCanceled = EditorUtility.DisplayCancelableProgressBar(title, path, progress);
                // 延迟
                await Task.Delay(100);
                // 是否取消了
                if (hasCanceled)
                {
                    break;
                }
                // 执行操作
                SetAssetBundleNameAndVariant(path, assetBundleName, null);
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Set the AssetBundle name and variant.
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        /// <param name="assetBundleVariant">AssetBundle 变体</param>
        private static void SetAssetBundleNameAndVariant(string assetPath, string assetBundleName, string assetBundleVariant)
        {
            AssetImporter.GetAtPath(assetPath)?.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
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
