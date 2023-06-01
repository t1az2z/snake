using System;
using System.Runtime.CompilerServices;

namespace Core.Bindings.Tools.Helpers
{
    public struct MathdInternal {
        public const double DoubleMinDenormal = double.Epsilon;
        public const double DoubleMinNormal = 2.2250738585072014E-308d;
        public static bool IsFlushToZeroEnabled = (DoubleMinDenormal == 0d);
    }

    public static class Mathd {
        public const double Deg2Rad = (PI / 180d);
        public const double Infinity = double.PositiveInfinity;
        public const double NegativeInfinity = double.NegativeInfinity;
        public const double PI = Math.PI;
        public const double Rad2Deg = (180d / PI);
        public static readonly double Epsilon = MathdInternal.IsFlushToZeroEnabled
            ? MathdInternal.DoubleMinNormal
            : MathdInternal.DoubleMinDenormal;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(double value) {
            return Math.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Acos(double d) {
            return Math.Acos(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(double a, double b) {
            return Approximately(a, b, Epsilon);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(double a, double b, double epsilon) {
            return Abs(a - b) <= (Abs(a) + Abs(b) + 1d) * epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Asin(double d) {
            return Math.Asin(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Atan(double d) {
            return Math.Atan(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Atan2(double y, double x) {
            return Math.Atan2(y, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Ceil(double a) {
            return Math.Ceiling(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(double a) {
            return (int) Math.Ceiling(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min, double max) {
            if (value > max) {
                return max;
            }

            if (value < min) {
                return min;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp01(double value) {
            if (value > 1.0d) {
                return 1.0d;
            }

            if (value < 0.0d) {
                return 0.0d;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cos(double d) {
            return Math.Cos(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Floor(double d) {
            return Math.Floor(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(double d) {
            return (int) Math.Floor(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this double a) {
            return Math.Abs(a) <= Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this double a, double epsilon) {
            return Math.Abs(a) <= epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double a, double b, double t) {
            return a + (b - a) * Clamp01(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log(double a, double newBase) {
            return Math.Log(a, newBase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log10(double d) {
            return Math.Log10(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double a, double b) {
            return a > b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(params double[] values) {
            var len = values.Length;
            if (len == 0) {
                return 0;
            }

            var m = values[0];
            for (int i = 1; i < len; i++) {
                if (values[i] > m) {
                    m = values[i];
                }
            }

            return m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double a, double b) {
            return a < b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(params double[] values) {
            var len = values.Length;
            if (len == 0) {
                return 0;
            }

            var m = values[0];
            for (int i = 1; i < len; i++) {
                if (values[i] < m)
                    m = values[i];
            }

            return m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MoveTowards(double current, double target, double maxDelta) {
            if (target > current) {
                current += maxDelta;
                if (current > target) {
                    return target;
                }
            }
            else {
                current -= maxDelta;
                if (current < target) {
                    return target;
                }
            }

            return current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Pow(double x, double y) {
            return Math.Pow(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Round(double a) {
            return Math.Round(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(double a) {
            return (int) Math.Round(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sin(double a) {
            return Math.Sin(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Tan(double a) {
            return Math.Tan(a);
        }
    }
}