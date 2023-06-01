using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class RectBinding : BaseBinding<RectProperty> {
        private RectTransform _rectTransform;

        protected override void Awake() {
            _rectTransform = transform as RectTransform;
            base.Awake();
            OnRectTransformDimensionsChange();
        }

        private void OnRectTransformDimensionsChange() {
            if (Property != null) {
                Property.Value = _rectTransform.rect;
            }
        }
    }
}