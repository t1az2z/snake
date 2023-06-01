using System;
using UnityEngine;

namespace Core.Bindings.Tools.Helpers {
    public static class Phase {
        /// <summary>
        /// Berp - Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        public static float Berp(float start, float end, float value) {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        /// <summary>
        ///  Bounce - Returns a value between 0 and 1 that can be used to easily make bouncing GUI items (a la OS X's Dock)
        /// </summary>
        public static float Bounce(float phase) {
            return Mathf.Abs(Mathf.Sin(6.28f * (phase + 1f) * (phase + 1f)) * (1f - phase));
        }

        public static float Clamp(double phase, bool forward) {
            return Clamp((float) phase, forward);
        }

        public static float Clamp(float phase, bool forward) {
            var result = Mathf.Clamp01(phase);
            return forward ? result : 1f - result;
        }

        /// <summary>
        ///  CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
        ///  This is useful when interpolating eulerAngles and the object
        ///  crosses the 0/360 boundary.  The standard Lerp function causes the object
        ///  to rotate in the wrong direction and looks stupid. CLerp fixes that.
        /// </summary>
        public static float CLerp(float start, float end, float value) {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) * 0.5f);
            float retval = 0.0f;
            float diff = 0.0f;
            if ((end - start) < -half) {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half) {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;
            return retval;
        }

        public static float EaseInBack(float start, float end, float value) {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static float EaseInBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static float EaseInCirc(float start, float end, float value) {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float value) {
            end -= start;
            return end * value * value * value + start;
        }

        public static float EaseInElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseInExpo(float start, float end, float value) {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }

        public static float EaseInOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1) {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static float EaseInOutBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float EaseInOutCirc(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;
        }

        public static float EaseInOutElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }

        public static float EaseInOutExpo(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start;
        }

        public static float EaseInOutQuint(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }

        public static float EaseInOutSine(float start, float end, float value) {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }

        public static float EaseInQuad(float start, float end, float value) {
            end -= start;
            return end * value * value + start;
        }

        public static float EaseInQuart(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value + start;
        }

        public static float EaseInQuint(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        public static float EaseInSine(float start, float end, float value) {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }

        public static float EaseOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        //public static float bounce(float start, float end, float value){
        public static float EaseOutBounce(float start, float end, float value) {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f)) {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f)) {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75)) {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static float EaseOutCirc(float start, float end, float value) {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        public static float EaseOutCubic(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        //public static float elastic(float start, float end, float value){
        public static float EaseOutElastic(float start, float end, float value) {
            //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p * 0.25f;
            }
            else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseOutExpo(float start, float end, float value) {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }

        public static float EaseOutQuad(float start, float end, float value) {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        public static float EaseOutQuart(float start, float end, float value) {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        public static float EaseOutQuint(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        public static float EaseOutSine(float start, float end, float value) {
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }

        public static float Ease(this float value, EaseType easeType) => GetEasing(easeType)(0f, 1f, value);

        public static Func<float, float, float, float> GetEasing(EaseType easeType) {
            return easeType switch {
                EaseType.Lerp => Lerp,
                EaseType.CLerp => CLerp,
                EaseType.Berp => Berp,
                EaseType.Punch => PunchDefault,
                EaseType.EaseInQuad => EaseInQuad,
                EaseType.EaseOutQuad => EaseOutQuad,
                EaseType.EaseInOutQuad => EaseInOutQuad,
                EaseType.EaseInCubic => EaseInCubic,
                EaseType.EaseOutCubic => EaseOutCubic,
                EaseType.EaseInOutCubic => EaseInOutCubic,
                EaseType.EaseInQuart => EaseInQuart,
                EaseType.EaseOutQuart => EaseOutQuart,
                EaseType.EaseInOutQuart => EaseInOutQuart,
                EaseType.EaseInQuint => EaseInQuint,
                EaseType.EaseOutQuint => EaseOutQuint,
                EaseType.EaseInOutQuint => EaseInOutQuint,
                EaseType.EaseInSine => EaseInSine,
                EaseType.EaseOutSine => EaseOutSine,
                EaseType.EaseInOutSine => EaseInOutSine,
                EaseType.EaseInExpo => EaseInExpo,
                EaseType.EaseOutExpo => EaseOutExpo,
                EaseType.EaseInOutExpo => EaseInOutExpo,
                EaseType.EaseInCirc => EaseInCirc,
                EaseType.EaseOutCirc => EaseOutCirc,
                EaseType.EaseInOutCirc => EaseInOutCirc,
                EaseType.EaseInBounce => EaseInBounce,
                EaseType.EaseOutBounce => EaseOutBounce,
                EaseType.EaseInOutBounce => EaseInOutBounce,
                EaseType.EaseInBack => EaseInBack,
                EaseType.EaseOutBack => EaseOutBack,
                EaseType.EaseInOutBack => EaseInOutBack,
                EaseType.EaseInElastic => EaseInElastic,
                EaseType.EaseOutElastic => EaseOutElastic,
                EaseType.EaseInOutElastic => EaseInOutElastic,
                _ => SmoothStep,
            };
        }

        /// <summary>
        /// Like lerp with Ease in Ease out
        /// </summary>
        public static float Hermite(float phase) {
            return phase * phase * (3f - 2f * phase);
        }

        public static float HermiteStrong(float phase) {
            return phase * phase * phase * (phase * (6f * phase - 15f) + 10f);
        }

        public static bool IsFinished(float phase, bool forward) {
            return (phase == 1f && forward) || (phase == 0f && !forward);
        }

        public static float Lerp(float start, float end, float value) {
            return Mathf.Lerp(start, end, value);
        }

        // public static T Map<T>(ref float phase) where T : unmanaged, Enum {
        //     Split(phase, EnumTools<T>.count, out phase, out var intEnum);
        //     Assert.IsTrue(Enum.IsDefined(typeof(T), intEnum), $"Could not map {intEnum} to {typeof(T).Name}");
        //     return EnumTools<T>.From(intEnum);
        //}

        public static float Punch(float amplitude, float value) {
            float s = 9;
            if (value == 0) {
                return 0;
            }
            else if (value == 1) {
                return 0;
            }
            float period = 1 * 0.3f;
            s = period / (2 * Mathf.PI) * Mathf.Asin(0);
            return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
        }

        /// <summary>
        /// Like lerp with Ease in Ease out
        /// </summary>
        public static float SmoothStep(float start, float end, float value) {
            value = Mathf.Clamp(value, start, end);
            float v1 = (value - start) / (end - start);
            float v2 = (value - start) / (end - start);
            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }

        public static void Split(float phase, int count, out float subPhase, out int subPhaseNum) {
            var step = 1f / count;
            subPhaseNum = (int)Math.Truncate(phase / step);
            if (subPhaseNum >= count) {
                subPhaseNum = count - 1;
                subPhase = 1f;
            }
            else {
                subPhase = (phase % step) / step;
            }
        }

        // public static void Split<T>(float phase, int count, out float subPhase, out T subPhaseEnum) where T : unmanaged, Enum {
        //     Split(phase, count, out subPhase, out var intEnum);
        //     Assert.IsTrue(Enum.IsDefined(typeof(T), intEnum), $"Could not map {intEnum} to {nameof(T)}");
        //     subPhaseEnum = EnumTools<T>.From(intEnum);
        // }

        public static float SubPhase(float phase, float start, float end) {
            return Mathf.Clamp01((phase - start) / (end - start));
        }

        private static float PunchDefault(float start, float end, float value) {
            end -= start;
            return end * Punch(1f, value) + start;
        }
    }

    public enum EaseType {
        CLerp,
        Lerp,
        Berp,
        Punch,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        //bounce,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        //elastic,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    }

}