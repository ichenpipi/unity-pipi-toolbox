using System;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 输入框窗口
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220829</version>
    public class InputDialogWindow : EditorWindow
    {

        #region Window Definition

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="title">窗口标题文本</param>
        /// <param name="description">描述文本</param>
        /// <param name="defaultInput">默认输入内容</param>
        /// <returns></returns>
        public static InputDialogWindow Create(string title = "Input Dialog", string description = "Type in the text area:", string defaultInput = "")
        {
            var window = GetWindow(typeof(InputDialogWindow), true) as InputDialogWindow;
            if (window == null)
            {
                throw new Exception("Failed to open window!");
            }
            // 窗口标题
            window.SetTitle(title);
            // 描述文本
            window.SetDescription(description);
            // 输入内容文本
            window.SetInputContent(defaultInput);
            // 窗口尺寸
            window.SetSize(450, 200);
            // 使窗口居中于编辑器
            window.SetCenter(0, -100);
            return window;
        }

        /// <summary>
        /// 描述文本
        /// </summary>
        private string description = "Type in the text area:";

        /// <summary>
        /// 输入内容
        /// </summary>
        private string inputContent = "";

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        private string confirmBtnLabel = "Confirm";

        /// <summary>
        /// 是否点击了确认按钮
        /// </summary>
        private bool isConfirmed = false;

        /// <summary>
        /// 确认按钮回调函数
        /// </summary>
        private Action<string> confirmCallback;

        /// <summary>
        /// 窗口关闭回调函数
        /// </summary>
        private Action<bool> closeCallback;

        /// <summary>
        /// 设置窗口标题文本
        /// </summary>
        /// <param name="value"></param>
        public void SetTitle(string value)
        {
            titleContent = new GUIContent(value);
        }

        /// <summary>
        /// 设置描述文本
        /// </summary>
        /// <param name="value"></param>
        public void SetDescription(string value)
        {
            description = value;
        }

        /// <summary>
        /// 设置输入内容文本
        /// </summary>
        /// <param name="value"></param>
        public void SetInputContent(string value)
        {
            inputContent = value;
        }

        /// <summary>
        /// 设置确认按钮回调函数
        /// </summary>
        /// <param name="callback">回调函数</param>
        public void SetConfirmCallback(Action<string> callback)
        {
            confirmCallback = callback;
        }

        /// <summary>
        /// 设置窗口关闭回调函数
        /// </summary>
        /// <param name="callback">回调函数</param>
        public void SetCloseCallback(Action<bool> callback)
        {
            closeCallback = callback;
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
        public void SetCenter(int offsetX = 0, int offsetY = 0)
        {
            Rect mainWindowPos = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = position;
            float centerOffsetX = (mainWindowPos.width - pos.width) * 0.5f;
            float centerOffsetY = (mainWindowPos.height - pos.height) * 0.5f;
            pos.x = mainWindowPos.x + centerOffsetX + offsetX;
            pos.y = mainWindowPos.y + centerOffsetY + offsetY;
            position = pos;
        }

        /// <summary>
        /// OnDestroy is called to close the EditorWindow window.
        /// </summary>
        public void OnDestroy()
        {
            closeCallback?.Invoke(isConfirmed);
        }

        #endregion

        #region GUI

        /// <summary>
        /// 绘制内容
        /// </summary>
        private void DrawContentGUI()
        {
            // 内容文本
            EditorGUILayout.LabelField(new GUIContent(description));

            GUILayout.Space(5);

            // 名称输入
            EditorGUILayout.BeginHorizontal();
            {
                inputContent = EditorGUILayout.TextArea(inputContent, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            // 确认按钮
            if (GUILayout.Button(new GUIContent(confirmBtnLabel)))
            {
                // 触发回调
                isConfirmed = true;
                confirmCallback?.Invoke(inputContent);
                // 关闭窗口
                Close();
            }
        }

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
                    DrawContentGUI();
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
