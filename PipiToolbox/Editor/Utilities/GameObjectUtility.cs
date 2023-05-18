using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ChenPipi.PipiToolbox
{

    /// <summary>
    /// 皮皮工具箱
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20230516</version>
    public static class GameObjectUtility
    {

        /// <summary>
        /// 获取 GameObject 的路径
        /// </summary>
        public static string GetPath(GameObject gameObject)
        {
            if (!gameObject)
            {
                return string.Empty;
            }
            Transform transform = gameObject.transform;
            if (!transform.parent)
            {
                return transform.name;
            }
            return string.Concat(GetPath(transform.parent.gameObject), "/", transform.name);
        }

        /// <summary>
        /// 递归设置 GameObject 的 Layer
        /// </summary>
        /// <param name="root"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        #region Local Identifier In File

        private static readonly PropertyInfo s_InspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        public static long GetLocalIdentifier(Object obj)
        {
            SerializedObject serializedObject = new SerializedObject(obj);
            s_InspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug);
            return serializedObject.FindProperty("m_LocalIdentfierInFile").longValue;
        }

        public static T FindObjectInChildrenByFileID<T>(GameObject root, long fileId) where T : Object
        {
            T[] objects = root.GetComponentsInChildren<T>();
            return objects.Where(w => w != null).FirstOrDefault(w => GetLocalIdentifier(w).Equals(fileId));
        }

        #endregion

    }

}
