using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox.Editor
{

    /// <summary>
    /// Transform 工具
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230519</version>
    public static class TransformTool
    {

        /// <summary>
        /// 菜单项路径
        /// </summary>
        private const string k_MenuPath = PipiToolboxMenu.GameObjectMenuBasePath + "Transform Tool/";

        /// <summary>
        /// 菜单项优先级
        /// </summary>
        private const int k_MenuPriority = PipiToolboxMenu.GameObjectMenuBasePriority + 201;

        /// <summary>
        /// 上移（Ctrl + ↑）
        /// </summary>
        [MenuItem(k_MenuPath + "Move Up (1 pixel) %UP", false, k_MenuPriority)]
        private static void Menu_MoveUp()
        {
            Move(Selection.transforms, Vector2.up);
        }

        /// <summary>
        /// 下移（Ctrl + ↓）
        /// </summary>
        [MenuItem(k_MenuPath + "Move Down (1 pixel) %DOWN", false, k_MenuPriority)]
        private static void Menu_MoveDown()
        {
            Move(Selection.transforms, Vector2.down);
        }

        /// <summary>
        /// 左移（Ctrl + ←）
        /// </summary>
        [MenuItem(k_MenuPath + "Move Left (1 pixel) %LEFT", false, k_MenuPriority)]
        private static void Menu_MoveLeft()
        {
            Move(Selection.transforms, Vector2.left);
        }

        /// <summary>
        /// 右移（Ctrl + →）
        /// </summary>
        [MenuItem(k_MenuPath + "Move Right (1 pixel) %RIGHT", false, k_MenuPriority)]
        private static void Menu_MoveRight()
        {
            Move(Selection.transforms, Vector2.right);
        }

        /// <summary>
        /// 顺时针选择（Ctrl + Shift + ←）
        /// </summary>
        [MenuItem(k_MenuPath + "Rotate Clockwise (1 degree) %#RIGHT", false, k_MenuPriority)]
        private static void Menu_RotateClockwise()
        {
            RotateZ(Selection.transforms, -1f);
        }

        /// <summary>
        /// 逆时针旋转（Ctrl + Shift + →）
        /// </summary>
        [MenuItem(k_MenuPath + "Rotate Anti-clockwise (1 degree) %#LEFT", false, k_MenuPriority)]
        private static void Menu_RotateAnticlockwise()
        {
            RotateZ(Selection.transforms, 1f);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="movement"></param>
        private static void Move(Transform transform, Vector2 movement)
        {
            Undo.RegisterFullObjectHierarchyUndo(transform, "Update position");
            if (transform is RectTransform rectTransform)
            {
                rectTransform.anchoredPosition += movement;
            }
            else
            {
                transform.localPosition += (Vector3)movement;
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="movement"></param>
        private static void Move(IEnumerable<Transform> transforms, Vector2 movement)
        {
            foreach (Transform transform in transforms)
            {
                Move(transform, movement);
            }
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="zAngle"></param>
        private static void RotateZ(Transform transform, float zAngle)
        {
            Undo.RegisterFullObjectHierarchyUndo(transform, "Update rotation");
            transform.Rotate(0, 0, zAngle);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="zAngle"></param>
        private static void RotateZ(IEnumerable<Transform> transforms, float zAngle)
        {
            foreach (Transform transform in transforms)
            {
                RotateZ(transform, zAngle);
            }
        }

    }

}
