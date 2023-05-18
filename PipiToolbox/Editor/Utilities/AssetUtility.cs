using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// 资源工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230518</version>
    public static class AssetUtility
    {

        /// <summary>
        /// Set the AssetBundle name and variant.
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetBundleName">AssetBundle 名称</param>
        /// <param name="assetBundleVariant">AssetBundle 变体</param>
        public static void SetAssetBundleNameAndVariant(string assetPath, string assetBundleName, string assetBundleVariant)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            if (assetImporter == null)
            {
                return;
            }
            assetBundleName = assetBundleName.ToLower();
            assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
            assetImporter.SaveAndReimport();
        }

        /// <summary>
        /// 应用资源变更
        /// </summary>
        /// <param name="asset">目标资源</param>
        public static void ApplyAssetChanges(Object asset)
        {
            EditorUtility.DisplayProgressBar("正在应用资源变更...", "请稍候...", 1);
            {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 重新导入资源
        /// </summary>
        /// <param name="asset">目标资源</param>
        public static async void ReimportAsset(Object asset)
        {
            EditorUtility.DisplayProgressBar("正在重新导入资源...", "请稍候...", 1);
            {
                // 资源文件路径
                string assetPath = AssetDatabase.GetAssetPath(asset);
                string filePath = Application.dataPath + assetPath.Substring(6);
                string metaPath = filePath + ".meta";
                FileInfo fileInfo = new FileInfo(filePath);
                string dirPath = fileInfo.DirectoryName!.Replace("\\", "/");
                // 临时目录路径（项目根目录）
                string tempDirPath = Application.dataPath.Replace("/Assets", "");
                string tempFilePath = filePath.Replace(dirPath, tempDirPath);
                string tempMetaPath = tempFilePath + ".meta";
                // 将文件移动到项目外的临时目录
                File.Move(filePath, tempFilePath);
                File.Move(metaPath, tempMetaPath);
                AssetDatabase.Refresh();
                // 等待一会
                await Task.Delay(100);
                // 将文件移动回项目内原始位置
                File.Move(tempFilePath, filePath);
                File.Move(tempMetaPath, metaPath);
                AssetDatabase.Refresh();
                // 选中资源
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 获取指定目录下的所有资源的路径
        /// </summary>
        public static string[] GetAssetsAtPath(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(p => !p.EndsWith(".meta"))
                .ToArray();
        }

        /// <summary>
        /// 绝对路径转相对路径（基于项目 Assets 目录）
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns></returns>
        public static string ToRelativePath(string absolutePath)
        {
            return ("Assets" + absolutePath.Substring(Application.dataPath.Length)).Replace("\\", "/");
        }

        public static readonly string ProjectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal));

        /// <summary>
        /// 相对路径转绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public static string ToAbsolutePath(string relativePath)
        {
            return Path.Combine(ProjectPath, relativePath).Replace("\\", "/");
        }

    }

}
