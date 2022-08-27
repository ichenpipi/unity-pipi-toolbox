using System;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 输入框窗口
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220827</version>
    public class InputDialogWindow : EditorWindow
    {

        #region Window Definition

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static InputDialogWindow Create(string title = "Input Dialog")
        {
            var window = GetWindow(typeof(InputDialogWindow)) as InputDialogWindow;
            if (window == null)
            {
                throw new Exception("Failed to open window!");
            }
            // 设置窗口标题
            window.SetTitle(title);
            // 设置窗口尺寸
            window.SetSize(450, 200);
            // 使窗口居中于编辑器
            window.Center(0, -100);
            return window;
        }

        /// <summary>
        /// 设置标题文本
        /// </summary>
        /// <param name="value">内容</param>
        public void SetTitle(string value)
        {
            titleContent = new GUIContent(value);
        }

        /// <summary>
        /// 设置内容文本
        /// </summary>
        /// <param name="value">内容</param>
        public void SetContent(string value)
        {
            content = value;
        }

        /// <summary>
        /// 设置窗口尺寸
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public void SetSize(int width, int height)
        {
            // 同时设置最小尺寸
            minSize = new Vector2(width, height);
            // 窗口尺寸
            Rect pos = position;
            pos.width = width;
            pos.height = height;
            position = pos;
        }

        /// <summary>
        /// 使窗口居中（基于 Unity 编辑器主窗口）
        /// </summary>
        /// <param name="offsetX">水平偏移</param>
        /// <param name="offsetY">垂直偏移</param>
        public void Center(int offsetX = 0, int offsetY = 0)
        {
            Rect mainWindowPos = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = position;
            float centerOffsetX = (mainWindowPos.width - pos.width) * 0.5f;
            float centerOffsetY = (mainWindowPos.height - pos.height) * 0.5f;
            pos.x = mainWindowPos.x + centerOffsetX + offsetX;
            pos.y = mainWindowPos.y + centerOffsetY + offsetY;
            position = pos;
        }

        #endregion

        #region GUI Styles

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        private readonly GUIContent _confirmButtonLabel = new GUIContent("Confirm");

        /// <summary>
        /// 布局选项
        /// </summary>
        private static class MyLayoutOptions
        {

            public static readonly GUILayoutOption expandWidth = GUILayout.ExpandWidth(true);

            public static readonly GUILayoutOption expandHeight = GUILayout.ExpandHeight(true);

        }

        /// <summary>
        /// 风格
        /// </summary>
        private static class MyStyles
        {

            /// <summary>
            /// 风格
            /// </summary>
            public static readonly GUIStyle ContentStyle = new GUIStyle(EditorStyles.boldLabel);

            /// <summary>
            /// 输入框风格
            /// </summary>
            public static readonly GUIStyle InputStyle = new GUIStyle();

        }

        #endregion

        #region GUI

        /// <summary>
        /// 内容文本
        /// </summary>
        private string content = "Type in the text area:";

        /// <summary>
        /// 输入文本
        /// </summary>
        private string inputValue = "";

        /// <summary>
        /// 绘制 GUI
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();
                {
                    // 内容文本
                    EditorGUILayout.LabelField(new GUIContent(content));

                    GUILayout.Space(10);

                    // 名称输入
                    EditorGUILayout.BeginHorizontal();
                    {
                        // EditorGUILayout.LabelField(_inputTitleLabel);
                        inputValue = EditorGUILayout.TextArea(inputValue, MyLayoutOptions.expandWidth ,MyLayoutOptions.expandHeight);
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    // 确认按钮
                    if (GUILayout.Button(_confirmButtonLabel))
                    {
                        GUI.FocusControl(null);
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        #endregion

    }

}
