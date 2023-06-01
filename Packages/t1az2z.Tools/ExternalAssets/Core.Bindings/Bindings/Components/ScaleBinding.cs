using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class ScaleBinding : BaseBinding<Vector2Property> {
        [SerializeField] private WriteBackPolicyEnum WriteBack = WriteBackPolicyEnum.OnBindIfZero;
        
        private RectTransform _rectTransform;
        private bool _initialWritten;

        protected override void Awake() {
            _rectTransform = transform as RectTransform;
            _initialWritten = false;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (_initialWritten == false) {
                switch (WriteBack) {
                    case WriteBackPolicyEnum.OnBindIfZero:
                        if (Property.Value == default) 
                            Property.Value = _rectTransform.localScale;
                        break;
                    case WriteBackPolicyEnum.OnBindAlways:
                        Property.Value = _rectTransform.localScale;
                        break;
                }

                _initialWritten = true;
            }
            
            var scale = Property.Value;
            if (scale.x != _rectTransform.localScale.x || scale.y != _rectTransform.localScale.y) {
                _rectTransform.localScale = new Vector3(scale.x, scale.y, _rectTransform.localScale.z);
            }
        }

        public enum WriteBackPolicyEnum {
            DoNotWrite = 0,
            OnBindIfZero = 1,
            OnBindAlways = 2,
        }
    }
}