using System.Diagnostics;
using UnityEditor;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// SVN 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221025</version>
    public static class SubversionTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.AssetsMenuBasePath + "SVN/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.AssetsMenuBasePriority + 50;

        private static class Operation
        {
            public const string Update = "update";
            public const string Commit = "commit";
            public const string Revert = "revert";
            public const string Log = "log";
            public const string Diff = "diff";
        }

        [MenuItem(k_MenuPath + "Update", false, k_MenuPriority)]
        private static void Menu_Update()
        {
            ExecSVNCmd(Operation.Update, GetSelectedFilePaths().Join("*"));
        }

        [MenuItem(k_MenuPath + "Commit", false, k_MenuPriority)]
        private static void Menu_Commit()
        {
            ExecSVNCmd(Operation.Commit, GetSelectedFilePaths().Join("*"));
        }

        [MenuItem(k_MenuPath + "Revert", false, k_MenuPriority)]
        private static void Menu_Revert()
        {
            ExecSVNCmd(Operation.Revert, GetSelectedFilePaths().Join("*"));
        }

        [MenuItem(k_MenuPath + "Show Log", false, k_MenuPriority)]
        private static void Menu_Log()
        {
            ExecSVNCmd(Operation.Log, AssetUtility.ToAbsolutePath(AssetDatabase.GetAssetPath(Selection.activeObject)));
        }

        [MenuItem(k_MenuPath + "Diff", false, k_MenuPriority)]
        private static void Menu_Diff()
        {
            ExecSVNCmd(Operation.Diff, AssetUtility.ToAbsolutePath(AssetDatabase.GetAssetPath(Selection.activeObject)));
        }

        /// <summary>
        /// 执行 SVN 命令
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="path"></param>
        /// <param name="closeOnEnd"></param>
        public static void ExecSVNCmd(string operation, string path, bool closeOnEnd = false)
        {
            string command = $"/c tortoiseproc.exe /command:{operation} /path:\"{path}\"";
            command += (closeOnEnd ? " /closeonend:3" : " /closeonend:0");
            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", command)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(info);
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <returns></returns>
        private static string[] GetSelectedFilePaths()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            string[] paths = new string[assetGUIDs.Length];
            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                paths[i] = AssetUtility.ToAbsolutePath(AssetDatabase.GUIDToAssetPath(assetGUIDs[i]));
            }
            return paths;
        }

    }

}
