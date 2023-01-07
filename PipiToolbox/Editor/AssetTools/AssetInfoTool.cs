using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LZParkInc.Watermelon;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 资源信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230107</version>
    public static class AssetInfoTools
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "Asset Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 1;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "AssetInfo";

        /// <summary>
        /// 打印全部信息
        /// </summary>
        [MenuItem(MenuPath + "Print All", false, MenuPriority)]
        private static void PrintAll()
        {
            PrintName();
            PrintTypeName();
            PrintRelativePath();
            PrintAbsolutePath();
            PrintGUIDAndFileID();
            PrintInstanceID();
            PrintAssetBundleName();
        }

        /// <summary>
        /// 打印名称
        /// </summary>
        [MenuItem(MenuPath + "Print Name", false, MenuPriority)]
        private static void PrintName()
        {
            if (!Selection.activeObject) return;
            string name = Selection.activeObject.name;
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>Name</color>: <color={LogColor.Value}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印类型名称
        /// </summary>
        [MenuItem(MenuPath + "Print Type Name", false, MenuPriority)]
        private static void PrintTypeName()
        {
            if (!Selection.activeObject) return;
            string name = Selection.activeObject.GetType().Name;
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>Type Name</color>: <color={LogColor.Value}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印相对路径
        /// </summary>
        [MenuItem(MenuPath + "Print Path", false, MenuPriority)]
        private static void PrintRelativePath()
        {
            if (!Selection.activeObject) return;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>Relative Path</color>: <color={LogColor.Value}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印绝对路径
        /// </summary>
        [MenuItem(MenuPath + "Print Absolute Path", false, MenuPriority)]
        private static void PrintAbsolutePath()
        {
            if (!Selection.activeObject) return;
            string path = GetAbsolutePath(Selection.activeObject);
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>Absolute Path</color>: <color={LogColor.Value}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 GUID 和 File ID
        /// </summary>
        [MenuItem(MenuPath + "Print GUID and File ID", false, MenuPriority)]
        private static void PrintGUIDAndFileID()
        {
            if (!Selection.activeObject) return;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localId);
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>GUID</color>: <color={LogColor.Value}>{guid}</color>", Selection.activeObject);
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>File ID (Local ID, Local Identifier In File)</color>: <color={LogColor.Value}>{localId}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 Instance ID
        /// </summary>
        [MenuItem(MenuPath + "Print Instance ID", false, MenuPriority)]
        private static void PrintInstanceID()
        {
            if (!Selection.activeObject) return;
            int instanceID = Selection.activeObject.GetInstanceID();
            PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>Instance ID</color>: <color={LogColor.Value}>{instanceID}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "Print AssetBundle Name", false, MenuPriority)]
        private static void PrintAssetBundleName()
        {
            if (!Selection.activeObject) return;
            string name = GetAssetBundleName(Selection.activeObject);
            if (string.IsNullOrEmpty(name))
            {
                PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>AssetBundle Name</color>: <color=red><None></color>", Selection.activeObject);
            }
            else
            {
                PipiToolbox.Log(LogHeader, $"<color={LogColor.Key}>AssetBundle Name</color>: <color={LogColor.Value}>{name}</color>", Selection.activeObject);
            }
        }

        /// <summary>
        /// 复制名称到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy Name", false, MenuPriority + 11)]
        private static void CopyName()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                list.Add(asset.name);
            }
            SaveToClipboard(Join(list, ", "));
        }

        /// <summary>
        /// 复制相对路径到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy Path", false, MenuPriority + 11)]
        private static void CopyRelativePath()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            SaveToClipboard(Join(list, ", "));
        }

        /// <summary>
        /// 复制 GUID 到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy GUID", false, MenuPriority + 11)]
        private static void CopyGUID()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                list.Add(guid);
            }
            SaveToClipboard(Join(list, ", "));
        }

        /// <summary>
        /// 复制 AssetBundle 名称到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy AssetBundle Name", false, MenuPriority + 11)]
        private static void CopyAssetBundleName()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                list.Add(GetAssetBundleName(asset));
            }
            SaveToClipboard(Join(list, ", "));
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static string GetAbsolutePath(Object asset)
        {
            string assetsPath = Application.dataPath;
            assetsPath = assetsPath.Substring(0, assetsPath.LastIndexOf("Assets", StringComparison.Ordinal));
            return Path.Combine(assetsPath, AssetDatabase.GetAssetPath(asset));
        }

        /// <summary>
        /// 获取 AssetBundle 名称
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static string GetAssetBundleName(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            return AssetDatabase.GetImplicitAssetBundleName(path);
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string Join<T>(IList<T> list, string separator = ",")
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; ++i)
            {
                builder.Append(list[i]);
                if (i < list.Count - 1)
                {
                    builder.Append(separator);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 保存内容到系统剪切板
        /// </summary>
        /// <param name="content"></param>
        private static void SaveToClipboard(string content)
        {
            GUIUtility.systemCopyBuffer = content;
        }

    }

}
