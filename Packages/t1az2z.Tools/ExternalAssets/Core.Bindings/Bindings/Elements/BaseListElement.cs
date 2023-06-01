using Binding.Base;
using Core.Bindings.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Bindings.Tools.Extensions;
using UnityEngine;

namespace Binding.Elements {
    public abstract class BaseListElement : MonoBehaviour, IBindingTarget, IComparable<BaseListElement> {
        public List<BaseBinding> SizeAffectors;
        public event Action OnForceUpdateProperties;

        public BaseListElementData Data { get; private set; }

        public void Init(BaseListElementData data) {
            OnDataChange(data);

            this.Data = data;

            data.LinkedGameObject = gameObject;

            OnInit();
            ForceUpdateProperties();
        }

        protected virtual void OnInit() { }

        protected virtual void OnDataChange(BaseListElementData newData) { }

        public int CompareTo(BaseListElement other) {
            return Data.Sort.CompareTo(other.Data.Sort);
        }

        public virtual void ButtonClickHandler() {
            Data.ClickFromUI();
        }

        protected void ForceUpdateProperties() => OnForceUpdateProperties?.Invoke();
        MethodInfo IBindingTarget.GetMethod(string name) => BindingTargetFlatmap.GetMethod(this, name) ?? BindingTargetFlatmap.GetMethod(Data, name);
        IProperty IBindingTarget.GetProperty(string name) => BindingTargetFlatmap.GetProperty(this, name) ?? BindingTargetFlatmap.GetProperty(Data, name);
        bool IBindingTarget.Editor_HasProperty(string name) => BindingTargetFlatmap.GetPropertyFieldInfo(this, name) != null || BindingTargetFlatmap.GetPropertyFieldInfo(Data, name) != null;
        Dictionary<string, FieldInfo> IBindingTarget.Editor_GetPropertiesFlatmap() {
            var element = BindingTargetFlatmap.GetCachedFlatmap(GetType()).Properties;

            var hostInfo = Editor_FindPropertyInfo(GetComponentInParent<BaseBinding<IListProperty<BaseListElementData>>>());
            if  ((hostInfo == null) || !hostInfo.FieldType.IsGenericType(typeof(ListProperty<>), out var hostType)) {
                return element;
            }
            var elementType = hostType.GenericTypeArguments[0];

            var data = BindingTargetFlatmap.GetCachedFlatmap(elementType).Properties;
            var combined = new Dictionary<string, FieldInfo>();
            foreach (var pair in element) {
                combined.Add(pair.Key, pair.Value);
            }
            foreach (var pair in data) {
                combined.Add(pair.Key, pair.Value);
            }

            return combined;
        }

        private FieldInfo Editor_FindPropertyInfo(BaseBinding host) {
            if (host == null) {
                return null;
            }

            if (string.IsNullOrEmpty(host.PropertyName)) {
                return null;
            }

            FieldInfo result = null;
            var parent = host.transform;
            while (parent != null) {
                var target = parent.GetComponent<IBindingTarget>();
                if (target != null && !(target is BaseListElement)) {
                    if (target.Editor_GetPropertiesFlatmap().TryGetValue(host.PropertyName, out result)) {
                        return result;
                    }
                }

                parent = parent.parent;
            }

            return result;
        }
    }
}