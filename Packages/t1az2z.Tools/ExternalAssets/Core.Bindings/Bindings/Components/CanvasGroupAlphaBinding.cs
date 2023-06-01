using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupAlphaBinding : BaseBinding<FloatProperty> {
        private CanvasGroup _canvasGroup = null;

        protected override void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _canvasGroup.alpha = Property.Value;
        }
    }
}