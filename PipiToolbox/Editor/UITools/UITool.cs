using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{
    /// <summary>
    /// UI 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221115</version>
    public static class UITool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.GameObjectMenuBasePath + "UI Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.GameObjectMenuBasePriority + 0;

        /// <summary>
        /// Log 头部信息
        /// </summary>
        private const string LogHeader = "SearchTool";

        /// <summary>
        /// Log 键颜色
        /// </summary>
        private const string LogKeyColor = "white";

        /// <summary>
        /// Log 值颜色
        /// </summary>
        private const string LogValueColor = "yellow";

        /// <summary>
        /// Ctrl + ↑
        /// </summary>
        [MenuItem(MenuPath + "Move Up (1 pixel) %UP", false, MenuPriority)]
        private static void Menu_MoveUp()
        {
            Move((RectTransform) Selection.activeTransform, Vector2.up);
        }

        /// <summary>
        /// Ctrl + ↓
        /// </summary>
        [MenuItem(MenuPath + "Move Down (1 pixel) %DOWN", false, MenuPriority)]
        private static void Menu_MoveDown()
        {
            Move((RectTransform) Selection.activeTransform, Vector2.down);
        }

        /// <summary>
        /// Ctrl + ←
        /// </summary>
        [MenuItem(MenuPath + "Move Left (1 pixel) %LEFT", false, MenuPriority)]
        private static void Menu_MoveLeft()
        {
            Move((RectTransform) Selection.activeTransform, Vector2.left);
        }

        /// <summary>
        /// Ctrl + →
        /// </summary>
        [MenuItem(MenuPath + "Move Right (1 pixel) %RIGHT", false, MenuPriority)]
        private static void Menu_MoveRight()
        {
            Move((RectTransform) Selection.activeTransform, Vector2.right);
        }

        /// <summary>
        /// Ctrl + Shift + ←
        /// </summary>
        [MenuItem(MenuPath + "Rotate Clockwise (1 degree) %#RIGHT", false, MenuPriority)]
        private static void Menu_RotateClockwise()
        {
            Rotate((RectTransform) Selection.activeTransform, -1f);
        }

        /// <summary>
        /// Ctrl + Shift + →
        /// </summary>
        [MenuItem(MenuPath + "Rotate Anti-clockwise (1 degree) %#LEFT", false, MenuPriority)]
        private static void Menu_RotateAnticlockwise()
        {
            Rotate((RectTransform) Selection.activeTransform, 1f);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="movement"></param>
        private static void Move(RectTransform transform, Vector2 movement)
        {
            if (transform)
            {
                transform.anchoredPosition += movement;
            }
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="zAngle"></param>
        private static void Rotate(RectTransform transform, float zAngle)
        {
            if (transform)
            {
                transform.Rotate(0, 0, zAngle);
            }
        }
    }
}