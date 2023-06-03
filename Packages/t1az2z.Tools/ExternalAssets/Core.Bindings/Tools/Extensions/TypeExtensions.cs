using System;

#if !BASE_GLOBAL_EXTENSIONS
namespace Core.Bindings.Tools.Extensions {
#endif
    public static class TypeExtensions {
        public static bool InheritedFromType(this Type type, Type targetType, Type endType = null) {
            var currentType = type;
            var cycleBreakType = endType != null ? endType : typeof(object);
            if (currentType.BaseType != null) {
                while (currentType != cycleBreakType) {
                    if (currentType == targetType) {
                        return true;
                    }

                    currentType = currentType.BaseType;
                }
            }

            return false;
        }

        public static bool InheritedOrInterfacedFromType(this Type type, Type targetType, Type endType = null) {
            if (!targetType.IsInterface) {
                return InheritedFromType(type, targetType, endType);
            }

            var currentType = type;
            var cycleBreakType = endType != null ? endType : typeof(object);
            if (currentType.BaseType != null) {
                while (currentType != cycleBreakType) {
                    if (currentType.FindInterfaces(InterfaceStrongComparator, targetType).Length > 0) {
                        return true;
                    }
                    currentType = currentType.BaseType;
                }
            }

            return false;
        }

        public static bool InterfaceStrongComparator(Type typeObj, Object criteriaObj) => typeObj == (criteriaObj as Type);

        public static bool IsGenericType(this Type typeToCheck, Type genericType) => typeToCheck.IsGenericType(genericType, out _);

        public static bool IsGenericType(this Type typeToCheck, Type genericType, out Type concreteGenericType) {
            while (true) {
                concreteGenericType = null;

                if (genericType == null) {
                    throw new ArgumentNullException(nameof(genericType));
                }

                if (!genericType.IsGenericTypeDefinition) {
                    throw new ArgumentException("The definition should be a GenericTypeDefinition", nameof(genericType));
                }

                if (typeToCheck == null || typeToCheck == typeof(object)) {
                    return false;
                }

                if (typeToCheck == genericType) {
                    concreteGenericType = typeToCheck;
                    return true;
                }

                if ((typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck) == genericType) {
                    concreteGenericType = typeToCheck;
                    return true;
                }

                if (genericType.IsInterface) {
                    foreach (var i in typeToCheck.GetInterfaces())
                        if (i.IsGenericType(genericType, out concreteGenericType)) {
                            return true;
                        }
                }

                typeToCheck = typeToCheck.BaseType;
            }
        }
    }
}