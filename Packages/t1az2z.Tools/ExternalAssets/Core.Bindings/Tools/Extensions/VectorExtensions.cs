using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class VectorExtensions {
        public static Vector3 Abs(in this Vector3 v) {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector2 Abs(in this Vector2 v) {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector3 AddX(this Vector3 input, float value) {
            input.x += value;
            return input;
        }

        public static Vector3 AddY(this Vector3 input, float value) {
            input.y += value;
            return input;
        }

        public static Vector3 AddZ(this Vector3 input, float value) {
            input.z += value;
            return input;
        }

        public static Vector3 Get0XY(in this Vector2 v) {
            return new Vector3(0f, v.x, v.y);
        }

        public static Vector3 Get0YX(in this Vector2 v) {
            return new Vector3(0f, v.y, v.x);
        }

        public static Vector3 GetX0Y(in this Vector2 v) {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector2 GetXY(in this Vector3 v) {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 GetXY0(in this Vector2 v) {
            return new Vector3(v.x, v.y, 0f);
        }

        public static Vector3 GetXYZ(in this Vector3 v) {
            return v;
        }

        public static Vector2 GetXZ(in this Vector3 v) {
            return new Vector2(v.x, v.z);
        }

        public static Vector3 GetXZY(in this Vector3 v) {
            return new Vector3(v.x, v.z, v.y);
        }

        public static Vector3 GetY0X(in this Vector2 v) {
            return new Vector3(v.y, 0f, v.x);
        }

        public static Vector2 GetYX(in this Vector3 v) {
            return new Vector2(v.y, v.x);
        }

        public static Vector3 GetYX0(in this Vector2 v) {
            return new Vector3(v.y, v.x, 0f);
        }

        public static Vector3 GetYXZ(in this Vector3 v) {
            return new Vector3(v.y, v.x, v.z);
        }

        public static Vector2 GetYZ(in this Vector3 v) {
            return new Vector2(v.y, v.z);
        }

        public static Vector3 GetYZX(in this Vector3 v) {
            return new Vector3(v.y, v.z, v.x);
        }

        public static Vector2 GetZX(in this Vector3 v) {
            return new Vector2(v.z, v.x);
        }

        public static Vector3 GetZXY(in this Vector3 v) {
            return new Vector3(v.z, v.x, v.y);
        }

        public static Vector2 GetZY(in this Vector3 v) {
            return new Vector2(v.z, v.y);
        }

        public static Vector3 GetZYX(in this Vector3 v) {
            return new Vector3(v.z, v.y, v.x);
        }

        public static bool IsZero(in this Vector2 v) => v == Vector2.zero;
        public static bool IsZero(in this Vector3 v) => v == Vector3.zero;
        public static bool IsZero(in this Vector2 v, float Eps) => v.sqrMagnitude <= Eps * Eps;
        public static bool IsZero(in this Vector3 v, float Eps) => v.sqrMagnitude <= Eps * Eps;

        public static Vector3 ScaleX(this Vector3 v, float xFactor) {
            v.x *= xFactor;
            return v;
        }

        public static Vector3 ScaleXY(this Vector3 v, float xFactor, float yFactor) {
            v.x *= xFactor;
            v.y *= yFactor;
            return v;
        }

        public static Vector3 ScaleXYZ(this Vector3 v, float xFactor, float yFactor, float zFactor) {
            v.x *= xFactor;
            v.y *= yFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3 ScaleXZ(this Vector3 v, float xFactor, float zFactor) {
            v.x *= xFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3 ScaleY(this Vector3 v, float yFactor) {
            v.y *= yFactor;
            return v;
        }

        public static Vector3 ScaleYZ(this Vector3 v, float yFactor, float zFactor) {
            v.y *= yFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3 ScaleZ(this Vector3 v, float zFactor) {
            v.z *= zFactor;
            return v;
        }

        public static Vector3 ToVector3(in this Vector2 v, float z = 0f) {
            return new Vector3(v.x, v.y, z);
        }

        public static float WeightedMagnitude(in this Vector3 input, float xWeight, float yWeight, float zWeight) {
            var x = input.x * xWeight;
            var y = input.y * yWeight;
            var z = input.z * zWeight;
            var mag = x * x + y * y + z * z;
            return (mag >= 0f) ? Mathf.Sqrt(mag) : 0f;
        }

        public static float WeightedMagnitude(in this Vector2 input, float xWeight, float yWeight) {
            var x = input.x * xWeight;
            var y = input.y * yWeight;
            var mag = x * x + y * y;
            return (mag >= 0f) ? Mathf.Sqrt(mag) : 0f;
        }

        public static Vector2 WithX(this Vector2 input, float value) {
            input.x = value;
            return input;
        }

        public static Vector3 WithX(this Vector3 input, float value) {
            input.x = value;
            return input;
        }

        public static Vector3 WithXY(this Vector3 input, float x, float y) {
            input.x = x;
            input.y = y;
            return input;
        }

        public static Vector3 WithXY(this Vector3 input, Vector2 source) {
            input.x = source.x;
            input.y = source.y;
            return input;
        }

        public static Vector3 WithXZ(this Vector3 input, float x, float z) {
            input.x = x;
            input.z = z;
            return input;
        }

        public static Vector3 WithXZ(this Vector3 input, Vector2 source) {
            input.x = source.x;
            input.z = source.y;
            return input;
        }

        public static Vector2 WithY(this Vector2 input, float value) {
            input.y = value;
            return input;
        }

        public static Vector3 WithY(this Vector3 input, float value) {
            input.y = value;
            return input;
        }

        public static Vector3 WithYX(this Vector3 input, Vector2 source) {
            input.y = source.x;
            input.x = source.y;
            return input;
        }

        public static Vector3 WithYZ(this Vector3 input, float y, float z) {
            input.y = y;
            input.z = z;
            return input;
        }

        public static Vector3 WithYZ(this Vector3 input, Vector2 source) {
            input.y = source.x;
            input.z = source.y;
            return input;
        }

        public static Vector3 WithZ(this Vector3 input, float value) {
            input.z = value;
            return input;
        }

        public static Vector3 WithZX(this Vector3 input, Vector2 source) {
            input.z = source.x;
            input.x = source.y;
            return input;
        }

        public static Vector3 WithZY(this Vector3 input, Vector2 source) {
            input.z = source.x;
            input.y = source.y;
            return input;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif