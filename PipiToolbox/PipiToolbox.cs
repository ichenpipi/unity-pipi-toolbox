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
        /// 资源菜单项路径
        /// </summary>
        public const string AssetsMenuBasePath = "Assets/Pipi Toolbox 😼/";

        /// <summary>
        /// 资源菜单项优先级
        /// </summary>
        public const int AssetsMenuBasePriority = 8;

        /// <summary>
        /// 节点菜单项路径
        /// </summary>
        public const string GameObjectMenuBasePath = "GameObject/Pipi Toolbox 😼/";

        /// <summary>
        /// 节点菜单项优先级
        /// </summary>
        public const int GameObjectMenuBasePriority = 11;

        [MenuItem(AssetsMenuBasePath, false, AssetsMenuBasePriority)]
        public static void AssetsMenu()
        {

        }

        [MenuItem(GameObjectMenuBasePath, false, GameObjectMenuBasePriority)]
        public static void GameObjectMenu()
        {

        }

    }

}
