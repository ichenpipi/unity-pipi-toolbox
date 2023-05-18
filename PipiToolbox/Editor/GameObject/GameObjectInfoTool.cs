using UnityEditor;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// GameObject 信息工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221130</version>
    public static class GameObjectInfoTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.GameObjectMenuBasePath + "Info/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.GameObjectMenuBasePriority + 001;

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority2 = k_MenuPriority + 20;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string k_LogTag = "GameObjectInfo";

        /// <summary>
        /// 打印名称
        /// </summary>
        [MenuItem(k_MenuPath + "Print Name", false, k_MenuPriority)]
        private static void Menu_PrintName()
        {
            if (!Selection.activeGameObject) return;
            string value = Selection.activeGameObject.name;
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Name</color>: <color={LogColor.Yellow}>{value}</color>", Selection.activeGameObject);
        }

        /// <summary>
        /// 打印路径
        /// </summary>
        [MenuItem(k_MenuPath + "Print Path", false, k_MenuPriority)]
        private static void Menu_PrintPath()
        {
            if (!Selection.activeGameObject) return;
            string value = GameObjectUtility.GetPath(Selection.activeGameObject);
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>Path</color>: <color={LogColor.Yellow}>{value}</color>", Selection.activeGameObject);
        }

        /// <summary>
        /// 打印路径
        /// </summary>
        [MenuItem(k_MenuPath + "Print InstanceID", false, k_MenuPriority)]
        private static void Menu_PrintInstanceID()
        {
            if (!Selection.activeGameObject) return;
            string value = Selection.activeGameObject.GetInstanceID().ToString();
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>InstanceID</color>: <color={LogColor.Yellow}>{value}</color>", Selection.activeGameObject);
        }

        /// <summary>
        /// 打印路径
        /// </summary>
        [MenuItem(k_MenuPath + "Print fileID (Local Identifier In File)", false, k_MenuPriority)]
        private static void Menu_PrintFileID()
        {
            if (!Selection.activeGameObject) return;
            string value = GameObjectUtility.GetLocalIdentifier(Selection.activeGameObject).ToString();
            PipiToolboxUtility.LogNormal(k_LogTag, $"<color={LogColor.White}>fileID</color>: <color={LogColor.Yellow}>{value}</color>", Selection.activeGameObject);
        }

        /// <summary>
        /// 复制名称
        /// </summary>
        [MenuItem(k_MenuPath + "Copy Name", false, k_MenuPriority2)]
        private static void Menu_CopyName()
        {
            if (!Selection.activeGameObject) return;
            string value = Selection.activeGameObject.name;
            PipiToolboxUtility.SaveToClipboard(value);
        }

        /// <summary>
        /// 复制路径
        /// </summary>
        [MenuItem(k_MenuPath + "Copy Path", false, k_MenuPriority2)]
        private static void Menu_CopyPath()
        {
            if (!Selection.activeGameObject) return;
            string value = GameObjectUtility.GetPath(Selection.activeGameObject);
            PipiToolboxUtility.SaveToClipboard(value);
        }

        /// <summary>
        /// 复制 InstanceID
        /// </summary>
        [MenuItem(k_MenuPath + "Copy InstanceID", false, k_MenuPriority2)]
        private static void Menu_CopyInstanceID()
        {
            if (!Selection.activeGameObject) return;
            string value = Selection.activeGameObject.GetInstanceID().ToString();
            PipiToolboxUtility.SaveToClipboard(value);
        }

        /// <summary>
        /// 复制 fileID
        /// </summary>
        [MenuItem(k_MenuPath + "Copy fileID (Local Identifier In File)", false, k_MenuPriority2)]
        private static void Menu_CopyFileID()
        {
            if (!Selection.activeGameObject) return;
            string value = GameObjectUtility.GetLocalIdentifier(Selection.activeGameObject).ToString();
            PipiToolboxUtility.SaveToClipboard(value);
        }

    }

}
