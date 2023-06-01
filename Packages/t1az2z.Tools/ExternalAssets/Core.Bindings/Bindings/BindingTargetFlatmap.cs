using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Bindings.Tools;

namespace Binding.Base {
    public static class BindingTargetFlatmap {
        private static Dictionary<Type, PropertiesFlatmap> _cache = new Dictionary<Type, PropertiesFlatmap>();
        public class PropertiesFlatmap {
            public Dictionary<string, FieldInfo> Properties = new Dictionary<string, FieldInfo>();
        }

        public static PropertiesFlatmap GetCachedFlatmap(Type type) {
            if (_cache.TryGetValue(type, out var result)) {
                return result;
            }

            result = new PropertiesFlatmap();
            _cache[type] = result;
            var fieldInfos = type.GetCachedFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fieldInfos) {
                var fieldType = fieldInfo.FieldType;
                if (typeof(IProperty).IsAssignableFrom(fieldType)) {
                    result.Properties[fieldInfo.Name] = fieldInfo;
                }
            }

            return result;
        }

        public static FieldInfo GetPropertyFieldInfo(IBindingTarget target, string name) {
            if (target == null) {
                return null;
            }

            var type = target.GetType();
            var map = GetCachedFlatmap(type).Properties;
            return map.TryGetValue(name, out var result) ? result : null;
        }
        
        public static FieldInfo GetPropertyFieldInfo(Type type, string name) {
            if (type == null) {
                return null;
            }

            var map = GetCachedFlatmap(type).Properties;
            return map.TryGetValue(name, out var result) ? result : null;
        }

        public static IProperty GetProperty(IBindingTarget target, string name) {
            if (target == null) {
                return null;
            }

            var type = target.GetType();
            var map = GetCachedFlatmap(type).Properties;
            return map.TryGetValue(name, out var result) ? result.GetValue(target) as IProperty : null;
        }

        public static MethodInfo GetMethod(IBindingTarget target, string name) {
            return target?.GetType().GetMethod(name);
        }
    }
}