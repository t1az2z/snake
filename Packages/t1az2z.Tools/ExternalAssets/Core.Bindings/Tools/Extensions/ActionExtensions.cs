using System;
#if !UNITY_EDITOR
using UnityEngine;
#endif

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class ActionExtensions {
        public static void TryCatchCall(this Action action) {
#if !UNITY_EDITOR
        try
        {
#endif
            action?.Invoke();
#if !UNITY_EDITOR
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
#endif
        }

        public static void TryCatchCall<T>(this Action<T> action, T param) {
#if !UNITY_EDITOR
        try
        {
#endif
            action?.Invoke(param);
#if !UNITY_EDITOR
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
#endif
        }

        public static void TryCatchCall<T, T2>(this Action<T, T2> action, T param, T2 param2) {
#if !UNITY_EDITOR
        try
        {
#endif
            action?.Invoke(param, param2);
#if !UNITY_EDITOR
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
#endif
        }

        public static void TryCatchCall<T, T2, T3>(this Action<T, T2, T3> action, T param, T2 param2, T3 param3) {
#if !UNITY_EDITOR
        try
        {
#endif
            action?.Invoke(param, param2, param3);
#if !UNITY_EDITOR
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
#endif
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif