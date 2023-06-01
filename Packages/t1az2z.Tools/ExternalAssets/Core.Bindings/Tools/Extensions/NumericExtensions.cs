using System.Runtime.CompilerServices;
using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif    
    public static class NumericExtensions {
        // FLOAT
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Abs(in this float value) => Mathf.Abs(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Acos(in this float value) => Mathf.Acos(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool Approximately(in this float num, float value) => Approximately(num, value, Mathf.Epsilon);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool Approximately(in this float num, float value, float epsilon) => Abs(num - value) <= (Abs(num) + Abs(value) + 1d) * epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Asin(in this float value) => Mathf.Asin(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Atan(in this float value) => Mathf.Atan(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Ceil(in this float value) => Mathf.Ceil(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CeilToInt(in this float value) => Mathf.CeilToInt(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Clamp(in this float value, float min, float max) => Mathf.Clamp(value, min, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Clamp01(in this float value) => Mathf.Clamp01(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Cos(in this float value) => Mathf.Cos(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Floor(in this float value) => Mathf.Floor(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int FloorToInt(in this float value)=> Mathf.FloorToInt(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsZero(in this float value) => Mathf.Abs(value) <= Mathf.Epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsZero(in this float value, float epsilon) => Mathf.Abs(value) <= epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Log(in this float value, float newBase) => Mathf.Log(value, newBase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Log10(in this float value) => Mathf.Log10(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float MoveTowards(in this float current, float target, float maxDelta) => Mathf.MoveTowards(current, target, maxDelta);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Pow(in this float value, float x) => Mathf.Pow(value, x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Round(in this float value) => Mathf.Round(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int RoundToInt(in this float value) => Mathf.RoundToInt(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Sin(in this float value) => Mathf.Sin(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Tan(in this float value) => Mathf.Tan(value);
    }
#if !BASE_GLOBAL_EXTENSIONS
}
#endif