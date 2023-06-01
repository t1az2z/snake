using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class GameObjectExtensions {
        public static void Destroy(this GameObject gameObject) => GameObject.Destroy(gameObject);
        public static void DestroyImmediate(this GameObject gameObject) => GameObject.DestroyImmediate(gameObject);
        public static void Destroy(this Transform transform) => GameObject.Destroy(transform.gameObject);
        public static void DestroyImmediate(this Transform transform) => GameObject.DestroyImmediate(transform.gameObject);

        public static GameObject FindChildWithName(this GameObject go, string name) => go.transform.Find(name)?.gameObject;

        public static MonoBehaviour GetInterfacedComponentInParent<T>(this GameObject @this) => @this.GetComponentInParent<T>() as MonoBehaviour;
        public static MonoBehaviour GetInterfacedComponentInParent<T>(this Component @this) => @this.GetComponentInParent<T>() as MonoBehaviour;
        public static void GetChildInterfacedComponentRoots<T>(this Component @this, in IList<MonoBehaviour> list) {
            foreach (Transform itr in @this.transform) {
                var cmp = itr.GetComponent<T>() as MonoBehaviour;
                if (cmp) {
                    list.Add(cmp);
                }
                else {
                    GetChildInterfacedComponentRoots<T>(itr, list);
                }
            }
        }

        public static T GetComponent<T>(this Component @this, string childName) where T : Component => @this.gameObject.GetComponent<T>(childName);
        public static T GetComponent<T>(this GameObject @this, string childName) where T : Component => @this.transform.Find(childName)?.GetComponent<T>();

        public static T[] GetComponentsStrictInChildren<T>(this GameObject go, Type stopType) where T : Component {
            var result = new List<T>();
            foreach (Transform itr in go.transform) {
                var cmp = itr.GetComponent<T>();
                if (cmp) {
                    result.Add(cmp);
                }

                if (itr.GetComponent(stopType) == null) {
                    result.AddRange(itr.gameObject.GetComponentsStrictInChildren<T>(stopType));
                }
            }

            return result.ToArray();
        }

        public static T GetComponentStrictInParent<T>(this GameObject go) where T : Component {
            var result = new List<T>();
            var transform = go.transform;
            if (transform.parent == null) {
                return default;
            }
            else {
                var parent = transform.parent.gameObject;
                var cmp = parent.GetComponent<T>();
                if (cmp) {
                    return cmp;
                }

                return parent.GetComponentStrictInParent<T>();
            }
        }

        public static string GetFullPath(this GameObject obj) => obj.transform.GetFullPath();
        public static string GetFullPath(this Component obj) {
            var builder = new StringBuilder();

            builder.Append(obj.transform.name);

            var p = obj.transform.parent;
            while (p != null) {
                builder.Insert(0, p.name + "/");
                p = p.parent;
            }

            return builder.ToString();
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component => component.gameObject.GetOrAddComponent<T>();
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
            if (go == null) {
                return null;
            }

            T comp = go.GetComponent<T>();

            if (comp == null) {
                comp = go.AddComponent<T>();
            }

            return comp;
        }

        public static void ReplaceLayerRecursively(this GameObject go, int fromLayer, int toLayer) {
            if (go.layer == fromLayer) {
                go.layer = toLayer;
            }

            foreach (Transform child in go.transform) {
                child.gameObject.ReplaceLayerRecursively(fromLayer, toLayer);
            }
        }

        public static void SetLayerRecursively(this GameObject go, int layer) {
            go.layer = layer;

            foreach (Transform child in go.transform) {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        public static GameObject Spawn(this UnityEngine.Object original, bool activated = true) =>
            Internal_InstantiateGameObject(original, activated, (go) => GameObject.Instantiate(go));

        public static GameObject Spawn(this UnityEngine.Object original, Transform parent, bool activated = true) =>
            Internal_InstantiateGameObject(original, activated, (go) => GameObject.Instantiate(go, parent));

        public static GameObject Spawn(this UnityEngine.Object original, Vector3 position, Quaternion rotation, bool activated = true) =>
            Internal_InstantiateGameObject(original, activated, (go) => GameObject.Instantiate(go, position, rotation));

        public static GameObject Spawn(this UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent,
                                       bool activated = true) =>
            Internal_InstantiateGameObject(original, activated, (go) => GameObject.Instantiate(go, position, rotation, parent));

        public static T Spawn<T>(this UnityEngine.Object original, bool activated = true) where T : Component =>
            Internal_InstantiateGameObjectAndExtractComponent<T>(original, activated, (go) => GameObject.Instantiate(go));

        public static T Spawn<T>(this UnityEngine.Object original, Transform parent, bool activated = true) where T : Component =>
            Internal_InstantiateGameObjectAndExtractComponent<T>(original, activated, (go) => GameObject.Instantiate(go, parent));

        public static T Spawn<T>(this UnityEngine.Object original, Vector3 position, Quaternion rotation, bool activated = true)
            where T : Component =>
            Internal_InstantiateGameObjectAndExtractComponent<T>(original, activated, (go) => GameObject.Instantiate(go, position, rotation));

        public static T Spawn<T>(this UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent, bool activated = true)
            where T : Component =>
            Internal_InstantiateGameObjectAndExtractComponent<T>(original, activated, (go) => GameObject.Instantiate(go, position, rotation, parent));

        private static GameObject Internal_ExtractGameObject(this UnityEngine.Object original) {
            if (original is GameObject go) {
                return go;
            }

            if (original is Transform trf) {
                return trf.gameObject;
            }

            if (original is Component cmp) {
                return cmp.gameObject;
            }

            return null;
        }

        private static GameObject Internal_InstantiateGameObject(this UnityEngine.Object original, bool activated,
                                                                 Func<GameObject, GameObject> constructor) {
            var go = original.Internal_ExtractGameObject();
            if (go == null) {
                return null;
            }

            var oldState = go.activeSelf;
            go.SetActive(false);
            var result = constructor.Invoke(go);
            try {
                go.SetActive(oldState);
                result.SetActive(activated);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            return result.Internal_ExtractGameObject();
        }

        private static T Internal_InstantiateGameObjectAndExtractComponent<T>(this UnityEngine.Object original, bool activated,
                                                                              Func<GameObject, GameObject> constructor) where T : Component {
            var go = Internal_InstantiateGameObject(original, activated, constructor);
            return go?.GetOrAddComponent<T>();
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif