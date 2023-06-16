using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// 纹理工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230116</version>
    public static class TextureTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "Texture Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 140;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "Texture";

        /// <summary>
        /// 尺寸调整模式
        /// </summary>
        public enum ResizeMode
        {
            /// <summary>
            /// 扩充
            /// </summary>
            Expand = 1,

            /// <summary>
            /// 缩减
            /// </summary>
            Shrink = 2,
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        [MenuItem(k_MenuPath + "Resize to Multiple of 4 (Multi-asset support)/Expand (Ceil)", false, k_MenuPriority)]
        private static void Menu_ResizeToMultipleOf4_Expand()
        {
            Selection_ResizeToMultipleOf4(ResizeMode.Expand);
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        [MenuItem(k_MenuPath + "Resize to Multiple of 4 (Multi-asset support)/Shrink (Floor)", false, k_MenuPriority)]
        private static void Menu_ResizeToMultipleOf4_Shrink()
        {
            Selection_ResizeToMultipleOf4(ResizeMode.Shrink);
        }

        /// <summary>
        /// 调整纹理的尺寸到 2 次幂
        /// </summary>
        [MenuItem(k_MenuPath + "Resize to Power of 2 (POT) (Multi-asset support)/Expand (Ceil)", false, k_MenuPriority)]
        private static void Menu_ResizeToPowerOf2_Expand()
        {
            Selection_ResizeToPowerOf2(ResizeMode.Expand);
        }

        /// <summary>
        /// 调整纹理的尺寸到 2 次幂
        /// </summary>
        [MenuItem(k_MenuPath + "Resize to Power of 2 (POT) (Multi-asset support)/Shrink (Floor)", false, k_MenuPriority)]
        private static void Menu_ResizeToPowerOf2_Shrink()
        {
            Selection_ResizeToPowerOf2(ResizeMode.Shrink);
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        private static void Selection_ResizeToMultipleOf4(ResizeMode resizeMode)
        {
            Object[] assets = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            foreach (Object asset in assets)
            {
                if (!(asset is Texture2D texture)) continue;
                ResizeToMultipleOf4(texture, resizeMode);
            }
        }

        /// <summary>
        /// 调整纹理的尺寸到 2 次幂
        /// </summary>
        private static void Selection_ResizeToPowerOf2(ResizeMode resizeMode)
        {
            Object[] assets = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            foreach (Object asset in assets)
            {
                if (!(asset is Texture2D texture)) continue;
                ResizeToPowerOf2(texture, resizeMode);
            }
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="resizeMode">调整模式</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static void ResizeToMultipleOf4(Texture2D texture, ResizeMode resizeMode)
        {
            // 原尺寸
            int originalWidth = texture.width, originalHeight = texture.height;
            // 计算期望尺寸
            int desiredWidth, desiredHeight;
            switch (resizeMode)
            {
                case ResizeMode.Expand:
                    desiredWidth = Mathf.CeilToInt(originalWidth / 4f) * 4;
                    desiredHeight = Mathf.CeilToInt(originalHeight / 4f) * 4;
                    break;
                case ResizeMode.Shrink:
                    desiredWidth = Mathf.FloorToInt(originalWidth / 4f) * 4;
                    desiredHeight = Mathf.FloorToInt(originalHeight / 4f) * 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resizeMode), resizeMode, null);
            }
            // 调整
            Resize(texture, desiredWidth, desiredHeight);
        }

        /// <summary>
        /// 调整纹理的尺寸到 2 次幂
        /// <param name="texture">纹理</param>
        /// <param name="resizeMode">调整模式</param>
        /// </summary>
        private static void ResizeToPowerOf2(Texture2D texture, ResizeMode resizeMode)
        {
            // 原尺寸
            int originalWidth = texture.width, originalHeight = texture.height;
            // 计算期望尺寸
            int desiredWidth, desiredHeight;
            switch (resizeMode)
            {
                case ResizeMode.Expand:
                    desiredWidth = (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(originalWidth) / Mathf.Log(2)));
                    desiredHeight = (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(originalHeight) / Mathf.Log(2)));
                    break;
                case ResizeMode.Shrink:
                    desiredWidth = (int) Mathf.Pow(2, Mathf.Floor(Mathf.Log(originalWidth) / Mathf.Log(2)));
                    desiredHeight = (int) Mathf.Pow(2, Mathf.Floor(Mathf.Log(originalHeight) / Mathf.Log(2)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resizeMode), resizeMode, null);
            }
            // 调整
            Resize(texture, desiredWidth, desiredHeight);
        }

        /// <summary>
        /// 调整纹理尺寸
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        private static void Resize(Texture2D texture, int width, int height)
        {
            int originalWidth = texture.width, originalHeight = texture.height;
            if (originalWidth == width && originalHeight == height)
            {
                PipiToolboxUtility.LogWarning(k_LogTag, $"This texture does not need to be resized! Asset path: <color={LogColor.Yellow}>{AssetDatabase.GetAssetPath(texture)}</color>", texture);
                return;
            }
            TextureUtility.Resize(texture, width, height);
            PipiToolboxUtility.LogSuccess(k_LogTag, $"Resized texture from <color={LogColor.White}>{originalWidth}x{originalHeight}</color> to <color={LogColor.Yellow}>{width}x{height}</color>! Asset path: <color={LogColor.Yellow}>{AssetDatabase.GetAssetPath(texture)}</color>", texture);
        }

    }

}
