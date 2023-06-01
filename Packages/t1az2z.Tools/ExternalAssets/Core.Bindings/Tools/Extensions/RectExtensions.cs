using System.Runtime.CompilerServices;
using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif
    public static class RectExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt RoundToInt(in this Rect rect) => new RectInt(rect.xMin.RoundToInt(), rect.yMin.RoundToInt(), rect.width.RoundToInt(), rect.height.RoundToInt());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt FloorToInt(in this Rect rect) => new RectInt(rect.xMin.FloorToInt(), rect.yMin.FloorToInt(), rect.width.FloorToInt(), rect.height.FloorToInt());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt CeilToInt(in this Rect rect) => new RectInt(rect.xMin.CeilToInt(), rect.yMin.CeilToInt(), rect.width.CeilToInt(), rect.height.CeilToInt());
    }
#if !BASE_GLOBAL_EXTENSIONS
}
#endif