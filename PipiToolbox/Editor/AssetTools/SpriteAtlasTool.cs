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
    public static class SpriteAtlasTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "SpriteAtlas Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 120;

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
        /// 存放 SpriteAtlas 资源的路径（基于项目的 Assets 目录）
        /// </summary>
        private const string SpriteAtlasFolderName = "";

        /// <summary>
        /// 存放 SpriteAtlas 资源的路径（绝对路径）
        /// </summary>
        private static readonly string SpriteAtlasFolderPath = Path.Combine(Application.dataPath, SpriteAtlasFolderName);

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath + "Create SpriteAtlas With Selection", false, MenuPriority)]
        private static void Menu_CreateSpriteAtlasWithSelection()
        {
            // 获取选中的 Sprite
            Sprite[] sprites = GetAllSpritesInSelection();
            if (sprites.Length == 0) return;
        }

        // /// <summary>
        // /// 
        // /// </summary>
        // [MenuItem(MenuPath + "Create SpriteAtlas Based On Sprite Tag", false, MenuPriority)]
        // private static void Menu_CreateSpriteAtlasBasedOnSpriteTag()
        // {
        //
        // }

        /// <summary>
        /// 添加当前选中的 Sprite 资源到已有的 SpriteAtlas
        /// </summary>
        [MenuItem(MenuPath + "Add Selection To Existing SpriteAtlas", false, MenuPriority)]
        private static void Menu_AddSelectionToExistingSpriteAtlas()
        {
            // 获取选中的 Sprite
            Sprite[] sprites = GetAllSpritesInSelection();
            if (sprites.Length == 0)
            {
                Debug.Log($"[{LogHeader}] No sprite asset found at current selection.");
                return;
            }
            // 选择已有的图集
            SpriteAtlas spriteAtlas = PickExistingSpriteAtlas();
            // 添加
            AddSpritesToSpriteAtlas(spriteAtlas, sprites);
        }

        /// <summary>
        /// 添加 Sprite 到 SpriteAtlas
        /// </summary>
        /// <param name="spriteAtlas"></param>
        /// <param name="sprites"></param>
        public static async void AddSpritesToSpriteAtlas(SpriteAtlas spriteAtlas, Sprite[] sprites)
        {
            if (sprites.Length == 0)
            {
                Debug.Log($"[{LogHeader}] sprite array is empty, skip adding.");
                return;
            }
            // 图集路径
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
                await Task.Delay(1);
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
            string[] guids = Selection.assetGUIDs;
            List<Sprite> sprites = new List<Sprite>();
            foreach (string guid in guids)
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
            const string extension = "spriteatlas";
            string directory = SpriteAtlasFolderPath;
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
