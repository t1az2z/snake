using System.Collections.Generic;

namespace Core.Bindings.ExternalAssets.Core.Bindings.Tools {
    public class MultiKeyDictionary<K1, K2, V> : Dictionary<K1, Dictionary<K2, V>> {

        public V this[K1 key1, K2 key2] {
            get => this[key1][key2];
            set {
                if (!this.TryGetValue(key1, out var secondThis)) {
                    secondThis = new Dictionary<K2, V>();
                    this[key1] = secondThis;
                }
                secondThis[key2] = value;
            }
        }

        public void Add(K1 key1, K2 key2, V value) {
            if (!this.TryGetValue(key1, out var secondThis)) {
                secondThis = new Dictionary<K2, V>();
                Add(key1, secondThis);
            }
            secondThis.Add(key2, value);
        }

        public bool ContainsKeys(K1 key1, K2 key2) => ContainsKey(key1) && this[key1].ContainsKey(key2);

        public bool TryGetValue(K1 key1, K2 key2, out V value) {
            if (!TryGetValue(key1, out var secondThis)) {
                value = default;
                return false;
            }

            return secondThis.TryGetValue(key2, out value);
        }
    }


    public class MultiKeyDictionary<K1, K2, K3, V> : Dictionary<K1, MultiKeyDictionary<K2, K3, V>> {
        public V this[K1 key1, K2 key2, K3 key3] {
            get => this[key1][key2, key3];
            set {
                if (!this.TryGetValue(key1, out var secondThis)) {
                    secondThis = new MultiKeyDictionary<K2, K3, V>();
                    this[key1] = secondThis;
                }
                secondThis[key2, key3] = value;
            }
        }

        public bool ContainsKeys(K1 key1, K2 key2, K3 key3) => ContainsKey(key1) && this[key1].ContainsKeys(key2, key3);

        public bool TryGetValue(K1 key1, K2 key2, K3 key3, out V value) {
            if (!TryGetValue(key1, out var secondThis)) {
                value = default;
                return false;
            }

            return secondThis.TryGetValue(key2, key3, out value);
        }
    }

    public class MultiKeyDictionary<K1, K2, K3, K4, V> : Dictionary<K1, MultiKeyDictionary<K2, K3, K4, V>> {
        public V this[K1 key1, K2 key2, K3 key3, K4 key4] {
            get => this[key1][key2, key3, key4];
            set {
                if (!this.TryGetValue(key1, out var secondThis)) {
                    secondThis = new MultiKeyDictionary<K2, K3, K4, V>();
                    this[key1] = secondThis;
                }
                secondThis[key2, key3, key4] = value;
            }
        }

        public bool ContainsKeys(K1 key1, K2 key2, K3 key3, K4 key4) => ContainsKey(key1) && this[key1].ContainsKeys(key2, key3, key4);

        public bool TryGetValue(K1 key1, K2 key2, K3 key3, K4 key4, out V value) {
            if (!TryGetValue(key1, out var secondThis)) {
                value = default;
                return false;
            }

            return secondThis.TryGetValue(key2, key3, key4, out value);
        }
    }

    public class MultiKeyDictionary<K1, K2, K3, K4, K5, V> : Dictionary<K1, MultiKeyDictionary<K2, K3, K4, K5, V>> {
        public V this[K1 key1, K2 key2, K3 key3, K4 key4, K5 key5] {
            get => this[key1][key2, key3, key4, key5];
            set {
                if (!this.TryGetValue(key1, out var secondThis)) {
                    secondThis = new MultiKeyDictionary<K2, K3, K4, K5, V>();
                    this[key1] = secondThis;
                }
                secondThis[key2, key3, key4, key5] = value;
            }
        }

        public bool ContainsKeys(K1 key1, K2 key2, K3 key3, K4 key4, K5 key5) => ContainsKey(key1) && this[key1].ContainsKeys(key2, key3, key4, key5);

        public bool TryGetValue(K1 key1, K2 key2, K3 key3, K4 key4, K5 key5, out V value) {
            if (!TryGetValue(key1, out var secondThis)) {
                value = default;
                return false;
            }

            return secondThis.TryGetValue(key2, key3, key4, key5, out value);
        }
    }
}