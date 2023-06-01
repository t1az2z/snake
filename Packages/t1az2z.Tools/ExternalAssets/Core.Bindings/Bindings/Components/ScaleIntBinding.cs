using System;
using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class ScaleIntBinding : BaseBinding<IntProperty> {
        [SerializeField] private ScaleElement[] _elements = null;
        [SerializeField] private bool _useBoundaryValues = false;
        private Vector3 _default;

        protected override void Awake() {
            base.Awake();
            _default = transform.localScale;
        }

        protected override void OnValueUpdated() {
            if (_elements == null || _elements.Length == 0) {
                return;
            }

            var value = Property.Value;

            if (value < _elements[0].Value) {
                SetScale(_useBoundaryValues ? _elements[0].Scale : _default);
                return;
            }
            else if (value > _elements[_elements.Length - 1].Value) {
                SetScale(_useBoundaryValues ? _elements[_elements.Length - 1].Scale : _default);

                return;
            }

            for (int i = 0; i < _elements.Length; i++) {
                var element = _elements[i];
                if (element.Value == value) {
                    SetScale(element.Scale);
                    break;
                }
            }
        }

        private void SetScale(Vector3 scale) {
            transform.localScale = scale;
        }
        
        [Serializable]
        public class ScaleElement {
            public int Value => _value;

            public Vector3 Scale => _scale;

            [SerializeField] private int _value = 0;
            [SerializeField] private Vector3 _scale = Vector3.one;
        }
    }
}