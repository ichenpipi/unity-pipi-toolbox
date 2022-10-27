using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 资源信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221017</version>
    public static class AssetInfoTools
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.BaseMenuPath + "Asset Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.BaseMenuPriority + 1;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "AssetInfo";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        /// <summary>
        /// 打印全部信息
        /// </summary>
        [MenuItem(MenuPath + "All", false, MenuPriority)]
        public static void PrintAll()
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
        [MenuItem(MenuPath + "Name", false, MenuPriority)]
        public static void PrintName()
        {
            string name = Selection.activeObject.name;
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>Name</color>: <color={LogValueColor}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印类型名称
        /// </summary>
        [MenuItem(MenuPath + "Type Name", false, MenuPriority)]
        public static void PrintTypeName()
        {
            string name = Selection.activeObject.GetType().Name;
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>Type Name</color>: <color={LogValueColor}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印相对路径
        /// </summary>
        [MenuItem(MenuPath + "Path", false, MenuPriority)]
        public static void PrintRelativePath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>Relative Path</color>: <color={LogValueColor}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印绝对路径
        /// </summary>
        [MenuItem(MenuPath + "Absolute Path", false, MenuPriority)]
        public static void PrintAbsolutePath()
        {
            string path = GetAbsolutePath(Selection.activeObject);
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>Absolute Path</color>: <color={LogValueColor}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 GUID 和 File ID
        /// </summary>
        [MenuItem(MenuPath + "GUID and File ID", false, MenuPriority)]
        public static void PrintGUIDAndFileID()
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localId);
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>GUID</color>: <color={LogValueColor}>{guid}</color>", Selection.activeObject);
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>File ID (Local ID, Local Identifier In File)</color>: <color={LogValueColor}>{localId}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 Instance ID
        /// </summary>
        [MenuItem(MenuPath + "Instance ID", false, MenuPriority)]
        public static void PrintInstanceID()
        {
            int instanceID = Selection.activeObject.GetInstanceID();
            Debug.Log($"[{LogHeader}] <color={LogKeyColor}>Instance ID</color>: <color={LogValueColor}>{instanceID}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "AssetBundle Name", false, MenuPriority)]
        public static void PrintAssetBundleName()
        {
            string name = GetAssetBundleName(Selection.activeObject);
            if (string.IsNullOrEmpty(name))
            {
                Debug.Log($"[{LogHeader}] <color={LogKeyColor}>AssetBundle Name</color>: <color=red><None></color>", Selection.activeObject);
            }
            else
            {
                Debug.Log($"[{LogHeader}] <color={LogKeyColor}>AssetBundle Name</color>: <color={LogValueColor}>{name}</color>", Selection.activeObject);
            }
        }

        /// <summary>
        /// 复制相对路径到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy Path", false, MenuPriority + 11)]
        public static void CopyRelativePath()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            GUIUtility.systemCopyBuffer = Join(list, ", ");
        }

        /// <summary>
        /// 复制 AssetBundle 名称到系统剪切板
        /// </summary>
        [MenuItem(MenuPath + "Copy AssetBundle Name", false, MenuPriority + 11)]
        public static void CopyAssetBundleName()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                list.Add(GetAssetBundleName(asset));
            }
            GUIUtility.systemCopyBuffer = Join(list, ", ");
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

    }

}
