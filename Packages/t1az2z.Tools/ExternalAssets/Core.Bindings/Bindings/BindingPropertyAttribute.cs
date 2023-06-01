using System;
using UnityEngine;

namespace Binding.Base {
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class BindingPropertyAttribute : PropertyAttribute {
        public Type BindingPropertyType = null;
        public Type BindingValueType = null;
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class BindingMethodAttribute : PropertyAttribute {
        public BindingMethodType Type = BindingMethodType.All;
    }

    public enum BindingMethodType {
        All,
        Action,
    }
}