using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class ImageFillBinding : BaseBinding<FloatProperty> {
        private Image _component;

        protected override void Awake() {
            _component = GetComponent<Image>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _component.fillAmount = Property.Value;
        }
    }
}