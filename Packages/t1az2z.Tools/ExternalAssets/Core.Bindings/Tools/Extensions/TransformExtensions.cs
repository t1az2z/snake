using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class TransformExtensions {
        public static Transform[] GetAllChildren(this Transform self) {
            var result = new Transform[self.childCount];
            for (int i = 0; i < result.Length; ++i) {
                result[i] = self.GetChild(i);
            }
            return result;
        }

        public static void DestroyAllChildren(this Transform self, int skipFirstN = 0) {
            if (self.childCount > skipFirstN) {
                for (var i = self.childCount - 1; i >= skipFirstN; --i) {
                    UnityEngine.Object.Destroy(self.GetChild(i).gameObject);
                }
            }
        }

        public static void DestroyAllChildrenImmediate(this Transform self, int skipFirstN = 0) {
            if (self.childCount > skipFirstN) {
                for (var i = self.childCount - 1; i >= skipFirstN; --i) {
                    UnityEngine.Object.DestroyImmediate(self.GetChild(i).gameObject);
                }
            }
        }

        public static void SetParentAndReset(this Transform self, Transform target) {
            self.SetParent(target, false);
            self.localPosition = Vector3.zero;
            self.localScale = Vector3.one;
            self.localRotation = Quaternion.identity;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif