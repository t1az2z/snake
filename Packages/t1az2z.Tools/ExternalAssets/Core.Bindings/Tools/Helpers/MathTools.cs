using System;
using UnityEngine;

namespace Core.Bindings.Tools.Helpers
{
    public struct MathInternal {
        public const float FloatMinDenormal = float.Epsilon;
        public const float FloatMinNormal = 1.175494E-38f;
        public static bool IsFlushToZeroEnabled = (FloatMinDenormal == 0f);
    }

    public static partial class MathTools {
        public enum RoundMode {
            HalfEven, // Round to nearest or to even whole number. (a.k.a Bankers)
            HalfPos,  // Round to nearest or toward positive.
            HalfNeg,  // Round to nearest or toward negative.
            HalfDown, // Round to nearest or toward zero.
            HalfUp,   // Round to nearest or away from zero.
            RndNeg,   // Round toward negative.                    (a.k.a.Floor)
            RndPos,   // Round toward positive.                    (a.k.a.Ceil )
            RndDown,  // Round toward zero.                        (a.k.a.Trunc)
            RndUp,    // Round away from zero.
        }

        public static readonly float Epsilon = MathInternal.IsFlushToZeroEnabled
            ? MathInternal.FloatMinNormal
            : MathInternal.FloatMinDenormal;

        public static float GetQuadraticEquationDisicriminant(float a, float b, float c) {
            return b * b - 4f * a * c;
        }

        // 20/10 -> 2/1  | 8/16 -> 1/2
        public static (int x, int y) GetReducedFraction(int a, int b) {
            int gcd = GreatestCommonDivisor(a, b);
            int x = a / gcd;
            int y = b / gcd;

            return (x, y);
        }

        public static int GreatestCommonDivisor(int a, int b) {
            if (b == 0) {
                return a;
            }

            return GreatestCommonDivisor(b, a % b);
        }

        public static float Integrate(Func<float, float> func, float from, float to, int steps) {
            var invSteps = 1f / steps;
            var range = to - from;
            var h = range * invSteps;

            var res = 0.5f * (func(from) + func(to));
            for (int i = 1; i < steps; ++i) {
                res += func(from + i * h);
            }

            return h * res;
        }

        public static int IntPow(int x, int pow) {
            var upow = (uint) pow;
            int ret = 1;
            while (upow != 0) {
                if ((upow & 1) == 1) {
                    ret *= x;
                }

                x *= x;
                upow >>= 1;
            }

            return ret;
        }

        public static bool IsZero(this float a) {
            return Mathf.Abs(a) <= Epsilon;
        }

        public static bool IsZero(this float a, float epsilon) {
            return Mathf.Abs(a) <= epsilon;
        }

        public static byte PackAngleToByte(this float self) {
            return (byte) (AnglePackMultiplier * self);
        }

        public static float PowByAbs(float x, float pow) {
            if (x >= 0f) {
                return Mathf.Pow(x, pow);
            }

            return -Mathf.Pow(-x, pow);
        }

        public static AnimationCurve RefineCurve(AnimationCurve sourceCurve, int refinePoints) {
            var maxTime = sourceCurve.keys[sourceCurve.length - 1].time;
            var workArea = Integrate(sourceCurve.Evaluate, 0f, maxTime, refinePoints);
            var refinedOffsetCurve = new AnimationCurve();

            refinedOffsetCurve.AddKey(0f, 0f);
            var currentValue = sourceCurve.Evaluate(0f);
            var step = maxTime / (refinePoints - 1);
            var currentTime = 0f;
            var invMaxTime = 1f / maxTime;
            var invArea = 1f / workArea;
            var boundTime = maxTime - 1.5f * step;
            var itr = 0;
            var area = 0d;
            do {
                ++itr;
                currentTime = itr * step;
                var nextValue = sourceCurve.Evaluate(currentTime);
                area += Math.Round(0.5f * step * (currentValue + nextValue) * invArea, 5, MidpointRounding.AwayFromZero);
                refinedOffsetCurve.AddKey(currentTime * invMaxTime, (float) area);
                currentValue = nextValue;
            } while (currentTime < boundTime);

            refinedOffsetCurve.AddKey(1f, 1f);
            return refinedOffsetCurve;
        }

        public static float Round(float value, RoundMode mode = RoundMode.HalfEven) {
            const double EPS = 1.19209289550781E-07d;
            const double ERROR_LIMIT = 1.234375d;
            const double SAFE_ERROR = 2d;
            const double MAX_ERROR = EPS * ERROR_LIMIT * SAFE_ERROR;
            return Round(value, MAX_ERROR, mode);
        }

        public static float Round(float value, double error, RoundMode mode = RoundMode.HalfEven) {
            if (float.IsNaN(value)) {
                return value;
            }

            var scaledVal = value;
            var scaledErr = Math.Abs(error);

            double val = 0d;
            switch (mode) {
                case RoundMode.HalfEven: // Round to nearest or to even whole number. (a.k.a Bankers)
                    val = Math.Round((Math.Abs(scaledVal) - scaledErr), MidpointRounding.AwayFromZero);
                    if (((int) val) % 2 != 0) {
                        val = Math.Round((Math.Abs(scaledVal) + scaledErr), MidpointRounding.AwayFromZero);
                    }

                    break;

                case RoundMode.HalfPos: // Round to nearest or toward positive.
                    val = Math.Round((scaledVal + scaledErr), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.HalfNeg: // Round to nearest or toward negative.
                    val = Math.Round((scaledVal - scaledErr), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.HalfDown: // Round to nearest or toward zero.
                    val = Math.Round((Math.Abs(scaledVal) - scaledErr), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.HalfUp: // Round to nearest or away from zero.
                    val = Math.Round((Math.Abs(scaledVal) + scaledErr), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.RndNeg: // Round toward negative. (a.k.a.Floor)
                    val = Math.Round(scaledVal + (scaledErr - 0.5), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.RndPos: // Round toward positive. (a.k.a.Ceil )
                    val = Math.Round(scaledVal - (scaledErr - 0.5), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.RndDown: // Round toward zero. (a.k.a.Trunc)
                    val = Math.Round((Math.Abs(scaledVal) + (scaledErr - 0.5)), MidpointRounding.AwayFromZero);
                    break;

                case RoundMode.RndUp: // Round away from zero.
                    val = Math.Round((Math.Abs(scaledVal) - (scaledErr - 0.5)), MidpointRounding.AwayFromZero);
                    break;

                default:
                    val = Math.Round(scaledVal, MidpointRounding.AwayFromZero);
                    break;
            }

            return value < 0 ? (float) -val : (float) val;
        }

        /// <summary>
        /// Solves quadratic equation in a form ax^2 + bx + c = 0
        /// </summary>
        /// <param name="results">Array to be filled with real roots of the equation (length >= 2)</param>
        /// <returns>Number of real roots of the equation</returns>
        public static int SolveQuadraticEquation(float a, float b, float c, float[] results) {
            float discriminant = GetQuadraticEquationDisicriminant(a, b, c);

            if (discriminant < 0f) {
                // no real roots
                return 0;
            }

            float aTimesTwo = 2 * a;

            if (discriminant.IsZero()) {
                // one real root
                float root = (-b) / aTimesTwo;
                results[0] = root;

                return 1;
            }

            // two real roots
            float discriminantSqrt = Mathf.Sqrt(discriminant);

            float root1 = (-b - discriminantSqrt) / aTimesTwo;
            float root2 = (-b + discriminantSqrt) / aTimesTwo;

            results[0] = root1;
            results[1] = root2;

            return 2;
        }

        public static void Swap<T>(ref T a, ref T b) {
            var temp = a;
            a = b;
            b = temp;
        }

        public static float UnpackToAngle(this byte self) {
            return AngleUnPackMultiplier * self;
        }

        const float AnglePackMultiplier = 255f / 360f;
        const float AngleUnPackMultiplier = 360f / 255f;
    }
}