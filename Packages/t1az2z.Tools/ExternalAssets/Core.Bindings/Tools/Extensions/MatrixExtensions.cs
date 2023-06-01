using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class MatrixExtenstions {
        public static Vector3 ExtractPosition(this Matrix4x4 matrix) {
            return new Vector3(matrix.m03, matrix.m13, matrix.m23);
        }

        public static Quaternion ExtractRotation(this Matrix4x4 matrix) {
            var forward = new Vector3(matrix.m02, matrix.m12, matrix.m22);
            var upwards = new Vector3(matrix.m01, matrix.m11, matrix.m21);
            return Quaternion.LookRotation(forward, upwards);
        }

        public static Vector3 ExtractScale(this Matrix4x4 matrix) {
            var x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
            var y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
            var z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
            return new Vector3(x, y, z);
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif