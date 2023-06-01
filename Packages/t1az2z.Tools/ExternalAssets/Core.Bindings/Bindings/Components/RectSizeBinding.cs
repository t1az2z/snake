using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class RectSizeBinding : BaseBinding<Vector2Property> {
        private RectTransform _rectTransform;
        private bool _changing;

        protected override void Awake() {
            _changing = true;
            _rectTransform = transform as RectTransform;
            base.Awake();
            _changing = false;
            OnRectTransformDimensionsChange();
        }

        private void OnRectTransformDimensionsChange() {
            if (_changing || (Property == null)) {
                return;
            }

            _changing = true;
            Property.Value = _rectTransform.sizeDelta;
            _changing = false;
        }

        protected override void OnValueUpdated() {
            if (_changing) {
                return;
            }

            _changing = true;
            _rectTransform.sizeDelta = Property.Value;
            _changing = false;
        }
    }
}