using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// 纹理资源工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221215</version>
    public static class TextureUtility
    {

        /// <summary>
        /// 临时纹理缓存
        /// </summary>
        private static readonly Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// 获取临时纹理
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D GetTemporary(int width, int height)
        {
            string key = $"{width}x{height}";
            // 从缓存中获取
            if (TextureCache.TryGetValue(key, out Texture2D texture))
            {
                return texture;
            }
            // 创建新的纹理
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            TextureCache[key] = texture;
            return texture;
        }

        /// <summary>
        /// 清除纹理缓存
        /// </summary>
        public static void ClearTextureCache()
        {
            TextureCache.Clear();
            EditorUtility.UnloadUnusedAssetsImmediate();
        }

        /// <summary>
        /// 拷贝纹理像素数据
        /// </summary>
        /// <param name="source">源纹理</param>
        /// <param name="dest">目标纹理</param>
        public static void Copy(Texture2D source, Texture2D dest)
        {
            // 获取一个临时的 RenderTexture
            RenderTexture temp = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            // 将源 Texture 的像素数据拷贝到临时的 RenderTexture 中
            Graphics.Blit(source, temp);
            // 保存当前激活的 RenderTexture 的引用
            RenderTexture previous = RenderTexture.active;
            // 将我们的临时 RenderTexture 设为当前激活的 RenderTexture
            RenderTexture.active = temp;
            // 从当前激活的 RenderTexture 中读取像素数据并应用
            dest.ReadPixels(new Rect(0, 0, temp.width, temp.height), 0, 0);
            // 在右侧填充像素
            if (dest.width > source.width)
            {
                int fillWidth = dest.width - source.width;
                Color[] colors = new Color[fillWidth * dest.height];
                dest.SetPixels(source.width, 0, fillWidth, dest.height, colors);
            }
            // 在上方填充像素
            if (dest.height > source.height)
            {
                int fillHeight = dest.height - source.height;
                Color[] colors = new Color[dest.width * fillHeight];
                dest.SetPixels(0, source.height, dest.width, fillHeight, colors);
            }
            // 应用变更
            dest.Apply();
            // 恢复当前激活的 RenderTexture 的引用
            RenderTexture.active = previous;
            // 释放临时的 RenderTexture
            RenderTexture.ReleaseTemporary(temp);
        }

        /// <summary>
        /// 调整纹理尺寸
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static void Resize(Texture2D texture, int width, int height)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            // 获取 TextureImporter
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (!textureImporter) return;
            // 获取一个符合期望尺寸的临时纹理
            Texture2D tempTexture = GetTemporary(width, height);
            // 拷贝源纹理的数据到临时纹理
            Copy(texture, tempTexture);
            // 保存到文件
            byte[] bytes = path.ToLower().Contains(".jpg") ? tempTexture.EncodeToJPG() : tempTexture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            textureImporter.SaveAndReimport();
        }

        /// <summary>
        /// 设置 Sprite 资源的 Packing Tag
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="packingTag">Packing Tag</param>
        public static void SetSpritePackingTag(string assetPath, string packingTag)
        {
            TextureImporter assetImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (assetImporter == null || assetImporter.textureType != TextureImporterType.Sprite)
            {
                return;
            }
            assetImporter.spritePackingTag = packingTag;
            assetImporter.SaveAndReimport();
        }

        /// <summary>
        /// 获取 Sprite 资源的 Packing Tag
        /// </summary>
        public static string GetSpritePackingTag(string assetPath)
        {
            TextureImporter assetImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (assetImporter == null || assetImporter.textureType != TextureImporterType.Sprite)
            {
                return string.Empty;
            }
            return assetImporter.spritePackingTag;
        }

    }

}
