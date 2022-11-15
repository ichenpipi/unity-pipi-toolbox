using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
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
        private const string MenuPath = PipiToolbox.AssetsMenuBasePath + "SVN/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.AssetsMenuBasePriority + 50;

        private static class Operation
        {
            public const string Update = "update";
            public const string Commit = "commit";
            public const string Revert = "revert";
            public const string Log = "log";
            public const string Diff = "diff";
        }

        [MenuItem(MenuPath + "Update", false, MenuPriority)]
        private static void Menu_Update()
        {
            ExecSVNCmd(Operation.Update, Join(GetSelectedFilePaths(), "*"));
        }

        [MenuItem(MenuPath + "Commit", false, MenuPriority)]
        private static void Menu_Commit()
        {
            ExecSVNCmd(Operation.Commit, Join(GetSelectedFilePaths(), "*"));
        }

        [MenuItem(MenuPath + "Revert", false, MenuPriority)]
        private static void Menu_Revert()
        {
            ExecSVNCmd(Operation.Revert, Join(GetSelectedFilePaths(), "*"));
        }

        [MenuItem(MenuPath + "Show Log", false, MenuPriority)]
        private static void Menu_Log()
        {
            ExecSVNCmd(Operation.Log, ToAbsolutePath(AssetDatabase.GetAssetPath(Selection.activeObject)));
        }

        [MenuItem(MenuPath + "Diff", false, MenuPriority)]
        private static void Menu_Diff()
        {
            ExecSVNCmd(Operation.Diff, ToAbsolutePath(AssetDatabase.GetAssetPath(Selection.activeObject)));
        }

        /// <summary>
        /// 执行 SVN 命令
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="path"></param>
        /// <param name="closeOnEnd"></param>
        public static void ExecSVNCmd(string operation, string path, bool closeOnEnd = false) {
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
            string[] guids = Selection.assetGUIDs;
            string[] paths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                paths[i] = ToAbsolutePath(AssetDatabase.GUIDToAssetPath(guids[i]));
            }
            return paths;
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        private static string ToAbsolutePath(string relativePath)
        {
            string projectPath = Application.dataPath;
            projectPath = projectPath.Substring(0, projectPath.LastIndexOf("Assets", StringComparison.Ordinal));
            return Path.Combine(projectPath, relativePath).Replace("\\", "/");
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
