using UnityEditor;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// 皮皮工具箱
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230516</version>
    public static class PipiToolboxMenu
    {

        /// <summary>
        /// 资源菜单项路径
        /// </summary>
        internal const string AssetsMenuBasePath = "Assets/Pipi Toolbox 😼/";

        /// <summary>
        /// 资源菜单项优先级
        /// </summary>
        internal const int AssetsMenuBasePriority = 8;

        /// <summary>
        /// 节点菜单项路径
        /// </summary>
        internal const string GameObjectMenuBasePath = "GameObject/Pipi Toolbox 😼/";

        /// <summary>
        /// 节点菜单项优先级
        /// </summary>
        internal const int GameObjectMenuBasePriority = 0;

        [MenuItem(AssetsMenuBasePath, false, AssetsMenuBasePriority)]
        private static void AssetsMenu() { }

        [MenuItem(GameObjectMenuBasePath, false, GameObjectMenuBasePriority)]
        private static void GameObjectMenu() { }

    }

}
