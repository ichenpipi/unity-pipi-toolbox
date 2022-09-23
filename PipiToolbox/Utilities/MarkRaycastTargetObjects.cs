#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace PipiToolbox.Editor
{

    /// <summary>
    /// 快速标记启用了 RaycastTarget 选项的对象
    /// </summary>
    /// <author>陈皮皮</author>
    /// <version>20220913</version>
    public class MarkRaycastTargetObjects : MonoBehaviour
    {

        [SerializeField]
        public Color color = Color.red;

        private readonly Vector3[] _fourCornersArray = new Vector3[4];

        private void OnDrawGizmos()
        {
            var components = FindObjectsOfType<MaskableGraphic>();
            foreach (var component in components)
            {
                if (!component.raycastTarget)
                {
                    continue;
                }
                var rectTransform = component.transform as RectTransform;
                if (!rectTransform)
                {
                    continue;
                }
                rectTransform.GetWorldCorners(_fourCornersArray);
                Gizmos.color = color;
                for (var i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(_fourCornersArray[i], _fourCornersArray[(i + 1) % 4]);
                }
            }
        }

    }

}

#endif
