using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// SpriteAtlas 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230809</version>
    public static class SpriteAtlasTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "SpriteAtlas Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 54;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "SpriteAtlas";

        /// <summary>
        /// 存放 SpriteAtlas 资源的路径（基于项目的 Assets 目录）
        /// </summary>
        private const string k_SpriteAtlasFolderName = "";

        /// <summary>
        /// 存放 SpriteAtlas 资源的路径（绝对路径）
        /// </summary>
        private static readonly string s_SpriteAtlasFolderPath = Path.Combine(Application.dataPath, k_SpriteAtlasFolderName);

        /// <summary>
        /// 创建新的 SpriteAtlas
        /// </summary>
        [MenuItem(k_MenuPath + "Create New SpriteAtlas", false, k_MenuPriority)]
        private static void Menu_CreateNewSpriteAtlas()
        {
            // 创建 SpriteAtlas
            string path = PickNewSpriteAtlasPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            path = AssetUtility.ToRelativePath(path);
            SpriteAtlas spriteAtlas = CreateSpriteAtlas(path);

            // 获取选中的 Sprite
            Sprite[] sprites = GetSpritesInSelection();
            // 添加 Sprite
            if (sprites.Length > 0)
            {
                AddSpritesToSpriteAtlas(spriteAtlas, sprites);
            }
        }

        /// <summary>
        /// 添加当前选中的 Sprite 资源到已有的 SpriteAtlas
        /// </summary>
        [MenuItem(k_MenuPath + "Add Selection To Existing SpriteAtlas", false, k_MenuPriority)]
        private static void Menu_AddSelectionToExistingSpriteAtlas()
        {
            // 获取选中的 Sprite
            Sprite[] sprites = GetSpritesInSelection();
            if (sprites.Length == 0)
            {
                PipiToolboxUtil.LogWarning(k_LogTag, $"There are no sprite assets in the current selection.");
                return;
            }
            // 选择已有的图集并添加
            SpriteAtlas spriteAtlas = PickExistingSpriteAtlas();
            if (spriteAtlas)
            {
                AddSpritesToSpriteAtlas(spriteAtlas, sprites);
            }
        }

        /// <summary>
        /// 添加 Sprite 到 SpriteAtlas
        /// </summary>
        /// <param name="spriteAtlas"></param>
        /// <param name="sprites"></param>
        public static async void AddSpritesToSpriteAtlas(SpriteAtlas spriteAtlas, Sprite[] sprites)
        {
            if (sprites.Length == 0) return;
            // 图集路径
            string spriteAtlasPath = AssetDatabase.GetAssetPath(spriteAtlas);
            // 开始添加
            int totalCount = sprites.Length;
            int addedCount = 0;
            for (int i = 0; i < totalCount; i++)
            {
                Sprite sprite = sprites[i];
                // 展示进度
                string title = $"Adding sprites to SpriteAtlas... ({i + 1}/{totalCount})";
                string spritePath = AssetDatabase.GetAssetPath(sprite);
                float progress = (float)(i + 1) / totalCount;
                bool hasCanceled = EditorUtility.DisplayCancelableProgressBar(title, spritePath, progress);
                // 延迟
                await Task.Delay(1);
                // 是否已取消
                if (hasCanceled) break;
                // 是否能够添加
                if (spriteAtlas.GetSprite(sprite.name))
                {
                    PipiToolboxUtil.LogWarning(k_LogTag, $"SpriteAtlas '{spriteAtlas.name}' already has a sprite named '{sprite.name}', skipped!");
                    continue;
                }
                // 添加到图集
                spriteAtlas.Add(new Object[] { sprite });
                addedCount++;
                PipiToolboxUtil.LogSuccess(k_LogTag, $"Added to SpriteAtlas: <color={LogColor.White}>{spritePath}</color> => <color={LogColor.Yellow}>{spriteAtlasPath}</color>", sprite);
            }
            EditorUtility.ClearProgressBar();
            // 保存
            if (addedCount > 0)
            {
                AssetDatabase.SaveAssets();
                PipiToolboxUtil.LogSuccess(k_LogTag, $"SpriteAtlas Updated: <color={LogColor.Yellow}>{spriteAtlasPath}</color>", spriteAtlas);
            }
        }

        /// <summary>
        /// 获取选中的 Sprite 资源
        /// </summary>
        /// <returns></returns>
        private static Sprite[] GetSpritesInSelection()
        {
            Object[] assets = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
            List<Sprite> sprites = new List<Sprite>();
            foreach (Object asset in assets)
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                if (sprite) sprites.Add(sprite);
            }
            return sprites.ToArray();
        }

        /// <summary>
        /// 创建 SpriteAtlas 资源
        /// </summary>
        /// <param name="path">相对路径</param>
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
            };
            spriteAtlas.SetPlatformSettings(platformSettings);
            // 添加 Sprite
            if (sprites != null) spriteAtlas.Add(sprites);
            // 创建资源文件
            AssetDatabase.CreateAsset(spriteAtlas, path);
            PipiToolboxUtil.LogSuccess(k_LogTag, $"New SpriteAtlas Created: <color={LogColor.Yellow}>{path}</color>", spriteAtlas);
            return spriteAtlas;
        }

        /// <summary>
        /// 选择已有的 SpriteAtlas
        /// </summary>
        /// <returns></returns>
        private static SpriteAtlas PickExistingSpriteAtlas()
        {
            string path = PickExistingSpriteAtlasPath();
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            path = AssetUtility.ToRelativePath(path);
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
        }

        /// <summary>
        /// 选择已有的 SpriteAtlas 路径
        /// </summary>
        /// <returns>绝对路径</returns>
        private static string PickExistingSpriteAtlasPath()
        {
            string directory = s_SpriteAtlasFolderPath;
            const string title = "Select an existing SpriteAtlas";
            const string extension = "spriteatlas";
            return EditorUtility.OpenFilePanel(title, directory, extension);
        }

        /// <summary>
        /// 选择新建 SpriteAtlas 的路径
        /// </summary>
        /// <returns>绝对路径</returns>
        private static string PickNewSpriteAtlasPath()
        {
            string directory = s_SpriteAtlasFolderPath;
            const string title = "New SpriteAtlas";
            const string defaultName = "NewSpriteAtlas";
            const string extension = "spriteatlas";
            return EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
        }

    }

}
