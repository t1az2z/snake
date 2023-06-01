using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class TransformPRSExtensions {
        public static void AddChildResetPRS(this Transform obj, GameObject child) {
            child.transform.parent = obj;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this GameObject obj, GameObject child) {
            child.transform.parent = obj.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this Transform obj, Component child) {
            child.transform.parent = obj;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this GameObject obj, Component child) {
            child.transform.parent = obj.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this Transform obj, GameObject child, Vector3 localPosition) {
            child.transform.parent = obj;
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this GameObject obj, GameObject child, Vector3 localPosition) {
            child.transform.parent = obj.transform;
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this Transform obj, Component child, Vector3 localPosition) {
            child.transform.parent = obj;
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AddChildResetPRS(this GameObject obj, Component child, Vector3 localPosition) {
            child.transform.parent = obj.transform;
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
        }

        public static void AlignAs(this GameObject obj, GameObject other) {
            obj.transform.position = other.transform.position;
            obj.transform.rotation = other.transform.rotation;
            obj.transform.localScale = other.transform.localScale;
        }

        public static void AlignAs(this GameObject obj, Transform other) {
            obj.transform.position = other.position;
            obj.transform.rotation = other.rotation;
            obj.transform.localScale = other.localScale;
        }

        public static void ResetPRS(this Transform obj) {
            obj.localPosition = Vector3.zero;
            obj.localRotation = Quaternion.identity;
            obj.localScale = Vector3.one;
        }

        public static void ResetPRS(this GameObject obj) {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }

        public static void ResetPRS(this Component obj) {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }

        public static void SetLocalX(this Transform tr, float x) {
            Vector3 t = tr.localPosition;
            t.x = x;
            tr.localPosition = t;
        }

        public static void SetLocalY(this Transform tr, float y) {
            Vector3 t = tr.localPosition;
            t.y = y;
            tr.localPosition = t;
        }

        public static void SetLocalZ(this Transform tr, float z) {
            Vector3 t = tr.localPosition;
            t.z = z;
            tr.localPosition = t;
        }

        public static void SetX(this Transform tr, float x) {
            Vector3 t = tr.position;
            t.x = x;
            tr.position = t;
        }

        public static void SetY(this Transform tr, float y) {
            Vector3 t = tr.position;
            t.y = y;
            tr.position = t;
        }

        public static void SetZ(this Transform tr, float z) {
            Vector3 t = tr.position;
            t.z = z;
            tr.position = t;
        }

        public static void ShiftLocalX(this Transform tr, float ox) {
            Vector3 t = tr.localPosition;
            t.x += ox;
            tr.localPosition = t;
        }

        public static void ShiftLocalY(this Transform tr, float oy) {
            Vector3 t = tr.localPosition;
            t.y += oy;
            tr.localPosition = t;
        }

        public static void ShiftLocalZ(this Transform tr, float oz) {
            Vector3 t = tr.localPosition;
            t.z += oz;
            tr.localPosition = t;
        }

        public static void ShiftX(this Transform tr, float ox) {
            Vector3 t = tr.position;
            t.x += ox;
            tr.position = t;
        }

        public static void ShiftY(this Transform tr, float oy) {
            Vector3 t = tr.position;
            t.y += oy;
            tr.position = t;
        }

        public static void ShiftZ(this Transform tr, float oz) {
            Vector3 t = tr.position;
            t.z += oz;
            tr.position = t;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif