using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif

    public static class CollectionExtensions {
        public static T AddAndReturn<T>(this ICollection<T> collection, T item) {
            collection.Add(item);
            return item;
        }

        public static void AddFromEnumOfSameType<T>(this List<T> self) {
            foreach (T enumElement in Enum.GetValues(typeof(T))) {
                self.Add(enumElement);
            }
        }

        public static bool Contains<T>(this IEnumerable<T> self, T itemToFind) {
            foreach (var item in self) {
                if (item.Equals(itemToFind)) {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains(this Hashtable self, string key) {
            foreach (var item in self) {
                if (item.ToString() == key) {
                    return true;
                }
            }

            return false;
        }

        public static int CountOf<T>(this IEnumerable<T> self, T itemToFind) {
            var result = 0;
            foreach (var item in self) {
                if (itemToFind.Equals(item)) {
                    result++;
                }
            }

            return result;
        }

        public static T First<T>(this IList<T> list) {
            if (list.Count <= 0) {
                return default(T);
            }

            return list[0];
        }

        // Gets "1,2,3" from list of "1" "2" 3" if separator is ","
        public static string GenerateSeparatedString<T>(this List<T> self, string separator) {
            var result = "";

            var index = 0;
            foreach (var item in self) {
                result += item.ToString();

                if (index != self.Count - 1) {
                    result += separator;
                }

                ++index;
            }

            return result;
        }

        public static int GetClampedIndex<T>(this IList<T> list, int indexForClamp) {
            return Mathf.Clamp(indexForClamp, 0, list.Count);
        }

        public static int GetLoopedIndex<T>(this IList<T> list, int indexForLoop) {
            if (indexForLoop >= 0) {
                return indexForLoop < list.Count ? indexForLoop : 0;
            }

            return list.Count - 1;
        }

        public static T GetRandomElement<T>(this List<T> self) {
            if (self.Count > 0) {
                return self[UnityEngine.Random.Range(0, self.Count)];
            }
            else {
                return default(T);
            }
        }

        public static List<T> GetRange<T>(this IList<T> list, int index, int count) {
            var maxCount = Math.Min(count, Math.Max(0, list.Count - index));
            var result = new List<T>(maxCount);
            for (int i = 0; i < maxCount; ++i) {
                result.Add(list[index + i]);
            }

            return result;
        }

        public static List<T> GetShuffled<T>(this List<T> self) {
            return self.OrderBy(r => UnityEngine.Random.value).ToList();
        }

        public static bool HasAny<T>(this ICollection<T> collection, Func<T, bool> func) {
            if (func == null) {
                throw new ArgumentException("Func for HasAny is empty");
            }

            foreach (T item in collection) {
                if (func(item)) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNoneOf<T>(this T valueToFind, params T[] valuesToCheck) {
            return !IsOneOf(valueToFind, valuesToCheck);
        }

        public static bool IsNullOrEmpty(this ICollection collection) {
            return (collection == null) || (collection.Count < 1);
        }

        public static bool IsNullOrEmpty<T>(this T[] array) {
            return (array == null) || (array.Length < 1);
        }

        public static bool IsOneOf<T>(this T valueToFind, params T[] valuesToCheck) {
            foreach (var value in valuesToCheck) {
                if (value.Equals(valueToFind)) {
                    return true;
                }
            }

            return false;
        }
        public static bool IsValidIndex<T>(this IList<T> list, int index) {
            return (list != null) && ((uint)index < (uint)list.Count);
        }

        public static T Last<T>(this IList<T> list) {
            var index = list.Count - 1;
            if (index < 0) {
                return default(T);
            }

            return list[index];
        }

        public static TR[] ConvertToArray<T, TR>(this ICollection<T> collection, Func<T, TR> provider) {
            if (provider == null) {
                throw new ArgumentException("Provider for Map is empty");
            }

            var result = new TR[collection.Count];
            var i = 0;
            foreach (T item in collection) {
                result[i++] = provider(item);
            }

            return result;
        }

        public static List<TR> Convert<T, TR>(this ICollection<T> collection, Func<T, TR> provider) {
            if (provider == null) {
                throw new ArgumentException("Provider for Map is empty");
            }

            var result = new List<TR>(collection.Count);

            foreach (T item in collection) {
                result.Add(provider(item));
            }

            return result;
        }

        public static T PopLast<T>(this IList<T> list) {
            var index = list.Count - 1;
            if (index < 0) {
                return default(T);
            }

            var popped = list[index];
            list.RemoveAt(index);
            return popped;
        }

        public static TR Reduce<T, TR>(this ICollection<T> collection, TR initialValue, Func<T, TR, TR> func) {
            if (func == null) {
                throw new ArgumentException("Func for Reduce is empty");
            }

            var accum = initialValue;
            foreach (T item in collection) {
                accum = func(item, accum);
            }

            return accum;
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif