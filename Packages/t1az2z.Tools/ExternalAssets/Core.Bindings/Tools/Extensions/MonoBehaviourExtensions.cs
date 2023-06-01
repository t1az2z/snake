using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class MonoBehaviourExtensions {
        public static void StopAndNullCoroutine(this MonoBehaviour self, ref Coroutine routine) {
            if (routine != null) {
                self.StopCoroutine(routine);
                routine = null;
            }
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif