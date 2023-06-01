using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class VectorIntExtensions {
        public static Vector3Int Abs(in this Vector3Int v) {
            return new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector2Int Abs(in this Vector2Int v) {
            return new Vector2Int(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector3Int AddX(this Vector3Int input, int value) {
            input.x += value;
            return input;
        }

        public static Vector3Int AddY(this Vector3Int input, int value) {
            input.y += value;
            return input;
        }

        public static Vector3Int AddZ(this Vector3Int input, int value) {
            input.z += value;
            return input;
        }

        public static Vector3Int Get0XY(in this Vector2Int v) {
            return new Vector3Int(0, v.x, v.y);
        }

        public static Vector3Int Get0YX(in this Vector2Int v) {
            return new Vector3Int(0, v.y, v.x);
        }

        public static Vector3Int GetX0Y(in this Vector2Int v) {
            return new Vector3Int(v.x, 0, v.y);
        }

        public static Vector2Int GetXY(in this Vector3Int v) {
            return new Vector2Int(v.x, v.y);
        }

        public static Vector3Int GetXY0(in this Vector2Int v) {
            return new Vector3Int(v.x, v.y, 0);
        }

        public static Vector3Int GetXYZ(in this Vector3Int v) {
            return v;
        }

        public static Vector2Int GetXZ(in this Vector3Int v) {
            return new Vector2Int(v.x, v.z);
        }

        public static Vector3Int GetXZY(in this Vector3Int v) {
            return new Vector3Int(v.x, v.z, v.y);
        }

        public static Vector3Int GetY0X(in this Vector2Int v) {
            return new Vector3Int(v.y, 0, v.x);
        }

        public static Vector2Int GetYX(in this Vector3Int v) {
            return new Vector2Int(v.y, v.x);
        }

        public static Vector3Int GetYX0(in this Vector2Int v) {
            return new Vector3Int(v.y, v.x, 0);
        }

        public static Vector3Int GetYXZ(in this Vector3Int v) {
            return new Vector3Int(v.y, v.x, v.z);
        }

        public static Vector2Int GetYZ(in this Vector3Int v) {
            return new Vector2Int(v.y, v.z);
        }

        public static Vector3Int GetYZX(in this Vector3Int v) {
            return new Vector3Int(v.y, v.z, v.x);
        }

        public static Vector2Int GetZX(in this Vector3Int v) {
            return new Vector2Int(v.z, v.x);
        }

        public static Vector3Int GetZXY(in this Vector3Int v) {
            return new Vector3Int(v.z, v.x, v.y);
        }

        public static Vector2Int GetZY(in this Vector3Int v) {
            return new Vector2Int(v.z, v.y);
        }

        public static Vector3Int GetZYX(in this Vector3Int v) {
            return new Vector3Int(v.z, v.y, v.x);
        }

        public static Vector3Int ScaleX(this Vector3Int v, int xFactor) {
            v.x *= xFactor;
            return v;
        }

        public static Vector3Int ScaleXY(this Vector3Int v, int xFactor, int yFactor) {
            v.x *= xFactor;
            v.y *= yFactor;
            return v;
        }

        public static Vector3Int ScaleXYZ(this Vector3Int v, int xFactor, int yFactor, int zFactor) {
            v.x *= xFactor;
            v.y *= yFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3Int ScaleXZ(this Vector3Int v, int xFactor, int zFactor) {
            v.x *= xFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3Int ScaleY(this Vector3Int v, int yFactor) {
            v.y *= yFactor;
            return v;
        }

        public static Vector3Int ScaleYZ(this Vector3Int v, int yFactor, int zFactor) {
            v.y *= yFactor;
            v.z *= zFactor;
            return v;
        }

        public static Vector3Int ScaleZ(this Vector3Int v, int zFactor) {
            v.z *= zFactor;
            return v;
        }

        public static Vector3Int ToVector3Int(in this Vector2Int v, int z = 0) {
            return new Vector3Int(v.x, v.y, z);
        }

        public static float WeightedMagnitude(in this Vector3Int input, float xWeight, float yWeight, float zWeight) {
            var x = input.x * xWeight;
            var y = input.y * yWeight;
            var z = input.z * zWeight;
            var mag = x * x + y * y + z * z;
            return (mag >= 0) ? Mathf.Sqrt(mag) : 0;
        }

        public static float WeightedMagnitude(in this Vector2Int input, float xWeight, float yWeight) {
            var x = input.x * xWeight;
            var y = input.y * yWeight;
            var mag = x * x + y * y;
            return (mag >= 0) ? Mathf.Sqrt(mag) : 0;
        }

        public static Vector2Int WithX(this Vector2Int input, int value) {
            input.x = value;
            return input;
        }

        public static Vector3Int WithX(this Vector3Int input, int value) {
            input.x = value;
            return input;
        }

        public static Vector3Int WithXY(this Vector3Int input, int x, int y) {
            input.x = x;
            input.y = y;
            return input;
        }

        public static Vector3Int WithXY(this Vector3Int input, Vector2Int source) {
            input.x = source.x;
            input.y = source.y;
            return input;
        }

        public static Vector3Int WithXZ(this Vector3Int input, int x, int z) {
            input.x = x;
            input.z = z;
            return input;
        }

        public static Vector3Int WithXZ(this Vector3Int input, Vector2Int source) {
            input.x = source.x;
            input.z = source.y;
            return input;
        }

        public static Vector2Int WithY(this Vector2Int input, int value) {
            input.y = value;
            return input;
        }

        public static Vector3Int WithY(this Vector3Int input, int value) {
            input.y = value;
            return input;
        }

        public static Vector3Int WithYX(this Vector3Int input, Vector2Int source) {
            input.y = source.x;
            input.x = source.y;
            return input;
        }

        public static Vector3Int WithYZ(this Vector3Int input, int y, int z) {
            input.y = y;
            input.z = z;
            return input;
        }

        public static Vector3Int WithYZ(this Vector3Int input, Vector2Int source) {
            input.y = source.x;
            input.z = source.y;
            return input;
        }

        public static Vector3Int WithZ(this Vector3Int input, int value) {
            input.z = value;
            return input;
        }

        public static Vector3Int WithZX(this Vector3Int input, Vector2Int source) {
            input.z = source.x;
            input.x = source.y;
            return input;
        }

        public static Vector3Int WithZY(this Vector3Int input, Vector2Int source) {
            input.z = source.x;
            input.y = source.y;
            return input;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif