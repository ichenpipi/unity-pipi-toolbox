using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 纹理工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221214</version>
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
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        [MenuItem(MenuPath + "Resize to multiple of 4 (Multi-asset support)", false, MenuPriority)]
        private static void Menu_ResizeToMultipleOf4()
        {
            Object[] assets = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            foreach (Object asset in assets)
            {
                if (!(asset is Texture2D texture)) continue;
                ResizeToMultipleOf4(texture);
            }
        }

        /// <summary>
        /// 调整纹理的尺寸到 4 的倍数
        /// </summary>
        private static void ResizeToMultipleOf4(Texture2D texture)
        {
            // 期望尺寸
            int desiredWidth = Mathf.RoundToInt(texture.width / 4f) * 4;
            int desiredHeight = Mathf.RoundToInt(texture.height / 4f) * 4;
            // 是否需要调整
            if (texture.width == desiredWidth && texture.height == desiredHeight)
            {
                TextureUtility.Resize(texture, desiredWidth, desiredHeight);
            }
        }

    }

}
