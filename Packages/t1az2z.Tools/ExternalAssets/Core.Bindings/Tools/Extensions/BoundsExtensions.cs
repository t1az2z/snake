using UnityEngine;

namespace Core.Bindings.Tools.Extensions {
    public static class Bounds2DExtensions {
        public static Bounds GetInner(this Bounds bounds, float aspect) {
            var yBound = Mathf.Min(bounds.size.y, bounds.size.x / aspect);
            var xBound = yBound * aspect;
            return new Bounds(bounds.center, new Vector3(xBound, yBound, float.MaxValue));
        }

        public static Bounds GetOuter(this Bounds bounds, float aspect) {
            var yBound = Mathf.Max(bounds.size.y, bounds.size.x / aspect);
            var xBound = yBound * aspect;
            return new Bounds(bounds.center, new Vector3(xBound, yBound, float.MaxValue));
        }

        public static Bounds MakeInnerAndKeepAspect(this Bounds src, Bounds boundVolume, float eps = 0.000001f) {
            var aspect = src.size.x / src.size.y;

            var pos = src.center;
            var size = src.size;

            if (size.x > boundVolume.size.x + eps) {
                size.x = boundVolume.size.x;
                size.y = size.x / aspect;
            }

            if (size.y > boundVolume.size.y + eps) {
                size.y = boundVolume.size.y;
                size.x = size.y * aspect;
            }

            var extent = 0.5f * size;
            var min = boundVolume.min;
            if (pos.x - extent.x < min.x) {
                pos.x = min.x + extent.x;
            }

            if (pos.y - extent.y < min.y) {
                pos.y = min.y + extent.y;
            }

            var max = boundVolume.max;
            if (pos.x + extent.x > max.x) {
                pos.x = max.x - extent.x;
            }

            if (pos.y + extent.y > max.y) {
                pos.y = max.y - extent.y;
            }

            return new Bounds(pos, size);
        }
    }
}