using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// Sprite 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221222</version>
    public static class SpriteTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "Sprite Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 130;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "Sprite";

        /// <summary>
        /// 批量设置 Sprite 资源的 Packing Tag
        /// </summary>
        [MenuItem(k_MenuPath + "Set Sprite Packing Tag", false, k_MenuPriority)]
        private static void Menu_SetSpritePackingTag()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            InputDialogWindow inputDialog = InputDialogWindow.Create("New Packing Tag");
            inputDialog.inputContent = TextureUtility.GetSpritePackingTag(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]));
#pragma warning disable CS4014
            inputDialog.confirmCallback = (s) => SetSpritePackingTagByGUIDs(assetGUIDs, s);
#pragma warning restore CS4014
        }

        /// <summary>
        /// 移除 Sprite 资源的 Packing Tag
        /// </summary>
        [MenuItem(k_MenuPath + "Remove Sprite Packing Tag", false, k_MenuPriority)]
        private static async void Menu_RemoveSpritePackingTag()
        {
            await SetSpritePackingTagByGUIDs(Selection.assetGUIDs, "");
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
            string[] paths = AssetDatabase.IsValidFolder(assetPath) ? AssetUtility.GetAssetsAtPath(assetPath) : new[] { assetPath };
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
            PipiToolboxUtility.LogSuccess(k_LogTag, $"Set Sprite Packing Tag: <color={LogColor.White}>{assetPath}</color> => <color={LogColor.Yellow}>{packingTag}</color>", assetImporter);
        }

    }

}
