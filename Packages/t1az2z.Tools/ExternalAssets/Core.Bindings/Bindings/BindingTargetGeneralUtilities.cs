using System;
using System.Reflection;
using Core.Bindings.Tools;
using Core.Bindings.Tools.Extensions;
using UnityEngine;

namespace Binding.Base {
    public static class BindingTargetGeneralUtilities {
        private static readonly Type IPropertyType = typeof(IProperty);
        private static readonly Type IBindingTargetType = typeof(IBindingTarget);

        public static IProperty[] CollectProperties(IBindingTarget bindingTarget, out FieldInfo[] bindingTargetFieldInfos) {
            var bindingFieldInfos = new FieldInfo[0];
            var bindingProperties = new IProperty[0];
            var currentType = bindingTarget.GetType();
            while ((currentType != null) && IBindingTargetType.IsAssignableFrom(currentType)) {
                var fieldInfos =
                    currentType.GetCachedFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var fieldInfo in fieldInfos) {
                    var fieldType = fieldInfo.FieldType;
                    if (IBindingTargetType.IsAssignableFrom(fieldType) && !bindingFieldInfos.Contains(fieldInfo)) // Среди полей найден BindingTarget.
                    {
                        bindingFieldInfos = bindingFieldInfos.Add(fieldInfo);
                    }
                    else if (IPropertyType.IsAssignableFrom(fieldType) && (fieldInfo.GetValue(bindingTarget) is IProperty property) &&
                             !bindingProperties.Contains(property)) // Среди полей найден Property.
                    {
                        property.InstanceName = fieldInfo.Name;

                        bindingProperties = bindingProperties.Add(property);
                    }
                }

                currentType = currentType.BaseType;
            }

            bindingTargetFieldInfos = bindingFieldInfos;

            return bindingProperties;
        }

        public static MethodInfo GetMethod(IBindingTarget bindingTarget, string name, FieldInfo[] bindingTargetFieldInfos) {
            var method = bindingTarget.GetType().GetMethod(name);
            if (method != null) {
                return method;
            }

            if (!Application.isPlaying || bindingTargetFieldInfos.Length > 0) {
                for (int i = 0; i < bindingTargetFieldInfos.Length; i++) {
                    var bindingTargetFieldInfo = bindingTargetFieldInfos[i];
                    if (bindingTargetFieldInfo.GetValue(bindingTarget) is IBindingTarget nestedBindingTarget) {
                        if (bindingTargetFieldInfo.GetCustomAttribute<NotBindingTargetAttribute>() != null) {
                            continue;
                        }

                        if (nestedBindingTarget.GetMethod(name) is var result && (result != null)) {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}