using UnityEditor;
using UnityEngine;

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
        public const int GameObjectMenuBasePriority = 0;

        [MenuItem(AssetsMenuBasePath, false, AssetsMenuBasePriority)]
        public static void AssetsMenu()
        {

        }

        [MenuItem(GameObjectMenuBasePath, false, GameObjectMenuBasePriority)]
        public static void GameObjectMenu()
        {

        }

        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(string header, string message, Object context = null)
        {
            Debug.Log(string.IsNullOrEmpty(header) ? $"<color={LogColor.LogBase}>{message}</color>\n\n\n" : $"<color={LogColor.LogBase}>[{header}] {message}</color>\n\n\n", context);
        }

        /// <summary>
        /// A variant of Debug.Log that logs a warning message to the console.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogWarning(string header, string message, Object context = null)
        {
            Debug.LogWarning(string.IsNullOrEmpty(header) ? $"<color={LogColor.WarningBase}>{message}</color>\n\n\n" : $"<color={LogColor.WarningBase}>[{header}] {message}</color>\n\n\n", context);
        }

    }

    public static class LogColor
    {

        private static class Color
        {
            public const string Blue = "blue";
            public const string White = "white";
            public const string Yellow = "yellow";
            public const string Red = "red";
        }

        public const string LogBase = Color.Blue;

        public const string WarningBase = Color.Blue;

        public const string Key = Color.White;

        public const string Value = Color.Yellow;

    }

}
