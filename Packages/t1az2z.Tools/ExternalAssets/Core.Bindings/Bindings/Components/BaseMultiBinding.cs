using System.Collections.Generic;
using System.Text;
using Binding.Base;
using Core.Bindings.Tools.Extensions;
using Core.Bindings.Tools.Helpers;
using UnityEngine;

namespace Core.Bindings.Components {
    public abstract class BaseMultiBinding : MonoBehaviour {
        [SerializeField, BindingProperty] private string[] Paths;

        private IProperty[] _propertiesArray;
        private System.Action[] _delegates;
        private IBindingTarget _target;

        protected IProperty[] properties => _propertiesArray;
        protected IBindingTarget target => _target;
        
        protected virtual void Awake() {
            CaptureProperties();
            Bind();
        }

        private void OnDestroy() {
            Unbind();
        }

        protected abstract void OnValueChanged(IProperty property, int index);

        protected virtual void OnForceUpdate() {
            Unbind();
            CaptureProperties();
            Bind();
        }

        protected void Bind() {
            for (int i = 0; i < _propertiesArray.Length; ++i) {
                var self = this;
                var index = i;
                _delegates[i] = () => { self.OnValueChanged(self._propertiesArray[index], index); };
                _propertiesArray[i].OnValueChanged += _delegates[i];
            }

            if (_target != null) {
                _target.OnForceUpdateProperties += OnForceUpdate;
            }

            OnValueCaptured();
        }

        protected virtual void OnValueCaptured() { }

        protected void Unbind() {
            if (_propertiesArray != null) {
                for (int i = 0; i < _propertiesArray.Length; ++i) {
                    _propertiesArray[i].OnValueChanged -= _delegates[i];
                    _delegates[i] = null;
                }
            }

            if (_target != null) {
                _target.OnForceUpdateProperties -= OnForceUpdate;
            }
        }

        private void CaptureProperties() {
            _propertiesArray = new IProperty[Paths.Length];
            _delegates = new System.Action[Paths.Length];
            if (Paths.Length == 0) {
                return;
            }

            var found = StringTools.GetTempPreallocatedHashSet();
            var parent = transform;
            while (parent != null) {
                var target = parent.GetComponent<IBindingTarget>();
                if (target != null) {
                    _target = target;
                    GrabProperties(target, found);
                    if (AllCollected()) {
                        return;
                    }
                }

                parent = parent.parent;
            }
#if UNITY_EDITOR
            Debug.LogErrorFormat($"[{nameof(BaseMultiBinding)}] Properties wasn't found for \"{GetType().Name}\" \nTransform: \"{GetFullPath()}\"");
            for (int i = 0; i < Paths.Length; ++i) {
                if (_propertiesArray[i] == null) {
                    Debug.LogError($"{i}: {Paths[i]}");
                }
            }
#endif
        }

        private bool AllCollected() {
            foreach (var itr in _propertiesArray) {
                if (itr == null) {
                    return false;
                }
            }

            return true;
        }

        private string GetFullPath() {
            var builder = new StringBuilder();
            builder.Append(transform.name);

            var parent = transform.parent;
            while (parent != null) {
                builder.Insert(0, $"{parent.name}/");
                parent = parent.parent;
            }

            return builder.ToString();
        }

        private void GrabProperties(IBindingTarget target, HashSet<string> foundSet) {
            foreach (var itr in Paths) {
                if (!foundSet.Contains(itr)) {
                    var property = target.GetProperty(itr);
                    if (property != null) {
                        _propertiesArray[Paths.FindFirst(itr)] = property;
                        foundSet.Add(itr);
                    }
                }
            }
        }
    }
}