using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class DictionaryExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V GetOrCreateDefault<K, V>(this Dictionary<K, V> dictionary, K key) where V: class, new() {
            if (dictionary.TryGetValue(key, out var v)) {
                return v;
            }

            v = new V();
            dictionary.Add(key, v);
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V Pop<K, V>(this Dictionary<K, V> dictionary, K key) {
            if (dictionary.TryGetValue(key, out var value)) {
                dictionary.Remove(key);
                return value;
            }

            return default(V);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push<K, V>(this Dictionary<K, V> dictionary, K key, V value) {
            dictionary[key] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAndRemove<K, V>(this Dictionary<K, V> dictionary, K key, out V value) {
            if (dictionary.TryGetValue(key, out value)) {
                dictionary.Remove(key);
                return true;
            }

            return false;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif