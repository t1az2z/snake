using System;
using System.Collections.Generic;
using Core.Bindings.Tools.Rx;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Helpers {
#endif
    public static partial class StringTools {
        public static class StringCache {
            private static readonly Dictionary<string, CacheElement> _cache = new Dictionary<string, CacheElement>();

            public static IDisposable Populate(long from, long to, long step = 1, string format = "{0}") {
                if (_cache.TryGetValue(format, out var strings) == false) {
                    strings = new CacheElement();
                    _cache[format] = strings;
                }
                strings.UsageCount++;
                for (var i = from; i <= to; i += step) {
                    if (strings.Items.ContainsKey(i)) continue;
                    strings.Items[i] = string.Format(format, i);
                }
                return Disposable.Create(() => {
                    strings.UsageCount--;
                    if (strings.UsageCount <= 0) {
                        _cache.Remove(format);
                    }
                });
            }

            public static string Get(long value, string format = "{0}", bool allowAllocation = true) {
                if (_cache.TryGetValue(format, out var strings) && strings.Items.TryGetValue(value, out var cached)) {
                    return cached;
                }

                if (allowAllocation) {
                    return string.Format(format, value);
                }
                
                throw new ArgumentOutOfRangeException($"String cache not found for format {format} with value of {value}!");
            }
            
            private class CacheElement {
                public int UsageCount = 0;
                public readonly Dictionary<long, string> Items = new Dictionary<long, string>();
            }
        }
    }

#if !BASE_GLOBAL_EXTENSIONS
}
#endif