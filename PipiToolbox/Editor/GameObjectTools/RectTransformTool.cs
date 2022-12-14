using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// RectTransform 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221213</version>
    public static class RectTransformTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string MenuPath = PipiToolbox.GameObjectMenuBasePath + "RectTransform Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int MenuPriority = PipiToolbox.GameObjectMenuBasePriority + 201;

        /// <summary>
        /// 上移（Ctrl + ↑）
        /// </summary>
        [MenuItem(MenuPath + "Move Up (1 pixel) %UP", false, MenuPriority)]
        private static void Menu_MoveUp()
        {
            Move(Selection.transforms, Vector2.up);
        }

        /// <summary>
        /// 下移（Ctrl + ↓）
        /// </summary>
        [MenuItem(MenuPath + "Move Down (1 pixel) %DOWN", false, MenuPriority)]
        private static void Menu_MoveDown()
        {
            Move(Selection.transforms, Vector2.down);
        }

        /// <summary>
        /// 左移（Ctrl + ←）
        /// </summary>
        [MenuItem(MenuPath + "Move Left (1 pixel) %LEFT", false, MenuPriority)]
        private static void Menu_MoveLeft()
        {
            Move(Selection.transforms, Vector2.left);
        }

        /// <summary>
        /// 右移（Ctrl + →）
        /// </summary>
        [MenuItem(MenuPath + "Move Right (1 pixel) %RIGHT", false, MenuPriority)]
        private static void Menu_MoveRight()
        {
            Move(Selection.transforms, Vector2.right);
        }

        /// <summary>
        /// 顺时针选择（Ctrl + Shift + ←）
        /// </summary>
        [MenuItem(MenuPath + "Rotate Clockwise (1 degree) %#RIGHT", false, MenuPriority)]
        private static void Menu_RotateClockwise()
        {
            Rotate(Selection.transforms, -1f);
        }

        /// <summary>
        /// 逆时针旋转（Ctrl + Shift + →）
        /// </summary>
        [MenuItem(MenuPath + "Rotate Anti-clockwise (1 degree) %#LEFT", false, MenuPriority)]
        private static void Menu_RotateAnticlockwise()
        {
            Rotate(Selection.transforms, 1f);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="movement"></param>
        private static void Move(RectTransform rectTransform, Vector2 movement)
        {
            if (rectTransform == null) return;
            Undo.RegisterFullObjectHierarchyUndo(rectTransform, "Update position");
            rectTransform.anchoredPosition += movement;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="movement"></param>
        private static void Move(Transform[] transforms, Vector2 movement)
        {
            foreach (Transform transform in transforms)
            {
                if (!(transform is RectTransform rectTransform)) continue;
                Move(rectTransform, movement);
            }
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="zAngle"></param>
        private static void Rotate(RectTransform rectTransform, float zAngle)
        {
            if (rectTransform == null) return;
            Undo.RegisterFullObjectHierarchyUndo(rectTransform, "Update rotation");
            rectTransform.Rotate(0, 0, zAngle);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="zAngle"></param>
        private static void Rotate(Transform[] transforms, float zAngle)
        {
            foreach (Transform transform in transforms)
            {
                if (!(transform is RectTransform rectTransform)) continue;
                Rotate(rectTransform, zAngle);
            }
        }

    }

}
