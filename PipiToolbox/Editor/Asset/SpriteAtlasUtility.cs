using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// SpriteAtlas 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220927</version>
    public static class SpriteAtlasUtility
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.BaseMenuPath + "SpriteAtlas Utility/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.BaseMenuPriority + 22;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "SpriteAtlas";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath + "Test", false, MenuPriority)]
        private static void Menu_Test()
        {
            CreateSpriteAtlas("Assets/_zHero/Test/test.spriteatlas");
        }

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath + "Create SpriteAtlas With Selection", false, MenuPriority)]
        private static void Menu_CreateSpriteAtlasWithSelection()
        {
            Sprite[] sprites = GetAllSpritesInSelection();
        }

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath + "Create SpriteAtlas Based On Sprite Tag", false, MenuPriority)]
        private static void Menu_CreateSpriteAtlasBasedOnSpriteTag()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath + "Add Selection To Existing SpriteAtlas", false, MenuPriority)]
        private static async void Menu_AddSelectionToExistingSpriteAtlas()
        {
            // 获取选中的 Sprite
            Sprite[] sprites = GetAllSpritesInSelection();
            if (sprites.Length == 0) return;
            // 选择已有的图集
            SpriteAtlas spriteAtlas = PickExistingSpriteAtlas();
            string spriteAtlasPath = AssetDatabase.GetAssetPath(spriteAtlas);
            // 开始添加
            int totalCount = sprites.Length;
            for (int i = 0; i < totalCount; i++)
            {
                Sprite sprite = sprites[i];
                // 展示进度
                string title = $"Add to SpriteAtlas... ({i + 1}/{totalCount})";
                string spritePath = AssetDatabase.GetAssetPath(sprite);
                float progress = (float)(i + 1) / totalCount;
                bool hasCanceled = EditorUtility.DisplayCancelableProgressBar(title, spritePath, progress);
                // 延迟
                await Task.Delay(100);
                // 是否已取消
                if (hasCanceled) break;
                // 添加到图集
                spriteAtlas.Add(new [] {sprite});
                Debug.Log($"[{LogHeader}] Add to SpriteAtlas: <color={LogKeyColor}>{spritePath}</color> => <color={LogValueColor}>{spriteAtlasPath}</color>", sprite);
            }
            EditorUtility.ClearProgressBar();
            // 保存
            AssetDatabase.SaveAssets();
            Debug.Log($"[{LogHeader}] SpriteAtlas Updated: <color={LogValueColor}>{spriteAtlasPath}</color>", spriteAtlas);
        }

        /// <summary>
        /// 获取选中的 Sprite 资源
        /// </summary>
        /// <returns></returns>
        private static Sprite[] GetAllSpritesInSelection()
        {
            List<Sprite> sprites = new List<Sprite>();
            foreach (string guid in Selection.assetGUIDs)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid));
                if (sprite) sprites.Add(sprite);
            }
            return sprites.ToArray();
        }

        /// <summary>
        /// 创建 SpriteAtlas 资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="sprites">精灵</param>
        /// <returns></returns>
        private static SpriteAtlas CreateSpriteAtlas(string path, Sprite[] sprites = null)
        {
            SpriteAtlas spriteAtlas = new SpriteAtlas();
            // 打包设置
            SpriteAtlasPackingSettings packingSettings = new SpriteAtlasPackingSettings()
            {
                blockOffset = 1,
                enableRotation = false,
                enableTightPacking = false,
                padding = 4,
            };
            spriteAtlas.SetPackingSettings(packingSettings);
            // 纹理设置
            SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
            {
                readable = false,
                generateMipMaps = false,
                sRGB = true,
                filterMode = FilterMode.Bilinear,
            };
            spriteAtlas.SetTextureSettings(textureSettings);
            // 平台设置
            TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings()
            {
                maxTextureSize = 2048,
                format = TextureImporterFormat.Automatic,
                textureCompression = TextureImporterCompression.Compressed,
                // crunchedCompression = true,
                // compressionQuality = 50,
            };
            spriteAtlas.SetPlatformSettings(platformSettings);
            // 添加 Sprite
            if (sprites != null) spriteAtlas.Add(sprites);
            // 创建资源文件
            AssetDatabase.CreateAsset(spriteAtlas, path);
            return spriteAtlas;
        }

        /// <summary>
        /// 获取指定目录下的所有资源的路径
        /// </summary>
        /// <returns></returns>
        private static string[] GetAssetsAtPath(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(p => !p.EndsWith(".meta"))
                .ToArray();
        }

        /// <summary>
        /// 选择已有的 SpriteAtlas
        /// </summary>
        /// <returns></returns>
        private static SpriteAtlas PickExistingSpriteAtlas()
        {
            const string title = "Select an existing SpriteAtlas";
            string directory = Application.dataPath;
            const string extension = "spriteatlas";
            string path = EditorUtility.OpenFilePanel(title, directory, extension);
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(GetAssetRelativePath(path));
        }

        /// <summary>
        /// 获取目标资源的在项目 Assets 目录下的相对路径
        /// </summary>
        /// <param name="absolutePath">资源的绝对路径</param>
        /// <returns></returns>
        private static string GetAssetRelativePath(string absolutePath)
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length).Replace("\\", "/");
        }

    }

}
