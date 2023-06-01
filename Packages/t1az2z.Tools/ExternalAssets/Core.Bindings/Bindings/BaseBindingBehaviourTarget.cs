using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Binding.Base {
    public class BaseBindingBehaviourTarget : MonoBehaviour, IBindingTarget {
        public event Action OnForceUpdateProperties;

        protected void ForceUpdateProperties() => OnForceUpdateProperties?.Invoke();
        MethodInfo IBindingTarget.GetMethod(string name) => BindingTargetFlatmap.GetMethod(this, name);
        IProperty IBindingTarget.GetProperty(string name) => BindingTargetFlatmap.GetProperty(this, name);
        bool IBindingTarget.Editor_HasProperty(string name) => BindingTargetFlatmap.GetPropertyFieldInfo(this.GetType(), name) != null;
        Dictionary<string, FieldInfo> IBindingTarget.Editor_GetPropertiesFlatmap() => BindingTargetFlatmap.GetCachedFlatmap(GetType()).Properties;
    }
}