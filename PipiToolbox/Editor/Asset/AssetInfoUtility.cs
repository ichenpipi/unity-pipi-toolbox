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
    /// <version>20220905</version>
    public static class AssetInfoUtility
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = "Print Asset Info/";

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
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "All", false, 4)]
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
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "Name", false, 4)]
        public static void PrintName()
        {
            string name = Selection.activeObject.name;
            Debug.Log($"[AssetInfo] <color={keyColor}>Name</color>: <color={valueColor}>{name}</color>");
        }

        /// <summary>
        /// 打印类型名称
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "Type Name", false, 4)]
        public static void PrintTypeName()
        {
            string name = Selection.activeObject.GetType().Name;
            Debug.Log($"[AssetInfo] <color={keyColor}>Type Name</color>: <color={valueColor}>{name}</color>");
        }

        /// <summary>
        /// 打印相对路径
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "Relative Path", false, 4)]
        public static void PrintRelativePath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log($"[AssetInfo] <color={keyColor}>Relative Path</color>: <color={valueColor}>{path}</color>");
        }

        /// <summary>
        /// 打印绝对路径
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "Absolute Path", false, 4)]
        public static void PrintAbsolutePath()
        {
            string assetsPath = Application.dataPath;
            assetsPath = assetsPath.Substring(0, assetsPath.LastIndexOf("Assets", StringComparison.Ordinal));
            string path = Path.Combine(assetsPath, AssetDatabase.GetAssetPath(Selection.activeObject));
            Debug.Log($"[AssetInfo] <color={keyColor}>Absolute Path</color>: <color={valueColor}>{path}</color>");
        }

        /// <summary>
        /// 打印 GUID 和 File ID
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "GUID and File ID", false, 4)]
        public static void PrintGUIDAndFileID()
        {
            string guid;
            long localId;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out guid, out localId);
            Debug.Log($"[AssetInfo] <color={keyColor}>GUID</color>: <color={valueColor}>{guid}</color>");
            Debug.Log($"[AssetInfo] <color={keyColor}>File ID (Local ID, Local Identifier In File)</color>: <color={valueColor}>{localId}</color>");
        }

        /// <summary>
        /// 打印 Instance ID
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "Instance ID", false, 4)]
        public static void PrintInstanceID()
        {
            int instanceID = Selection.activeObject.GetInstanceID();
            Debug.Log($"[AssetInfo] <color={keyColor}>Instance ID</color>: <color={valueColor}>{instanceID}</color>");
        }

        /// <summary>
        /// 打印 AssetBundle 名称
        /// </summary>
        [MenuItem(PipiToolbox.BaseMenuPath + MenuPath + "AssetBundle Name", false, 4)]
        public static void PrintAssetBundleName()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string name = AssetDatabase.GetImplicitAssetBundleName(path);
            if (name.Equals(""))
            {
                Debug.Log($"[AssetInfo] <color={keyColor}>AssetBundle Name</color>: <color=red><None></color>");
            }
            else
            {
                Debug.Log($"[AssetInfo] <color={keyColor}>AssetBundle Name</color>: <color={valueColor}>{name}</color>");
            }
        }

    }

}
