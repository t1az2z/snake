using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Bindings.Tools.Extensions;
#if !BASE_GLOBAL_EXTENSIONS
#endif

namespace Core.Bindings.Tools {
    using FieldsDictionary   = Dictionary<int    /*Type.GetHashCode*/, FieldInfo[]>;
    using MethodsDictionary  = Dictionary<int    /*Type.GetHashCode*/, MethodInfo[]>;
    using PropertyDictionary = Dictionary<int    /*Type.GetHashCode*/, PropertyInfo[]>;

    public static class DynamicTypesManager {
        public interface IInheritanceCacheable { }
        private static Type[] _emptyTypes = new Type[0];

        private static HashSet<string> _cachedAssemblies = new HashSet<string>();
        private static Dictionary<Type, List<Type>> _typesByBaseType = new Dictionary<Type, List<Type>>();

        private static Dictionary<BindingFlags, FieldsDictionary> _fieldsCache = new Dictionary<BindingFlags, FieldsDictionary>(64);
        private static Dictionary<BindingFlags, PropertyDictionary> _propertiesCache = new Dictionary<BindingFlags, PropertyDictionary>(64);
        private static Dictionary<BindingFlags, MethodsDictionary> _methodsCache = new Dictionary<BindingFlags, MethodsDictionary>(64);

        private static object _lockedObject = new object();

        static DynamicTypesManager() {
            var self = typeof(IInheritanceCacheable).Assembly.GetName();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var canCache = false;
                foreach(var reference in assembly.GetReferencedAssemblies()) {
                    if (AssemblyName.ReferenceMatchesDefinition(reference, self)) {
                        canCache = true;
                        break;
                    }
                }

                if (canCache) {
                    CacheAssemblyTypes(assembly);
                }
            }
        }

        private static void CacheAssemblyTypes(Assembly assembly) {
            if (assembly == null) {
                return;
            }

            lock (_lockedObject) {
                var name = assembly.FullName;
                if (_cachedAssemblies.Contains(name)) {
                    return;
                }
                _cachedAssemblies.Add(name);

                foreach (var type in assembly.GetTypes()) {
                    if (type.IsAbstract) {
                        continue;
                    }

                    foreach (Type @interface in type.GetInterfaces()) {
                        if (@interface == typeof(IInheritanceCacheable)) {
                            var baseType = type.BaseType;
                            while (baseType != typeof(object)) {
                                if (!baseType.IsGenericType) {
                                    _typesByBaseType.GetOrCreateDefault(baseType).Add(type);
                                }

                                baseType = baseType.BaseType;
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns inherited classes that are marked with an interface <see cref="IInheritanceCacheable">MakeInheritanceCache</see>
        /// </summary>
        public static IList<Type> GetCachedDerivedTypes<T>() where T: IInheritanceCacheable {
            lock (_lockedObject) {
                if (_typesByBaseType.TryGetValue(typeof(T), out var list)) {
                    return list;
                }

                return _emptyTypes;
            }
        }

        public static FieldInfo[] GetCachedFields(this Type type, BindingFlags bindFlags) {
            lock (_lockedObject) {
                var fields = _fieldsCache.GetOrCreateDefault(bindFlags);

                FieldInfo[] result;
                int hash = type.GetHashCode();
                if (fields.TryGetValue(hash, out result)) {
                    return result;
                }

                result = type.GetFields(bindFlags);
                fields[hash] = result;
                return result;
            }
        }

        public static PropertyInfo[] GetCachedProperties(this Type type, BindingFlags bindFlags) {
            lock (_lockedObject) {
                var properties = _propertiesCache.GetOrCreateDefault(bindFlags);

                PropertyInfo[] result;
                int hash = type.GetHashCode();
                if (properties.TryGetValue(hash, out result)) {
                    return result;
                }

                result = type.GetProperties(bindFlags);
                properties[hash] = result;
                return result;
            }
        }

        public static MethodInfo[] GetCachedMethods(this Type type, BindingFlags bindFlags) {
            lock (_lockedObject) {
                var methods = _methodsCache.GetOrCreateDefault(bindFlags);

                MethodInfo[] result;
                int hash = type.GetHashCode();
                if (methods.TryGetValue(hash, out result)) {
                    return result;
                }

                result = type.GetMethods(bindFlags);
                methods[hash] = result;
                return result;
            }
        }
    }
}