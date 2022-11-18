using System;
using UnityEditor;
using UnityEngine;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 输入框窗口
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20221118</version>
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
            window.description = description;
            // 输入内容文本
            window.inputContent = defaultInput;
            // 窗口尺寸
            window.SetSize(450, 200);
            // 使窗口居中于编辑器
            window.SetCenter(0, -100);
            return window;
        }

        /// <summary>
        /// 描述文本
        /// </summary>
        public string description = "Type in the text area:";

        /// <summary>
        /// 输入内容
        /// </summary>
        public string inputContent = "";

        /// <summary>
        /// 占位符
        /// </summary>
        public string placeholder = "Text here...";

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        public string confirmBtnLabel = "Confirm";

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
        public bool needCancelBtn = false;

        /// <summary>
        /// 取消按钮文本
        /// </summary>
        public string cancelBtnLabel = "Cancel";

        /// <summary>
        /// 是否点击了确认按钮
        /// </summary>
        private bool isConfirmed = false;

        /// <summary>
        /// 是否点击了按钮按钮
        /// </summary>
        private bool isCanceled = false;

        /// <summary>
        /// 确认回调函数
        /// </summary>
        public Action<string> confirmCallback;

        /// <summary>
        /// 取消回调函数
        /// </summary>
        public Action cancelCallback;

        /// <summary>
        /// 窗口关闭回调函数
        /// </summary>
        public Action<bool, bool> closeCallback;

        /// <summary>
        /// 设置窗口标题文本
        /// </summary>
        /// <param name="value"></param>
        public void SetTitle(string value)
        {
            titleContent = new GUIContent(value);
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
            closeCallback?.Invoke(isConfirmed, isCanceled);
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
                // 输入框
                inputContent = EditorGUILayout.TextArea(inputContent, GUILayout.MinWidth(0), GUILayout.ExpandHeight(true));
                // 占位符
                if (string.IsNullOrEmpty(inputContent)) {
                    Rect pos = new Rect(GUILayoutUtility.GetLastRect());
                    GUIStyle style = new GUIStyle
                    {
                        alignment = TextAnchor.UpperLeft,
                        padding = new RectOffset(3, 0, 2, 0),
                        fontStyle = FontStyle.Italic,
                        normal =
                        {
                            textColor = Color.grey
                        }
                    };
                    EditorGUI.LabelField(pos, placeholder, style);
                }
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

            // 取消按钮
            if (needCancelBtn && GUILayout.Button(new GUIContent(cancelBtnLabel)))
            {
                // 触发回调
                isCanceled = true;
                cancelCallback?.Invoke();
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
