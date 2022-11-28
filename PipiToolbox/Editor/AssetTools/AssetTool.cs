using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 资源工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221128</version>
    public static class AssetTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "Asset Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 100;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "Asset";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        [MenuItem(MenuPath + "Reimport", false, MenuPriority)]
        private static void Menu_Reimport()
        {
            AssetUtility.ReimportAsset(Selection.activeObject);
        }
        
    }

}
