using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 资源信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220923</version>
    public static class AssetInfoUtility
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.BaseMenuPath + "Print Asset Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        public const int MenuPriority = PipiToolbox.BaseMenuPriority;

        /// <summary>
        /// 键颜色
        /// </summary>
        private static string keyColor = "white";

        /// <summary>
        /// 值颜色
        /// </summary>
        private static string valueColor = "yellow";

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
            Debug.Log($"[Asset Info] <color={keyColor}>Name</color>: <color={valueColor}>{name}</color>");
        }

        /// <summary>
        /// 打印类型名称
        /// </summary>
        [MenuItem(MenuPath + "Type Name", false, MenuPriority)]
        public static void PrintTypeName()
        {
            string name = Selection.activeObject.GetType().Name;
            Debug.Log($"[Asset Info] <color={keyColor}>Type Name</color>: <color={valueColor}>{name}</color>");
        }

        /// <summary>
        /// 打印相对路径
        /// </summary>
        [MenuItem(MenuPath + "Relative Path", false, MenuPriority)]
        public static void PrintRelativePath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log($"[Asset Info] <color={keyColor}>Relative Path</color>: <color={valueColor}>{path}</color>");
        }

        /// <summary>
        /// 打印绝对路径
        /// </summary>
        [MenuItem(MenuPath + "Absolute Path", false, MenuPriority)]
        public static void PrintAbsolutePath()
        {
            string assetsPath = Application.dataPath;
            assetsPath = assetsPath.Substring(0, assetsPath.LastIndexOf("Assets", StringComparison.Ordinal));
            string path = Path.Combine(assetsPath, AssetDatabase.GetAssetPath(Selection.activeObject));
            Debug.Log($"[Asset Info] <color={keyColor}>Absolute Path</color>: <color={valueColor}>{path}</color>");
        }

        /// <summary>
        /// 打印 GUID 和 File ID
        /// </summary>
        [MenuItem(MenuPath + "GUID and File ID", false, MenuPriority)]
        public static void PrintGUIDAndFileID()
        {
            string guid;
            long localId;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out guid, out localId);
            Debug.Log($"[Asset Info] <color={keyColor}>GUID</color>: <color={valueColor}>{guid}</color>");
            Debug.Log($"[Asset Info] <color={keyColor}>File ID (Local ID, Local Identifier In File)</color>: <color={valueColor}>{localId}</color>");
        }

        /// <summary>
        /// 打印 Instance ID
        /// </summary>
        [MenuItem(MenuPath + "Instance ID", false, MenuPriority)]
        public static void PrintInstanceID()
        {
            int instanceID = Selection.activeObject.GetInstanceID();
            Debug.Log($"[Asset Info] <color={keyColor}>Instance ID</color>: <color={valueColor}>{instanceID}</color>");
        }

        /// <summary>
        /// 打印 AssetBundle 名称
        /// </summary>
        [MenuItem(MenuPath + "AssetBundle Name", false, MenuPriority)]
        public static void PrintAssetBundleName()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string name = AssetDatabase.GetImplicitAssetBundleName(path);
            if (name.Equals(""))
            {
                Debug.Log($"[Asset Info] <color={keyColor}>AssetBundle Name</color>: <color=red><None></color>");
            }
            else
            {
                Debug.Log($"[Asset Info] <color={keyColor}>AssetBundle Name</color>: <color={valueColor}>{name}</color>");
            }
        }

    }

}
