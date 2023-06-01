#if !BASE_GLOBAL_EXTENSIONS
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Tools.Extensions {
#endif
    public static class RectTransformExtensions {
        private static List<MonoBehaviour> _searchCache = new List<MonoBehaviour>(16);

        public static void RebuildNearestLayout(this RectTransform trans) {
            var rt = trans.GetInterfacedComponentInParent<ILayoutElement>()?.transform as RectTransform;
            if (rt) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            }
        }

        public static void RebuildElementLayouts(this Component component) {
            _searchCache.Clear();
            component.GetChildInterfacedComponentRoots<ILayoutElement>(_searchCache);
            foreach (var itr in _searchCache) {
                if (itr.transform is RectTransform rt) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
                }
            }
        }

        public static float GetHeight(this RectTransform trans) {
            return trans.rect.height;
        }

        public static Rect GetScreenRect(this RectTransform rectTransform, Camera camera = null) {
            //DONT CALL FROM AWAKE!!!
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var xMin = float.PositiveInfinity;
            var xMax = float.NegativeInfinity;
            var yMin = float.PositiveInfinity;
            var yMax = float.NegativeInfinity;
            for (int i = 0; i < corners.Length; i++) {
                var corner = corners[i];
                var screenCoord = RectTransformUtility.WorldToScreenPoint(camera, corner);
                screenCoord.y = Screen.height - screenCoord.y;
                if (screenCoord.x < xMin) {
                    xMin = screenCoord.x;
                }

                if (screenCoord.x > xMax) {
                    xMax = screenCoord.x;
                }

                if (screenCoord.y < yMin) {
                    yMin = screenCoord.y;
                }

                if (screenCoord.y > yMax) {
                    yMax = screenCoord.y;
                }
            }

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        public static Vector2 GetSize(this RectTransform trans) {
            return trans.rect.size;
        }

        public static float GetWidth(this RectTransform trans) {
            return trans.rect.width;
        }

        public static void SetDefaultScale(this RectTransform trans) {
            trans.localScale = Vector3.one;
        }

        public static void SetHeight(this RectTransform trans, float newSize) {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height),
                trans.localPosition.z);
        }

        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height),
                trans.localPosition.z);
        }

        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
            trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        }

        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height),
                trans.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width),
                newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize) {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        public static void SetWidth(this RectTransform trans, float newSize) {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        public static void Stretch(this RectTransform rect) {
            rect.anchoredPosition3D = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif