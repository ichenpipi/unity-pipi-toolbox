using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// 资源信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230516</version>
    public static class AssetInfoTools
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "Asset Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 001;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "AssetInfo";

        /// <summary>
        /// 打印全部信息
        /// </summary>
        [MenuItem(k_MenuPath + "Print All", false, k_MenuPriority)]
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
        [MenuItem(k_MenuPath + "Print Name", false, k_MenuPriority)]
        private static void PrintName()
        {
            if (!Selection.activeObject) return;
            string name = Selection.activeObject.name;
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Name: </color><color={LogColor.Yellow}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印类型名称
        /// </summary>
        [MenuItem(k_MenuPath + "Print Type Name", false, k_MenuPriority)]
        private static void PrintTypeName()
        {
            if (!Selection.activeObject) return;
            string name = Selection.activeObject.GetType().Name;
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Type Name: </color><color={LogColor.Yellow}>{name}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印相对路径
        /// </summary>
        [MenuItem(k_MenuPath + "Print Path", false, k_MenuPriority)]
        private static void PrintRelativePath()
        {
            if (!Selection.activeObject) return;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Relative Path: </color><color={LogColor.Yellow}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印绝对路径
        /// </summary>
        [MenuItem(k_MenuPath + "Print Absolute Path", false, k_MenuPriority)]
        private static void PrintAbsolutePath()
        {
            if (!Selection.activeObject) return;
            string path = GetAbsolutePath(Selection.activeObject);
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Absolute Path: </color><color={LogColor.Yellow}>{path}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 GUID 和 File ID
        /// </summary>
        [MenuItem(k_MenuPath + "Print GUID and File ID", false, k_MenuPriority)]
        private static void PrintGUIDAndFileID()
        {
            if (!Selection.activeObject) return;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localId);
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>GUID: </color><color={LogColor.Yellow}>{guid}</color>", Selection.activeObject);
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>File ID (Local ID, Local Identifier In File): </color><color={LogColor.Yellow}>{localId}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 Instance ID
        /// </summary>
        [MenuItem(k_MenuPath + "Print Instance ID", false, k_MenuPriority)]
        private static void PrintInstanceID()
        {
            if (!Selection.activeObject) return;
            int instanceID = Selection.activeObject.GetInstanceID();
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Instance ID: </color><color={LogColor.Yellow}>{instanceID}</color>", Selection.activeObject);
        }

        /// <summary>
        /// 打印 AssetBundle 名称
        /// </summary>
        [MenuItem(k_MenuPath + "Print AssetBundle Name", false, k_MenuPriority)]
        private static void PrintAssetBundleName()
        {
            if (!Selection.activeObject) return;
            string name = GetAssetBundleName(Selection.activeObject);
            if (string.IsNullOrEmpty(name))
            {
                PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>AssetBundle Name: </color><color=red><None></color>", Selection.activeObject);
            }
            else
            {
                PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>AssetBundle Name: </color><color={LogColor.Yellow}>{name}</color>", Selection.activeObject);
            }
        }

        /// <summary>
        /// 复制名称到系统剪切板
        /// </summary>
        [MenuItem(k_MenuPath + "Copy Name", false, k_MenuPriority + 11)]
        private static void CopyName()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                list.Add(asset.name);
            }
            PipiToolboxUtility.SaveToClipboard(list.Join(", "));
        }

        /// <summary>
        /// 复制相对路径到系统剪切板
        /// </summary>
        [MenuItem(k_MenuPath + "Copy Path", false, k_MenuPriority + 11)]
        private static void CopyRelativePath()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            PipiToolboxUtility.SaveToClipboard(list.Join(", "));
        }

        /// <summary>
        /// 复制 GUID 到系统剪切板
        /// </summary>
        [MenuItem(k_MenuPath + "Copy GUID", false, k_MenuPriority + 11)]
        private static void CopyGUID()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                list.Add(guid);
            }
            PipiToolboxUtility.SaveToClipboard(list.Join(", "));
        }

        /// <summary>
        /// 复制 AssetBundle 名称到系统剪切板
        /// </summary>
        [MenuItem(k_MenuPath + "Copy AssetBundle Name", false, k_MenuPriority + 11)]
        private static void CopyAssetBundleName()
        {
            List<string> list = new List<string>();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                list.Add(GetAssetBundleName(asset));
            }
            PipiToolboxUtility.SaveToClipboard(list.Join(", "));
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static string GetAbsolutePath(Object asset)
        {
            return Path.Combine(AssetUtility.ProjectPath, AssetDatabase.GetAssetPath(asset));
        }

        /// <summary>
        /// 获取 AssetBundle 名称
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static string GetAssetBundleName(Object asset)
        {
            return AssetDatabase.GetImplicitAssetBundleName(AssetDatabase.GetAssetPath(asset));
        }

    }

}
