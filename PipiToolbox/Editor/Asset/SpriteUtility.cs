using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// Sprite 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220927</version>
    public static class SpriteUtility
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.BaseMenuPath + "Sprite Utility/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.BaseMenuPriority + 21;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "Sprite";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        /// <summary>
        /// 批量设置 Sprite 资源的 Packing Tag
        /// </summary>
        [MenuItem(MenuPath + "Batch Setting Sprite Packing Tag", false, MenuPriority)]
        private static void Menu_BatchSettingSpritePackingTag()
        {
            InputDialogWindow inputDialog = InputDialogWindow.Create("New Packing Tag");
            inputDialog.confirmCallback = s => SetSpritePackingTagByGUIDs(Selection.assetGUIDs, s);
        }

        /// <summary>
        /// 设置 Sprite 资源的 Packing Tag
        /// </summary>
        /// <param name="guids">GUID</param>
        /// <param name="packingTag">Packing Tag</param>
        public static async Task SetSpritePackingTagByGUIDs(IEnumerable<string> guids, string packingTag)
        {
            foreach (string guid in guids)
            {
                await SetSpritePackingTag(AssetDatabase.GUIDToAssetPath(guid), packingTag);
            }
        }

        /// <summary>
        /// 设置 Sprite 资源的 Packing Tag
        /// </summary>
        /// <param name="assetPaths">资源路径</param>
        /// <param name="packingTag">Packing Tag</param>
        public static async Task SetSpritePackingTag(IEnumerable<string> assetPaths, string packingTag)
        {
            foreach (string path in assetPaths)
            {
                await SetSpritePackingTag(path, packingTag);
            }
        }

        /// <summary>
        /// 设置 Sprite 资源的 Packing Tag
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="packingTag">Packing Tag</param>
        public static async Task SetSpritePackingTag(string assetPath, string packingTag)
        {
            // 获取资源路径
            string[] paths = AssetDatabase.IsValidFolder(assetPath) ? GetAssetsAtPath(assetPath) : new[] { assetPath };
            // 遍历处理
            int totalCount = paths.Length;
            for (int i = 0; i < totalCount; i++)
            {
                string path = paths[i];
                // 展示进度
                string title = $"Setting Sprite Packing Tag... ({i + 1}/{totalCount})";
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
                SetPackingTag(path, packingTag);
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 设置 Sprite 资源的 Packing Tag
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="packingTag">Packing Tag</param>
        private static void SetPackingTag(string assetPath, string packingTag)
        {
            TextureImporter assetImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (assetImporter == null || assetImporter.textureType != TextureImporterType.Sprite)
            {
                return;
            }
            assetImporter.spritePackingTag = packingTag;
            assetImporter.SaveAndReimport();
            Debug.Log($"[{LogHeader}] Set Sprite Packing Tag: <color={LogKeyColor}>{assetPath}</color> => <color={LogValueColor}>{packingTag}</color>", assetImporter);
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
