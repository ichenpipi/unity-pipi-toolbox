using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// 皮皮工具箱
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230516</version>
    internal static class PipiToolboxUtil
    {

        #region Clipboard

        internal static void SaveToClipboard(string content)
        {
            GUIUtility.systemCopyBuffer = content;
        }

        internal static string GetClipboardContent()
        {
            return GUIUtility.systemCopyBuffer;
        }

        #endregion

        #region Asset

        internal static Object GUIDToAsset(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath)) return null;
            return AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        }

        internal static void PingAsset(string guid)
        {
            Object asset = GUIDToAsset(guid);
            if (!asset) return;
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(asset);
        }

        internal static void FocusOnAsset(string guid)
        {
            Object asset = GUIDToAsset(guid);
            if (!asset) return;
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            EditorApplication.delayCall += () => EditorGUIUtility.PingObject(asset);
        }

        #endregion

        #region Log

        internal static void LogNormal(string tag, string message, Object context = null)
        {
            if (!string.IsNullOrEmpty(tag)) tag = $"[{tag}] ";
            Debug.Log($"<color={LogColor.NormalBase}>{tag}{message}</color>\n\n\n", context);
        }

        internal static void LogSuccess(string tag, string message, Object context = null)
        {
            if (!string.IsNullOrEmpty(tag)) tag = $"[{tag}] ";
            Debug.Log($"<color={LogColor.SuccessBase}>{tag}{message}</color>\n\n\n", context);
        }

        internal static void LogWarning(string tag, string message, Object context = null)
        {
            if (!string.IsNullOrEmpty(tag)) tag = $"[{tag}] ";
            Debug.LogWarning($"<color={LogColor.WarningBase}>{tag}{message}</color>\n\n\n", context);
        }

        #endregion

    }

    #region Log Color

    internal static class LogColor
    {

        internal const string NormalBase = Cyan;
        internal const string WarningBase = Orange;
        internal const string SuccessBase = Green;

        internal const string White = "#FFFFFF";
        internal const string Orange = "orange";
        internal const string Red = "#FF0000";
        internal const string Green = "#00FF00";
        internal const string Blue = "#0000FF";
        internal const string Yellow = "yellow";
        internal const string Cyan = "#00FFFF";
        internal const string Magenta = "#FF00FF";

        internal static string Colour(string color, string content)
        {
            return $"<color={color}>{content}</color>";
        }

    }

    #endregion

}
