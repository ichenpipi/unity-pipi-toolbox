using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// 皮皮工具箱
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230107</version>
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
            if (!string.IsNullOrEmpty(header)) header = $"[{header}] ";
            Debug.Log($"<color={LogColor.LogBase}>{header}{message}</color>\n\n\n", context);
        }

        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogSuccess(string header, string message, Object context = null)
        {
            if (!string.IsNullOrEmpty(header)) header = $"[{header}] ";
            Debug.Log($"<color={LogColor.SuccessBase}>{header}{message}</color>\n\n\n", context);
        }

        /// <summary>
        /// A variant of Debug.Log that logs a warning message to the console.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogWarning(string header, string message, Object context = null)
        {
            if (!string.IsNullOrEmpty(header)) header = $"[{header}] ";
            Debug.LogWarning($"<color={LogColor.WarningBase}>{header}{message}</color>\n\n\n", context);
        }

    }

    public static class LogColor
    {

        private static class MyColor
        {
            public const string White = "#FFFFFF";
            public const string Orange = "orange";
            public const string Red = "#FF0000";
            public const string Green = "#00FF00";
            public const string Blue = "#0000FF";
            public const string Yellow = "yellow";
            public const string Cyan = "#00FFFF";
            public const string Magenta = "#FF00FF";
        }

        public const string LogBase = MyColor.Cyan;

        public const string WarningBase = MyColor.Orange;

        public const string SuccessBase = MyColor.Green;

        public const string Key = MyColor.White;

        public const string Value = MyColor.Yellow;

    }

}
