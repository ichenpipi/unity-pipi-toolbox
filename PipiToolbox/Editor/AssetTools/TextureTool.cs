using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 纹理工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221215</version>
    public static class TextureTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "Texture Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 140;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "Texture";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

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
        [MenuItem(MenuPath + "Resize to multiple of 4 (Multi-asset support)/Expand (Ceil)", false, MenuPriority)]
        private static void Menu_ResizeToMultipleOf4_Expand()
        {
            Selection_ResizeToMultipleOf4(ResizeMode.Expand);
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        [MenuItem(MenuPath + "Resize to multiple of 4 (Multi-asset support)/Shrink (Floor)", false, MenuPriority)]
        private static void Menu_ResizeToMultipleOf4_Clip()
        {
            Selection_ResizeToMultipleOf4(ResizeMode.Shrink);
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
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        private static void ResizeToMultipleOf4(Texture2D texture, ResizeMode resizeMode)
        {
            // 计算期望尺寸
            int desiredWidth, desiredHeight;
            switch (resizeMode)
            {
                case ResizeMode.Expand:
                    desiredWidth = Mathf.CeilToInt(texture.width / 4f) * 4;
                    desiredHeight = Mathf.CeilToInt(texture.height / 4f) * 4;
                    break;
                case ResizeMode.Shrink:
                    desiredWidth = Mathf.FloorToInt(texture.width / 4f) * 4;
                    desiredHeight = Mathf.FloorToInt(texture.height / 4f) * 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resizeMode), resizeMode, null);
            }
            // 是否需要调整
            if (texture.width == desiredWidth && texture.height == desiredHeight)
            {
                TextureUtility.Resize(texture, desiredWidth, desiredHeight);
            }
        }

    }

}
