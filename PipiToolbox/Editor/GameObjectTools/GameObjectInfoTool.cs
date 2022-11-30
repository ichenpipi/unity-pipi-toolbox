using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221130</version>
    public static class GameObjectInfoTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.GameObjectMenuBasePath + "Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.GameObjectMenuBasePriority + 001;

        /// <summary>
        /// 复制名称
        /// </summary>
        [MenuItem(MenuPath + "Copy Name", false, MenuPriority)]
        private static void Menu_CopyName()
        {
            if (Selection.activeGameObject)
            {
                SaveToClipboard(Selection.activeGameObject.name);
            }
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
