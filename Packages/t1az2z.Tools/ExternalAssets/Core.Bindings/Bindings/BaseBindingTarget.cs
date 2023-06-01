using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
#if !BASE_GLOBAL_EXTENSIONS
#endif

namespace Binding.Base {
    public class BaseBindingTarget : IBindingTarget {
        public event Action OnForceUpdateProperties;

        public event Action OnLinkedGameObjectChanged;

        public GameObject LinkedGameObject {
            get { return _linkedGameObject; }
            set {
                _linkedGameObject = value;

                OnLinkedGameObjectChanged.TryCatchCall();
            }
        }

        private GameObject _linkedGameObject = null;

        protected void ForceUpdateProperties() => OnForceUpdateProperties?.Invoke();
        MethodInfo IBindingTarget.GetMethod(string name) => BindingTargetFlatmap.GetMethod(this, name);
        IProperty IBindingTarget.GetProperty(string name) => BindingTargetFlatmap.GetProperty(this, name);
        bool IBindingTarget.Editor_HasProperty(string name) => BindingTargetFlatmap.GetPropertyFieldInfo(this.GetType(), name) != null;
        Dictionary<string, FieldInfo> IBindingTarget.Editor_GetPropertiesFlatmap() => BindingTargetFlatmap.GetCachedFlatmap(GetType()).Properties;
    }
}