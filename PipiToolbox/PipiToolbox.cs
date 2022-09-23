using UnityEditor;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 皮皮工具箱
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220924</version>
    public static class PipiToolbox
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        public const string BaseMenuPath = "Assets/Pipi Toolbox 😼🧰/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        public const int BaseMenuPriority = 8;

        [MenuItem(BaseMenuPath, false, BaseMenuPriority)]
        public static void Menu()
        {

        }

    }

}
